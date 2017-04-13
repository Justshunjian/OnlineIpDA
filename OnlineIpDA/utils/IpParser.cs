using OnlineIpDA.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineIpDA
{
    /// <summary>
    /// 文件名:IpParser.cs
    ///	功能描述:IP数据库操作类
    ///
    /// 作者:吕凤凯
    /// 创建时间:2016/2/21 14:54:14
    /// 
    /// </summary>
    class IpParser
    {
        public enum LOAD_TP_TYLE
        {
            NONE = 0,
            LOAD_FILE = 1,
            DOWNLOAD = 2,

        }

        private static IpParser instance = new IpParser();
        private IpParser() { }

        public static IpParser getInstance()
        {
            if (instance == null)
                instance = new IpParser();
            return instance;
        }

        private static DataTable dt;

        private static LOAD_TP_TYLE loadType = LOAD_TP_TYLE.NONE;

        private static string ipfilePath;
        private static string DOWNLOAD_DIR = "download";
        private static string DEFAULT_IP_FOLDER = "ipblock";
        
        //各国ip段下载地址
        private static List<string> downloadUrls = new List<string>();
        //国家代码字典
        private static Dictionary<string, string> countrys = new Dictionary<string, string>();
        //国家代码Key
        private static List<string> keys = new List<string>();
        //下载失败的路径
        private static List<string> downloadFails = new List<string>();
        //存放每日的download IP的文件夹
        private static string folderName;

        public static bool bCancel = false;

        #region 获取IP数据库DataTable 对象 getIpDataTable
        /// <summary>
        /// 获取IP数据库DataTable 对象
        /// </summary>
        /// <returns>返回dt</returns>
        public DataTable getIpDataTable()
        {
            //判断是否存在
            try
            {
                if (loadType == LOAD_TP_TYLE.DOWNLOAD)//判断是否从网络下载
                {
                    string d = DateTime.Now.ToString("yyyyMMdd");
                    LogHelper.writeLog(LogHelper.IP_LOG_INFO, string.Format("loadType:{0},folderName={1},d={2}", loadType, folderName, d));
                    if (folderName == null || !folderName.Equals(d))
                    {
                        dt = null;
                    }
                }

                LogHelper.writeLog(LogHelper.IP_LOG_INFO, string.Format("loadType:{0},folderName={1}", loadType,folderName));

                if (dt == null)
                {
                    if (loadType == LOAD_TP_TYLE.DOWNLOAD)//判断是否从网络下载
                    {
                        bool ret = downloadParseIp();
                        if (!ret)
                        {
                            dt = null;
                        }
                        else
                        {
                            LogHelper.writeLog(LogHelper.IP_DB_SELECT, "使用ipblock IP数据库");
                        }
                    }
                    //判断路径的文件是否存在
                    else if (loadType == LOAD_TP_TYLE.LOAD_FILE && ipfilePath != null && File.Exists(ipfilePath))
                    {
                        bool ret = parseIpDBFile(ipfilePath);
                        if (!ret)
                        {
                            dt = null;
                        }
                        else
                        {
                            LogHelper.writeLog(LogHelper.IP_DB_SELECT, "使用纯真IP数据库");
                        }
                    }
                    else
                    {
                        dt = null;
                        ipfilePath = null;
                    }
                }
            }
            catch (Exception)
            {
                dt = null;
            }
            return dt;
        } 
        #endregion

        #region 解析IP数据库文件，并把处理后的数据保存到DataTable中 parseIpDBFile
        /// <summary>
        /// 解析IP数据库文件，并把处理后的数据保存到DataTable中
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>返回解析结果状态</returns>
        public bool parseIpDBFile(string filePath)
        {
            bCancel = false;
            ipfilePath = filePath;

            loadType = LOAD_TP_TYLE.LOAD_FILE;

            dt = new DataTable();
            dt.Columns.Add("ip_start", Type.GetType("System.UInt64"));
            dt.Columns.Add("ip_end", Type.GetType("System.UInt64"));
            dt.Columns.Add("addr", Type.GetType("System.String"));

            StreamReader sr = new StreamReader(filePath, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null && !line.Contains("255.255.255.255") && !bCancel)
            {
                string strPatten = @"([^\s+]+)";
                Regex rex = new Regex(strPatten, RegexOptions.IgnoreCase);
                MatchCollection matches = rex.Matches(line.ToString());
                //Console.WriteLine(string.Format("{0},{1},{2}\t",
                //    matches[0].Groups[0].Value,
                //    matches[1].Groups[0].Value,
                //    matches[2].Groups[0].Value));

                DataRow newRow = dt.NewRow();
                newRow["ip_start"] = IP2ulong(matches[0].Groups[0].Value);
                newRow["ip_end"] = IP2ulong(matches[1].Groups[0].Value);
                newRow["addr"] = matches[2].Groups[0].Value;
                dt.Rows.Add(newRow);
            }

            sr.Close();
            DataView dv = dt.DefaultView;
            dv.Sort = "ip_start asc";
            dt = dv.ToTable();
            LogHelper.writeLog(LogHelper.IP_DB_SELECT, "使用纯真IP数据库");
            return true;
        } 
        #endregion

        #region 初始化IP下载链接 initDownloadUrls
        /// <summary>
        /// 初始化IP下载链接
        /// </summary>
        private void initDownloadUrls(List<string> keys)
        {
            downloadUrls.Clear();
            foreach (string id in keys)
            {
                //downloadUrls.Add(string.Format("http://ipblock.chacuo.net/down/t_txt={0}", id));
                downloadUrls.Add(string.Format("http://ipblock.chacuo.net/down/t_file={0}", id));
            }
        } 
        #endregion

        #region 初始化 init
        /// <summary>
        /// 初始化
        /// </summary>
        public void init()
        {
            countrys.Clear();
            #region 添加国家代码字典
            countrys.Add("c_US", "美国");
            countrys.Add("c_CN", "中国");
            countrys.Add("c_JP", "日本");
            countrys.Add("c_GB", "英国");
            countrys.Add("c_DE", "德国");
            countrys.Add("c_KR", "韩国");
            countrys.Add("c_FR", "法国");
            countrys.Add("c_BR", "巴西");
            countrys.Add("c_CA", "加拿大");
            countrys.Add("c_IT", "意大利");
            countrys.Add("c_AU", "澳大利亚");
            countrys.Add("c_NL", "荷兰");
            countrys.Add("c_RU", "俄罗斯");

            countrys.Add("c_IN", "印度");
            countrys.Add("c_TW", "台湾省");
            countrys.Add("c_SE", "瑞典");
            countrys.Add("c_ES", "西班牙");
            countrys.Add("c_MX", "墨西哥");
            countrys.Add("c_BE", "比利时");
            countrys.Add("c_ZA", "南非");
            countrys.Add("c_PL", "波兰");
            countrys.Add("c_CH", "瑞士");
            countrys.Add("c_AR", "阿根廷");
            countrys.Add("c_ID", "印度尼西亚");
            countrys.Add("c_EG", "埃及");
            countrys.Add("c_CO", "哥伦比亚");
            countrys.Add("c_TR", "土耳其");
            countrys.Add("c_VN", "越南");
            countrys.Add("c_NO", "挪威");
            countrys.Add("c_FI", "芬兰");
            countrys.Add("c_DK", "丹麦");

            countrys.Add("c_UA", "乌克兰");
            countrys.Add("c_AT", "奥地利");
            countrys.Add("c_IR", "伊朗");
            countrys.Add("c_HK", "香港");
            countrys.Add("c_CL", "智利");
            countrys.Add("c_RO", "罗马尼亚");
            countrys.Add("c_CZ", "捷克");
            countrys.Add("c_TH", "泰国");
            countrys.Add("c_SA", "沙特阿拉伯");
            countrys.Add("c_IL", "以色列");
            countrys.Add("c_NZ", "新西兰");
            countrys.Add("c_VE", "委内瑞拉");
            countrys.Add("c_MA", "摩洛哥");
            countrys.Add("c_MY", "马来西亚");
            countrys.Add("c_PT", "葡萄牙");
            countrys.Add("c_IE", "爱尔兰");
            countrys.Add("c_SG", "新加坡");
            countrys.Add("c_EU", "欧洲联盟");

            countrys.Add("c_HU", "匈牙利");
            countrys.Add("c_GR", "希腊");
            countrys.Add("c_PH", "菲律宾");
            countrys.Add("c_PK", "巴基斯坦");
            countrys.Add("c_BG", "保加利亚");
            countrys.Add("c_KE", "肯尼亚");
            countrys.Add("c_AE", "阿拉伯联合酋长国");
            countrys.Add("c_DZ", "阿尔及利亚");
            countrys.Add("c_SC", "塞舌尔");
            countrys.Add("c_TN", "突尼斯");
            countrys.Add("c_PE", "秘鲁");
            countrys.Add("c_KZ", "哈萨克斯坦");
            countrys.Add("c_SK", "斯洛伐克");
            countrys.Add("c_SI", "斯洛文尼亚");
            countrys.Add("c_EC", "厄瓜多尔");
            countrys.Add("c_CR", "哥斯达黎加");
            countrys.Add("c_UY", "乌拉圭");
            countrys.Add("c_LT", "立陶宛");
            countrys.Add("c_RS", "塞尔维亚");
            countrys.Add("c_NG", "尼日利亚");
            countrys.Add("c_HR", "克罗地亚");
            countrys.Add("c_KW", "科威特");
            countrys.Add("c_PA", "巴拿马");
            countrys.Add("c_MU", "毛里求斯");
            countrys.Add("c_BY", "白俄罗斯");

            countrys.Add("c_LV", "拉脱维亚");
            countrys.Add("c_DO", "多米尼加共和国");
            countrys.Add("c_LU", "卢森堡");
            countrys.Add("c_EE", "爱沙尼亚");
            countrys.Add("c_SD", "苏丹");
            countrys.Add("c_GE", "格鲁吉亚");
            countrys.Add("c_AO", "安哥拉");
            countrys.Add("c_BO", "玻利维亚");
            countrys.Add("c_ZM", "赞比亚");
            countrys.Add("c_BD", "孟加拉国");
            countrys.Add("c_PY", "巴拉圭");
            countrys.Add("c_PR", "波多黎各");
            countrys.Add("c_TZ", "坦桑尼亚");
            countrys.Add("c_CY", "塞浦路斯");
            countrys.Add("c_MD", "摩尔多瓦");
            countrys.Add("c_OM", "阿曼");
            countrys.Add("c_IS", "冰岛");
            countrys.Add("c_SY", "叙利亚");
            countrys.Add("c_QA", "卡塔尔");
            countrys.Add("c_BA", "波黑");
            countrys.Add("c_GH", "加纳");
            countrys.Add("c_AZ", "阿塞拜疆");
            countrys.Add("c_MK", "马其顿");

            countrys.Add("c_JO", "约旦");
            countrys.Add("c_SV", "萨尔瓦多");
            countrys.Add("c_IQ", "伊拉克");
            countrys.Add("c_AM", "亚美尼亚");
            countrys.Add("c_MT", "马耳他");
            countrys.Add("c_GT", "危地马拉");
            countrys.Add("c_PS", "巴勒斯坦");
            countrys.Add("c_LK", "斯里兰卡");
            countrys.Add("c_TT", "特立尼达和多巴哥");
            countrys.Add("c_LB", "黎巴嫩");
            countrys.Add("c_NP", "尼泊尔");
            countrys.Add("c_NA", "纳米比亚");
            countrys.Add("c_BH", "巴林");
            countrys.Add("c_HN", "洪都拉斯");
            countrys.Add("c_MZ", "莫桑比克");
            countrys.Add("c_NI", "尼加拉瓜");
            countrys.Add("c_RW", "卢旺达");
            countrys.Add("c_GA", "加蓬");
            countrys.Add("c_AL", "阿尔巴尼亚");

            countrys.Add("c_MO", "澳门");
            countrys.Add("c_LY", "利比亚");
            countrys.Add("c_KG", "吉尔吉斯坦");
            countrys.Add("c_KH", "柬埔寨");
            countrys.Add("c_CU", "古巴");
            countrys.Add("c_CM", "喀麦隆");
            countrys.Add("c_UG", "乌干达");
            countrys.Add("c_SN", "塞内加尔");
            countrys.Add("c_UZ", "乌兹别克斯坦");
            countrys.Add("c_ME", "黑山");
            countrys.Add("c_GU", "关岛");
            countrys.Add("c_JM", "牙买加");
            countrys.Add("c_MN", "蒙古");
            countrys.Add("c_BN", "文莱");
            countrys.Add("c_VG", "英属维尔京群岛");
            countrys.Add("c_RE", "留尼旺");
            countrys.Add("c_CW", "库拉索岛");
            countrys.Add("c_CI", "科特迪瓦");
            countrys.Add("c_KY", "开曼群岛");
            countrys.Add("c_BB", "巴巴多斯");
            countrys.Add("c_MG", "马达加斯加");
            countrys.Add("c_BZ", "伯利兹");
            countrys.Add("c_NC", "新喀里多尼亚");

            countrys.Add("c_HT", "海地");
            countrys.Add("c_MW", "马拉维");
            countrys.Add("c_FJ", "斐济");
            countrys.Add("c_BS", "巴哈马");
            countrys.Add("c_BW", "博茨瓦纳");
            countrys.Add("c_CD", "刚果(金)");
            countrys.Add("c_AF", "阿富汗");
            countrys.Add("c_LS", "莱索托");
            countrys.Add("c_BM", "百慕大群岛");
            countrys.Add("c_ET", "埃塞俄比亚");
            countrys.Add("c_VI", "美属维尔京群岛");
            countrys.Add("c_LI", "列支敦士登");
            countrys.Add("c_ZW", "津巴布韦");
            countrys.Add("c_GI", "直布罗陀");
            countrys.Add("c_SR", "苏里南");
            countrys.Add("c_ML", "马里");
            countrys.Add("c_YE", "也门");
            countrys.Add("c_LA", "老挝");

            countrys.Add("c_TJ", "塔吉克斯坦");
            countrys.Add("c_AG", "安提瓜和巴布达");
            countrys.Add("c_BJ", "贝宁");
            countrys.Add("c_PF", "法属玻利尼西亚");
            countrys.Add("c_KN", "圣基茨和尼维斯");
            countrys.Add("c_GY", "圭亚那");
            countrys.Add("c_BF", "布基纳法索");
            countrys.Add("c_MV", "马尔代夫");
            countrys.Add("c_JE", "泽西岛");
            countrys.Add("c_MC", "摩纳哥");
            countrys.Add("c_PG", "巴布亚新几内亚");
            countrys.Add("c_CG", "刚果");
            countrys.Add("c_SL", "塞拉利昂");
            countrys.Add("c_DJ", "吉布提");
            countrys.Add("c_SZ", "斯威士兰");
            countrys.Add("c_MM", "缅甸");
            countrys.Add("c_MR", "毛里塔尼亚");
            countrys.Add("c_FO", "法罗群岛");
            countrys.Add("c_NE", "尼日尔");
            countrys.Add("c_AD", "安道尔共和国");
            countrys.Add("c_AW", "阿鲁巴");
            countrys.Add("c_BI", "布隆迪");
            countrys.Add("c_SM", "圣马力诺");
            countrys.Add("c_LR", "利比里亚");
            countrys.Add("c_GM", "冈比亚");
            countrys.Add("c_BT", "不丹");
            countrys.Add("c_GN", "几内亚");
            countrys.Add("c_VC", "圣文森特岛");

            countrys.Add("c_BQ", "荷兰加勒比区");
            countrys.Add("c_TG", "多哥");
            countrys.Add("c_GL", "格陵兰");
            countrys.Add("c_CV", "佛得角");
            countrys.Add("c_IM", "马恩岛");
            countrys.Add("c_SO", "索马里");
            countrys.Add("c_GF", "法属圭亚那");
            countrys.Add("c_WS", "西萨摩亚");
            countrys.Add("c_TM", "土库曼斯坦");
            countrys.Add("c_GP", "瓜德罗普");
            countrys.Add("c_MP", "马里亚那群岛");
            countrys.Add("c_VU", "瓦努阿图");
            countrys.Add("c_MQ", "马提尼克");
            countrys.Add("c_GQ", "赤道几内亚");
            countrys.Add("c_SS", "南苏丹");
            countrys.Add("c_VA", "梵蒂冈");
            countrys.Add("c_GD", "格林纳达");
            countrys.Add("c_SB", "所罗门群岛");
            countrys.Add("c_TC", "特克斯和凯科斯群岛");
            countrys.Add("c_DM", "多米尼克");
            countrys.Add("c_TD", "乍得");
            countrys.Add("c_TO", "汤加");
            countrys.Add("c_NR", "瑙鲁");
            countrys.Add("c_ST", "圣多美和普林西比");
            countrys.Add("c_AI", "安圭拉岛");
            countrys.Add("c_MF", "法属圣马丁");
            countrys.Add("c_TV", "图瓦卢");
            countrys.Add("c_CK", "库克群岛");
            countrys.Add("c_FM", "密克罗尼西亚联邦");
            countrys.Add("c_TL", "东帝汶");
            countrys.Add("c_GG", "根西岛");
            countrys.Add("c_CF", "中非共和国");
            countrys.Add("c_GW", "几内亚比绍");
            countrys.Add("c_PW", "帕劳");
            countrys.Add("c_AS", "东萨摩亚(美)");
            countrys.Add("c_PM", "圣皮埃尔和密克隆");
            countrys.Add("c_ER", "厄立特里亚");
            countrys.Add("c_KM", "科摩罗");
            countrys.Add("c_WF", "瓦利斯和富图纳");
            countrys.Add("c_IO", "英属印度洋领地");

            countrys.Add("c_TK", "托克劳");
            countrys.Add("c_MH", "马绍尔群岛");
            countrys.Add("c_KI", "基里巴斯");
            countrys.Add("c_NU", "纽埃");
            countrys.Add("c_NF", "诺福克岛");
            countrys.Add("c_MS", "蒙特塞拉特岛");
            countrys.Add("c_KP", "朝鲜");
            countrys.Add("c_YT", "马约特");
            countrys.Add("c_LC", "圣卢西亚");
            countrys.Add("c_BL", "圣巴泰勒米岛"); 
            #endregion

            #region 添加国家代码字典key
            keys.Add("c_US");
            keys.Add("c_CN");
            keys.Add("c_JP");
            keys.Add("c_GB");
            keys.Add("c_DE");
            keys.Add("c_KR");
            keys.Add("c_FR");
            keys.Add("c_BR");
            keys.Add("c_CA");
            keys.Add("c_IT");
            keys.Add("c_AU");
            keys.Add("c_NL");
            keys.Add("c_RU");

            keys.Add("c_IN");
            keys.Add("c_TW");
            keys.Add("c_SE");
            keys.Add("c_ES");
            keys.Add("c_MX");
            keys.Add("c_BE");
            keys.Add("c_ZA");
            keys.Add("c_PL");
            keys.Add("c_CH");
            keys.Add("c_AR");
            keys.Add("c_ID");
            keys.Add("c_EG");
            keys.Add("c_CO");
            keys.Add("c_TR");
            keys.Add("c_VN");
            keys.Add("c_NO");
            keys.Add("c_FI");
            keys.Add("c_DK");

            keys.Add("c_UA");
            keys.Add("c_AT");
            keys.Add("c_IR");
            keys.Add("c_HK");
            keys.Add("c_CL");
            keys.Add("c_RO");
            keys.Add("c_CZ");
            keys.Add("c_TH");
            keys.Add("c_SA");
            keys.Add("c_IL");
            keys.Add("c_NZ");
            keys.Add("c_VE");
            keys.Add("c_MA");
            keys.Add("c_MY");
            keys.Add("c_PT");
            keys.Add("c_IE");
            keys.Add("c_SG");
            keys.Add("c_EU");

            keys.Add("c_HU");
            keys.Add("c_GR");
            keys.Add("c_PH");
            keys.Add("c_PK");
            keys.Add("c_BG");
            keys.Add("c_KE");
            keys.Add("c_AE");
            keys.Add("c_DZ");
            keys.Add("c_SC");
            keys.Add("c_TN");
            keys.Add("c_PE");
            keys.Add("c_KZ");
            keys.Add("c_SK");
            keys.Add("c_SI");
            keys.Add("c_EC");
            keys.Add("c_CR");
            keys.Add("c_UY");
            keys.Add("c_LT");
            keys.Add("c_RS");
            keys.Add("c_NG");
            keys.Add("c_HR");
            keys.Add("c_KW");
            keys.Add("c_PA");
            keys.Add("c_MU");
            keys.Add("c_BY");

            keys.Add("c_LV");
            keys.Add("c_DO");
            keys.Add("c_LU");
            keys.Add("c_EE");
            keys.Add("c_SD");
            keys.Add("c_GE");
            keys.Add("c_AO");
            keys.Add("c_BO");
            keys.Add("c_ZM");
            keys.Add("c_BD");
            keys.Add("c_PY");
            keys.Add("c_PR");
            keys.Add("c_TZ");
            keys.Add("c_CY");
            keys.Add("c_MD");
            keys.Add("c_OM");
            keys.Add("c_IS");
            keys.Add("c_SY");
            keys.Add("c_QA");
            keys.Add("c_BA");
            keys.Add("c_GH");
            keys.Add("c_AZ");
            keys.Add("c_MK");

            keys.Add("c_JO");
            keys.Add("c_SV");
            keys.Add("c_IQ");
            keys.Add("c_AM");
            keys.Add("c_MT");
            keys.Add("c_GT");
            keys.Add("c_PS");
            keys.Add("c_LK");
            keys.Add("c_TT");
            keys.Add("c_LB");
            keys.Add("c_NP");
            keys.Add("c_NA");
            keys.Add("c_BH");
            keys.Add("c_HN");
            keys.Add("c_MZ");
            keys.Add("c_NI");
            keys.Add("c_RW");
            keys.Add("c_GA");
            keys.Add("c_AL");

            keys.Add("c_MO");
            keys.Add("c_LY");
            keys.Add("c_KG");
            keys.Add("c_KH");
            keys.Add("c_CU");
            keys.Add("c_CM");
            keys.Add("c_UG");
            keys.Add("c_SN");
            keys.Add("c_UZ");
            keys.Add("c_ME");
            keys.Add("c_GU");
            keys.Add("c_JM");
            keys.Add("c_MN");
            keys.Add("c_BN");
            keys.Add("c_VG");
            keys.Add("c_RE");
            keys.Add("c_CW");
            keys.Add("c_CI");
            keys.Add("c_KY");
            keys.Add("c_BB");
            keys.Add("c_MG");
            keys.Add("c_BZ");
            keys.Add("c_NC");

            keys.Add("c_HT");
            keys.Add("c_MW");
            keys.Add("c_FJ");
            keys.Add("c_BS");
            keys.Add("c_BW");
            keys.Add("c_CD");
            keys.Add("c_AF");
            keys.Add("c_LS");
            keys.Add("c_BM");
            keys.Add("c_ET");
            keys.Add("c_VI");
            keys.Add("c_LI");
            keys.Add("c_ZW");
            keys.Add("c_GI");
            keys.Add("c_SR");
            keys.Add("c_ML");
            keys.Add("c_YE");
            keys.Add("c_LA");

            keys.Add("c_TJ");
            keys.Add("c_AG");
            keys.Add("c_BJ");
            keys.Add("c_PF");
            keys.Add("c_KN");
            keys.Add("c_GY");
            keys.Add("c_BF");
            keys.Add("c_MV");
            keys.Add("c_JE");
            keys.Add("c_MC");
            keys.Add("c_PG");
            keys.Add("c_CG");
            keys.Add("c_SL");
            keys.Add("c_DJ");
            keys.Add("c_SZ");
            keys.Add("c_MM");
            keys.Add("c_MR");
            keys.Add("c_FO");
            keys.Add("c_NE");
            keys.Add("c_AD");
            keys.Add("c_AW");
            keys.Add("c_BI");
            keys.Add("c_SM");
            keys.Add("c_LR");
            keys.Add("c_GM");
            keys.Add("c_BT");
            keys.Add("c_GN");
            keys.Add("c_VC");

            keys.Add("c_BQ");
            keys.Add("c_TG");
            keys.Add("c_GL");
            keys.Add("c_CV");
            keys.Add("c_IM");
            keys.Add("c_SO");
            keys.Add("c_GF");
            keys.Add("c_WS");
            keys.Add("c_TM");
            keys.Add("c_GP");
            keys.Add("c_MP");
            keys.Add("c_VU");
            keys.Add("c_MQ");
            keys.Add("c_GQ");
            keys.Add("c_SS");
            keys.Add("c_VA");
            keys.Add("c_GD");
            keys.Add("c_SB");
            keys.Add("c_TC");
            keys.Add("c_DM");
            keys.Add("c_TD");
            keys.Add("c_TO");
            keys.Add("c_NR");
            keys.Add("c_ST");
            keys.Add("c_AI");
            keys.Add("c_MF");
            keys.Add("c_TV");
            keys.Add("c_CK");
            keys.Add("c_FM");
            keys.Add("c_TL");
            keys.Add("c_GG");
            keys.Add("c_CF");
            keys.Add("c_GW");
            keys.Add("c_PW");
            keys.Add("c_AS");
            keys.Add("c_PM");
            keys.Add("c_ER");
            keys.Add("c_KM");
            keys.Add("c_WF");
            keys.Add("c_IO");

            keys.Add("c_TK");
            keys.Add("c_MH");
            keys.Add("c_KI");
            keys.Add("c_NU");
            keys.Add("c_NF");
            keys.Add("c_MS");
            keys.Add("c_KP");
            keys.Add("c_YT");
            keys.Add("c_LC");
            keys.Add("c_BL"); 
            #endregion

            //初始化
            initDownloadUrls(keys);

        } 
        #endregion

        #region 从网络下载IP数据库 downloadParseIp
        /// <summary>
        /// 从网络下载IP数据库(http://ipblock.chacuo.net/)
        /// </summary>
        /// <returns></returns>
        public bool downloadParseIp()
        {
            bCancel = false;
            loadType = LOAD_TP_TYLE.DOWNLOAD;
            try
            {
                downloadFails.Clear();
                //初始化DataTable
                dt = new DataTable();
                dt.Columns.Add("ip_start", Type.GetType("System.UInt64"));
                dt.Columns.Add("ip_end", Type.GetType("System.UInt64"));
                dt.Columns.Add("addr", Type.GetType("System.String"));

                //判断文件夹download是否存在
                if (!Directory.Exists(DOWNLOAD_DIR))
                {
                    Directory.CreateDirectory(DOWNLOAD_DIR);
                }

                //下载处理
                int j = 0;
                int splitNum = 10;
                int threadNum = 0;
                List<string> tempUrls = new List<string>();
                List<string> tempKeys = new List<string>();
                Dictionary<string, string> tempCountrys = new Dictionary<string, string>();

                string d = DateTime.Now.ToString("yyyyMMdd");
                if (folderName == null || !folderName.Equals(d))
                {
                    folderName = DateTime.Now.ToString("yyyyMMdd");
                }
                
                string path = DOWNLOAD_DIR + "/" + folderName;

                tempUrls = downloadUrls;
                foreach (string k in keys)
                {
                    tempKeys.Add(k);
                }
                foreach (string k in countrys.Keys)
                {
                    tempCountrys.Add(k, countrys[k]);
                }

                //判断文件夹是否存在
                if (Directory.Exists(path))
                {
                    string[] files = Directory.GetFiles(path);
                    if (files.Length != 0)
                    {
                        DirectoryInfo infos = new DirectoryInfo(path);
                        foreach (FileInfo info in infos.GetFiles())
                        {
                            string key = info.Name.Substring(0, info.Name.IndexOf("."));
                            tempKeys.Remove(key);
                            tempCountrys.Remove(key);
                        }

                        initDownloadUrls(tempKeys);
                    }
                }
                else
                {
                    Directory.CreateDirectory(path);

                    initDownloadUrls(keys);
                }

                LogHelper.writeLog(LogHelper.IP_LOG_INFO, "download ipblock folder path:" + path);

                if (tempUrls.Count != 0)
                {
                    //启动线程下载IP各国数据段文件
                    threadNum = tempUrls.Count / splitNum + 1;
                    for (int i = 0; i < threadNum; i++)
                    {
                        string[] urls = new string[splitNum];
                        string[] ids = new string[splitNum];
                        string[] nation = new string[splitNum];
                        int n = i * splitNum + j;
                        for (j = 0; j < splitNum && n < tempUrls.Count; )
                        {
                            urls[j] = tempUrls[n];
                            ids[j] = tempKeys[n];
                            nation[j] = tempCountrys[ids[j]];
                            j++;
                            n = i * splitNum + j;
                        }
                        j = 0;
                        //创建线程
                        DownloadThread dlThread = new DownloadThread(urls, ids, nation, path, dt);
                        Thread t = new Thread(dlThread.execute);
                        t.Priority = ThreadPriority.Highest;
                        t.Start();
                        t.Join();
                    }

                    //下载失败的文件
                    if (downloadFails.Count != 0)
                    {
                        Console.WriteLine("下载失败的链接如下:");
                        StringBuilder sb = new StringBuilder ();
                        foreach (string f in downloadFails)
                        {
                            Console.WriteLine(f);
                            sb.AppendLine(f);
                        }

                        LogHelper.writeLog(LogHelper.IP_DOWNLOAD_LOG, sb.ToString());
                    }
                }

                //解析
                downloadFails.Clear();
                string[] ipFiles = Directory.GetFiles(path);
                //判断下载的IP文件数量如果不等于235个，那么使用默认的ipblock文件夹中的IP文件
                if (ipFiles.Length != 235)
                {
                    //判断默认ipblock IP库是否存在
                    if (Directory.Exists(DEFAULT_IP_FOLDER))
                    {
                        ipFiles = Directory.GetFiles(DEFAULT_IP_FOLDER);
                        if (ipFiles.Length == 0)
                        {
                            return false;
                        }
                        else
                        {
                            LogHelper.writeLog(LogHelper.IP_IPBLOCK, "使用本地默认的" + DEFAULT_IP_FOLDER);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    LogHelper.writeLog(LogHelper.IP_IPBLOCK, "使用网络下载的" + DEFAULT_IP_FOLDER);
                }

                splitNum = 10;
                threadNum = ipFiles.Length / splitNum + 1;
                j = 0;
                for (int i = 0; i < threadNum; i++)
                {
                    string[] files = new string[splitNum];
                    string[] nation = new string[splitNum];
                    int n = i * splitNum + j;
                    for (j = 0; j < splitNum && n < ipFiles.Length; )
                    {
                        string name = ipFiles[n];
                        files[j] = name;
                        //截取文件名字
                        string file = name.Substring(name.LastIndexOf(@"\"));
                        string key = file.Substring(1, file.IndexOf(".") - 1);
                        nation[j] = countrys[key];
                        j++;
                        n = i * splitNum + j;
                    }
                    j = 0;
                    //创建线程
                    IpParseThread parseThread = new IpParseThread(files, nation, dt);
                    Thread t = new Thread(parseThread.execute);
                    t.Priority = ThreadPriority.Highest;
                    t.Start();
                    t.Join();
                }

                //解析失败的文件
                if (downloadFails.Count != 0)
                {
                    Console.WriteLine("解析失败文件如下:");
                    StringBuilder sb = new StringBuilder();
                    foreach (string f in downloadFails)
                    {
                        Console.WriteLine(f);
                        sb.AppendLine(f);
                    }

                    LogHelper.writeLog(LogHelper.IP_PARSE_LOG, sb.ToString());
                }

                DataView dv = dt.DefaultView;
                dv.Sort = "ip_start asc";
                dt = dv.ToTable();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #region IP数据库下载线程 DownloadThread
        /// <summary>
        /// IP数据库下载线程
        /// </summary>
        class DownloadThread
        {
            private string[] mUrls;
            private string[] mIds;
            private string[] mNations;
            private string mPath;
            private DataTable dt;
            public DownloadThread(string[] urls, string[] ids, string[] nations, string path, DataTable d)
            {
                this.mUrls = urls;
                this.mIds = ids;
                this.mNations = nations;
                this.mPath = path;
                this.dt = d;
            }

            public void execute()
            {
                //进行下载
                for (int i = 0; i < mUrls.Length && !bCancel; i++)
                {
                    int downloadCount = 0;
                    if (mUrls == null || mUrls[i] == null)
                    {
                        break ;
                    }

                    do
                    {
                        try
                        {
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mUrls[i]);
                            request.Timeout = 10000;
                            request.Referer = mUrls[i];
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                            string result = reader.ReadToEnd();
                            if (result == null)
                            {
                                throw new Exception("结果result为空");
                            }
                            FileHelper.writeFile(mPath + "/" + mIds[i] + ".txt", result);
                            Console.WriteLine(string.Format("{0}下载成功..", mUrls[i]));
                            reader.Close();
                            break;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(string.Format("{0}下载失败..", mUrls[i]));
                            downloadCount++;
                            Thread.Sleep(500);
                        }

                    } while (downloadCount < 5 && !bCancel);

                    if (downloadCount >= 5)
                    {
                        downloadFails.Add(mUrls[i]);
                    }
                }
            }
        }
        #endregion

        #region IP解析线程 IpParseThread
        /// <summary>
        /// IP数据库下载线程
        /// </summary>
        class IpParseThread
        {
            private string[] mFils;
            private string[] mNations;
            private DataTable dt;
            public IpParseThread(string[] files, string[] nations, DataTable d)
            {
                this.mFils = files;
                this.mNations = nations;
                this.dt = d;
            }

            public void execute()
            {
                //进行下载
                for (int i = 0; i < mFils.Length && !bCancel; i++)
                {
                    if (mFils == null || mFils[i] == null)
                    {
                        break;
                    }

                    try
                    {
                        FileStream fs = new FileStream(mFils[i], FileMode.Open);
                        StreamReader sr = new StreamReader(fs, false);
                        string line = "";
                        while ((line = sr.ReadLine()) != null)
                        {
                            string strPatten = @"([^\s+]+)";
                            Regex rex = new Regex(strPatten, RegexOptions.IgnoreCase);
                            MatchCollection matches = rex.Matches(line.ToString());

                            DataRow newRow = dt.NewRow();
                            newRow["ip_start"] = instance.IP2ulong(matches[0].Groups[0].Value);
                            newRow["ip_end"] = instance.IP2ulong(matches[1].Groups[0].Value);
                            newRow["addr"] = mNations[i];
                            dt.Rows.Add(newRow);
                        }
                        sr.Close();
                        fs.Close();
                        Console.WriteLine(string.Format("{0}解析成功..", mFils[i]));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(string.Format("{0}解析失败..", mFils[i]));
                        downloadFails.Add(mFils[i]);
                    }
                }
            }
        }
        #endregion 
        #endregion

        #region 转换IP地址为整型
        /// <summary>
        /// 转换IP地址为整型
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public ulong IP2ulong(string ipAddress)
        {
            byte[] bytes = IPAddress.Parse(ipAddress).GetAddressBytes();
            ulong ret = 0;

            foreach (byte b in bytes)
            {
                ret <<= 8;
                ret |= b;
            }
            return ret;
        }
        #endregion
    }
}
