using dotnetCHARTING.WinForms;
using OnlineIpDA.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineIpDA
{
    /// <summary>
    /// 文件名:IpDAHelper.cs
    ///	功能描述:IP地址分析统计类型
    ///
    /// 作者:吕凤凯
    /// 创建时间:2016/2/22 15:33:14
    /// 
    /// </summary>
    public delegate bool MyDelegate();
    class IpDAHelper
    {
        /// <summary>
        /// 图片存放根文件夹
        /// </summary>
        public static string DIR = "images";
        /// <summary>
        /// 图片存放时间文件夹
        /// </summary>
        public static string dateDir;
        /// <summary>
        /// 应用总量统计结果图片名称
        /// </summary>
        private static string APPS_ALL_RECORD_FILE_NAME = "appsAllRecord.jpg";
        /// <summary>
        /// 应用一周内访问量统计结果图片名称
        /// </summary>
        private static string APPS_RECORD_FILE_NAME = "appsRecord.jpg";
        /// <summary>
        /// 应用一周内Server访问记录结果图片名称
        /// </summary>
        private static string SERVER_ACCESS_NAME = "ServerAccess.jpg";

        public static string logFile;

        /// <summary>
        /// 控制取消DA功能
        /// </summary>
        private bool bCancel = false;

        private static IpDAHelper instance = new IpDAHelper();
        
        private IpDAHelper() { }

        public static IpDAHelper getInstance()
        {
            if (instance == null)
                instance = new IpDAHelper();
            return instance;
        }

        public void cancelDA(bool cancel)
        {
            this.bCancel = cancel;
        }

        /// <summary>
        /// 异步委托生成各种访问量以及IP对应地区访问量等图片
        /// </summary>
        /// <returns>返回操作的结果布尔值</returns>
        public bool generateIpDA()
        {
            MyDelegate mydelegate = new MyDelegate(IpDAMethod);

            //异步执行完成
            bool rest = mydelegate.Invoke();

            return rest;
        }

        //线程函数
        private bool IpDAMethod()
        {
            bool ret = false;
            try
            {
                dateDir = DateTime.Now.ToString("yyyy-MM-dd");

                try
                {
                    //根目录是否存在
                    if (Directory.Exists(DIR))
                    {
                        //上级目录是否存在
                        string savePath = DIR + "/" + dateDir;
                        if (Directory.Exists(savePath))
                        {
                            //删除文件夹中的文件
                            DirectoryInfo fileInfos = new DirectoryInfo(savePath);
                            foreach (FileInfo info in fileInfos.GetFiles())
                            {
                                File.Delete(info.FullName);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    LogHelper.writeLog(LogHelper.IP_DA_METHOD, string.Format("程序运行过程中发生错误,错误信息如下:\n{0}\n发生错误的程序集为:{1}\n发生错误的具体位置为:\n{2}", e.Message, e.Source, e.StackTrace));
                }
                

                //应用使用总量统计图
                ret = chartAppsAllRecord();
                if (!ret)
                {
                    return ret;
                }

                //应用过去一周时间使用量统计图
                ret = chartAppsRecord(7);
                if (!ret)
                {
                    return ret;
                }

                //IP过去一周时对应地区的统计图
                ret = chartIPRecord(7);
                if (!ret)
                {
                    return ret;
                }

                //统计Access表的数据量，如果大于了45w条，就备份数据库到指定路径
                Thread t = new Thread(saveAccessTableInXls);
                t.Start();
                t.Join();

                //分析Log访问记录数
                //ret = parseLogFile(7, logFile);
                //if (!ret)
                //{
                //    return ret;
                //}
                parseLogFile(7, logFile);
            }
            catch (Exception e)
            {
                LogHelper.writeLog(LogHelper.IP_DA_METHOD, string.Format("程序运行过程中发生错误,错误信息如下:\n{0}\n发生错误的程序集为:{1}\n发生错误的具体位置为:\n{2}", e.Message, e.Source, e.StackTrace));
                return false;
            }

            return this.bCancel ? false : ret;
        }

        #region 分析Log访问记录数 parseLogFile
        /// <summary>
        /// 分析Log访问记录数
        /// </summary>
        /// <param name="days"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool parseLogFile(int days, string path)
        {
            bool ret = false;
            do
            {
                try
                {
                    if(string.IsNullOrWhiteSpace(path)){
                        LogHelper.writeLog(LogHelper.PIC_SAVE_LOG + "_" + SERVER_ACCESS_NAME, "Log 路径未设置");
                        break;
                    }
                    DateTime end = DateTime.Now;
                    DateTime start = end;
                    start = start.AddDays(-days);//向前推迟一周
                    long rows = FileHelper.readFileContentRows(path, this.bCancel);

                    DataTable dt = new DataTable();
                    dt.Columns.Add("access", Type.GetType("System.String"));
                    dt.Columns.Add("count", Type.GetType("System.UInt64"));

                    DataRow newRow = dt.NewRow();
                    newRow["access"] = "服务器访问量";
                    newRow["count"] = rows;
                    dt.Rows.Add(newRow);

                    ChartHelper c = new ChartHelper();
                    c.Title = string.Format("服务器在 " + start + "-" + end + " 期间访问量统计");
                    c.YTitle = "访问量";
                    c.PicHight = 500;
                    c.PicWidth = 900;
                    c.SeriesName = "总访问量";
                    c.PhaysicalImagePath = "images";
                    c.DataSource = dt;

                    Chart ct = new Chart();
                    c.CreateColumn(ct);

                    saveDAImage(ct, SERVER_ACCESS_NAME);

                    ret = true;
                }
                catch (Exception e)
                {
                    LogHelper.writeLog(LogHelper.PIC_SAVE_LOG + "_" + SERVER_ACCESS_NAME, string.Format("程序运行过程中发生错误,错误信息如下:\n{0}\n发生错误的程序集为:{1}\n发生错误的具体位置为:\n{2}", e.Message, e.Source, e.StackTrace));
                    return false;
                }
            } while (false);

            return ret;
        } 
        #endregion

        #region 统计Access表的数据量，如果大于了45w条，就备份数据库到指定路径 saveAccessTableInXls
        private void saveAccessTableInXls()
        {
            try
            {
                DataTable dt = SqlDbHelper.executeNonQuery(@"SELECT * FROM [T_Accesses]", null);
                //数据条数大于了45w条，就把数据库备份，并把Accesses表清空
                if (dt.Rows.Count > 45 * 10000)
                {
                    //备份文件名时间戳
                    string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");

                    string bkPath = string.Format(@"E:/backUpDB/MvcApp_Server_V10_{0}.bak", timestamp);

                    if (SqlDbHelper.backUpDB(bkPath))
                    {
                        //清空Accesses表数据
                        SqlDbHelper.delete("[T_Accesses]");
                    }
                }
                else
                {
                    LogHelper.writeLog(LogHelper.IP_LOG_INFO + "_T_Accesses", "据条数小于了45w条，无需就把数据库备份以及把Accesses表清空");
                }
            }
            catch (Exception e)
            {
                LogHelper.writeLog(LogHelper.LOG_ERR, string.Format("程序运行过程中发生错误,错误信息如下:\n{0}\n发生错误的程序集为:{1}\n发生错误的具体位置为:\n{2}", e.Message, e.Source, e.StackTrace));
            }
        } 
        #endregion

        #region 应用使用总量统计图 chartAppsAllRecord
        /// <summary>
        /// 应用使用总量统计图
        /// </summary>
        /// <returns></returns>
        private bool chartAppsAllRecord()
        {
            try
            {
                if (!this.bCancel)
                {

                    ChartHelper c = new ChartHelper();
                    c.Title = string.Format("截止 {0} 应用使用总量统计",DateTime.Now.ToString());
                    //c.XTitle = "应用名称";
                    c.YTitle = "访问量";
                    c.PicHight = 600;
                    c.PicWidth = 1200;
                    c.SeriesName = "总访问量";
                    c.PhaysicalImagePath = "images";
                    c.DataSource = SqlDbHelper.executeNonQuery(@"SELECT [应用名称],[应用使用量]
  FROM T_Apps
  where [索引] != 1
  order by [应用使用量] asc", null);

                    Chart ct = new Chart();
                    c.CreateColumn(ct);

                    saveDAImage(ct, APPS_ALL_RECORD_FILE_NAME);
                }
            }
            catch (Exception e)
            {
                LogHelper.writeLog(LogHelper.PIC_SAVE_LOG + "_" + APPS_ALL_RECORD_FILE_NAME, string.Format("程序运行过程中发生错误,错误信息如下:\n{0}\n发生错误的程序集为:{1}\n发生错误的具体位置为:\n{2}", e.Message, e.Source, e.StackTrace));
                return false;
            }

            return true;
        } 
        #endregion

        #region 应用过去一周时间使用量统计图 chartAppsRecord
        /// <summary>
        /// 应用过去一周时间使用量统计图
        /// </summary>
        /// <param name="days">统计时间段</param>
        /// <returns>返回操作的结果布尔值</returns>
        private bool chartAppsRecord(int days)
        {
            try
            {
                if (!this.bCancel)
                {
                    DateTime end = DateTime.Now;
                    DateTime start = end;
                    start = start.AddDays(-days);//向前推迟一周

                    SqlParameter[] param = new SqlParameter[] { new SqlParameter("@start", start), new SqlParameter("@end", end) };

                    DataTable dt = SqlDbHelper.executeNonQuery(@"SELECT app.应用名称,count(app.[索引]) as num
  FROM [T_Apps] as app
  join [T_Accesses] as acc
  on app.索引 = acc.应用索引
  where acc.访问时间 >= @start and acc.访问时间 <= @end and 应用索引 != 1
  group by app.应用名称
  order by num asc", param);

                    //生成图片，并保存
                    ChartHelper c = new ChartHelper();
                    c.Title = "应用在 " + start + "-" + end + " 期间使用情况统计(未出现的应用表示这期间没有访问量)";
                    //c.XTitle = "应用名称";
                    c.YTitle = "访问量";
                    c.PicHight = 500;
                    c.PicWidth = 900;
                    c.SeriesName = "总访问量";
                    c.PhaysicalImagePath = "images";
                    c.DataSource = dt;
                    Chart ct = new Chart();
                    c.CreateColumn(ct);
                    saveDAImage(ct, APPS_RECORD_FILE_NAME);
                }
            }
            catch (Exception e)
            {
                LogHelper.writeLog(LogHelper.PIC_SAVE_LOG + "_" + APPS_RECORD_FILE_NAME, string.Format("程序运行过程中发生错误,错误信息如下:\n{0}\n发生错误的程序集为:{1}\n发生错误的具体位置为:\n{2}", e.Message, e.Source, e.StackTrace));
                return false;
            }
            return true;
        } 
        #endregion

        #region IP过去一周时对应地区的统计图 chartIPRecord
        /// <summary>
        /// IP过去一周时对应地区的统计图
        /// </summary>
        /// <returns></returns>
        private bool chartIPRecord(int days)
        {
            bool ret = false;

            try
            {
                //查数据库
                DateTime end = DateTime.Now;
                DateTime start = end;
                start = start.AddDays(-days);//向前推迟一周

                SqlParameter[] param = new SqlParameter[] { new SqlParameter("@start", start), new SqlParameter("@end", end) };

                DataTable ipSqlDt = SqlDbHelper.executeNonQuery(@"SELECT app.应用名称
      ,acc.[访问者IP]
	  ,count(acc.[访问者IP]) as 访问次数
  FROM [T_Accesses] as acc
  left join [T_Apps] as app
  on app.索引 = acc.应用索引
  where acc.访问时间 >= @start and acc.访问时间 <= @end and app.索引 != 1
  group by app.应用名称,acc.[访问者IP]", param);

                //查找APP应用数据库
                DataTable appsDt = SqlDbHelper.executeNonQuery(@"SELECT [应用名称] FROM [T_Apps] where [索引] != 1 ", null);

                //启动线程
                foreach (DataRow row in appsDt.Rows)
                {
                    string app = row.ItemArray[0].ToString();
                    //根据应用名字过滤
                    DataRow[] rows = ipSqlDt.Select(string.Format("应用名称='{0}'", app));
                    if (rows.Length != 0)
                    {
                        //启动线程，按照应用名字对应数据进行分析
                        AppIpAddrParser appIpAddrParser = new AppIpAddrParser(rows, app,start,end);
                        Thread t = new Thread(appIpAddrParser.execute);
                        t.Priority = ThreadPriority.Highest;
                        t.Start();
                        t.Join();
                    }
                }

                ret = true;
            }
            catch (Exception)
            {
                ret = false;
            }

            return ret;
        }

        class AppIpAddrParser
        {
            private DataRow[] mRows;
            private string mApp;
            private DateTime mStart;
            private DateTime mEnd;
            public AppIpAddrParser(DataRow[] rows, string app, DateTime start, DateTime end)
            {
                this.mRows = rows;
                this.mApp = app;
                this.mStart = start;
                this.mEnd = end;
                Console.WriteLine(string.Format("{0}:{1}", mApp, rows.Length));
            }

            public void execute()
            {
                try
                {
                    //存放结果的DataTable
                    DataTable dt = new DataTable();
                    dt.Columns.Add("addr", Type.GetType("System.String"));
                    dt.Columns.Add("count", Type.GetType("System.UInt64"));
                    dt.Columns.Add("name", Type.GetType("System.String"));

                    //根据数据条数启动多条线程
                    int splitNum = 200;
                    int threadNum = mRows.Length / splitNum + 1;
                    int j = 0;
                    for (int i = 0; i < threadNum; i++)
                    {
                        DataTable newDt = mRows.CopyToDataTable().Clone();

                        int n = i * splitNum + j;
                        for (j = 0; j < splitNum && n < mRows.Length; )
                        {
                            DataRow r = newDt.NewRow();
                            r.ItemArray = mRows[n].ItemArray;
                            newDt.Rows.Add(r);
                            j++;
                            n = i * splitNum + j;
                        }
                        j = 0;
                        DataRow[] newRows = newDt.Select();

                        IpThread ipThread = new IpThread(newRows, mApp, dt);
                        Thread t = new Thread(ipThread.execute);
                        t.Priority = ThreadPriority.Highest;
                        t.Start();
                        t.Join();
                    }

                    //判断dt是否有数据
                    if (dt.Rows.Count > 0)
                    {
                        //生成图片，并保存
                        DataView dv = dt.DefaultView;
                        dv.Sort = "count desc";
                        dt = dv.ToTable();
                        string title = string.Format("{0}应用在 " + mStart + "-" + mEnd + " 期间使用情况统计", mApp);
                        instance.chartIpPic(title, mApp, dt);
                    }
                    else
                    {
                        LogHelper.writeLog(LogHelper.PIC_SAVE_LOG + "_" + mApp,"No Data");
                    }
                }
                catch (Exception e)
                {
                    LogHelper.writeLog(LogHelper.IP_ADD_PARSER_THREAD_LOG + "_" + mApp, string.Format("程序运行过程中发生错误,错误信息如下:\n{0}\n发生错误的程序集为:{1}\n发生错误的具体位置为:\n{2}", e.Message, e.Source, e.StackTrace));
                }
            }
        }

        class IpThread
        {
            private DataRow[] mRows;
            private DataTable mIpDt;
            private string mApp;
            private DataTable dt;
            public IpThread(DataRow[] rows, string app, DataTable d)
            {
                this.mRows = rows;
                this.mApp = app;
                this.dt = d;
            }

            public void execute()
            {
                //ip数据源
                mIpDt = IpParser.getInstance().getIpDataTable();
                if (mIpDt == null || mIpDt.Rows.Count == 0)
                {
                    LogHelper.writeLog(LogHelper.IP_DB_NULL + "_" + mApp, "IP 数据库为空，请修复!!");
                    return;
                }

                //对数据库中IP归属地进行分析
                foreach (DataRow row in mRows)
                {
                    //判断是否取消统计
                    if (instance.bCancel)
                    {
                        return;
                    }

                    try
                    {
                        //string appName = row.ItemArray[0].ToString();
                        string ip = row.ItemArray[1].ToString();
                        UInt64 count = UInt64.Parse(row.ItemArray[2].ToString());
                        UInt64 u64Ip = ip2ulong(ip);

                        //比对
                        //查找ip在数据库中的归属地，并把结果放置DataRow 数组对象 ipSel
                        DataRow[] arrIpFilter = mIpDt.Select(string.Format("ip_start<={0} and ip_end >={1}", u64Ip, u64Ip));

                        //说明该IP出现在两处地址
                        if (arrIpFilter.Length > 1)
                        {
                            //出错
                        }
                        else if (arrIpFilter.Length == 1)
                        {
                            //如果已有该Key，直接操作
                            string addr = arrIpFilter[0].ItemArray[2].ToString();//ip归属地
                            ////查找存放结果的dt中是否存储 应用和城市对应的信息
                            DataRow[] drArr = dt.Select(string.Format("name='{0}' and addr='{1}'", mApp, addr));
                            //不存在记录，就先添加
                            if (drArr.Length == 0)
                            {
                                DataRow newRow = dt.NewRow();
                                newRow["name"] = mApp;
                                newRow["count"] = count;
                                newRow["addr"] = addr;
                                dt.Rows.Add(newRow);
                            }
                            else
                            {
                                //直接修改数据
                                DataRow dr = drArr[0];
                                dr.BeginEdit();
                                UInt64 cnt = UInt64.Parse(dr.ItemArray[1].ToString());
                                dr["count"] = cnt + count;
                                dr.EndEdit();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.writeLog(LogHelper.IP_THREAD_LOG + "_" + mApp, string.Format("程序运行过程中发生错误,错误信息如下:\n{0}\n发生错误的程序集为:{1}\n发生错误的具体位置为:\n{2}",e.Message,e.Source,e.StackTrace));
                    }
                }
            }

            #region 转换IP地址为整型
            /// <summary>
            /// 转换IP地址为整型
            /// </summary>
            /// <param name="ip"></param>
            /// <returns></returns>
            private ulong ip2ulong(string ipAddress)
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

        private void chartIpPic(string title,string fileName,DataTable dt){
            ChartHelper c = new ChartHelper();
            c.Title = title;
            //c.XTitle = "应用名称";
            c.YTitle = "访问量";
            if (dt.Rows.Count < 10)
            {
                c.PicHight = 400;
                c.PicWidth = 800;
            }
            else if (dt.Rows.Count < 50)
            {
                c.PicHight = 600;
                c.PicWidth = 1200;
            }
            else
            {
                c.PicHight = 1400;
                c.PicWidth = 2000;
            }

            c.SeriesName = "总访问量";
            c.PhaysicalImagePath = "images";
            c.DataSource = dt;
            Chart ct = new Chart();
            c.CreateColumn(ct);
            saveDAImage(ct, fileName+".jpg");
        }
        #endregion

        #region 保存图片 saveDAImage
        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="chart">图片流</param>
        /// <param name="filename">图片保存名字</param>
        private void saveDAImage(Chart chart, string filename)
        {
            try
            {
                Image _image = chart.GetChartBitmap() as Image;
                //根目录是否存在
                if (!Directory.Exists(DIR))
                {
                    Directory.CreateDirectory(DIR);
                }
                //上级目录是否存在
                string savePath = DIR + "/" + dateDir;
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                string filePath = savePath + "/" + filename;
                _image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                _image.Dispose();
            }
            catch (Exception e)
            {
                LogHelper.writeLog(LogHelper.PIC_SAVE_LOG + "_" + filename, string.Format("程序运行过程中发生错误,错误信息如下:\n{0}\n发生错误的程序集为:{1}\n发生错误的具体位置为:\n{2}",e.Message,e.Source,e.StackTrace));
            }
        } 
        #endregion
    }
}
