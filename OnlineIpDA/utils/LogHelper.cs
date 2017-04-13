using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineIpDA.utils
{
    /// <summary>
    /// 文件名:LogHelper.cs
    ///	功能描述:Log工具类
    ///
    /// 作者:吕凤凯
    /// 创建时间:2016/2/28 11:34:54
    /// 
    /// </summary>
    class LogHelper
    {
        private static string LOG_DIR = "log";

        public static string IP_LOG_INFO = "logInfo";

        public static string LOG_ERR = "error";

        public static string IP_DOWNLOAD_LOG = "downloadErr";
        public static string IP_PARSE_LOG = "parseIpErr";
        public static string IP_THREAD_LOG = "IpThreadErr";
        public static string IP_ADD_PARSER_THREAD_LOG = "IpAddrParserThreadErr";
        public static string PIC_SAVE_LOG = "picSaveErr";
        public static string IP_GENERATE_EMAIL_ERR = "generateEamilErr";
        public static string IP_GENERATE_EMAIL_SUCCESS = "generateEamilSuccess";

        public static string BACKUP_DB_LOG = "backupDb";
        public static string TABLE_DELETE_LOG = "tableDel";
        
        public static string IP_DA_METHOD= "IpDAMethod";
        public static string IP_DB_NULL = "IpDBNull";

        public static string IP_DB_SELECT = "IpDBSelect";

        public static string IP_IPBLOCK = "ipblock";

        public static string SQL_CONNECT_FAIL = "sqlConnect";

        public static string COMPUTER = "computer_info";

        public static string QUARTZ_EXECUTE = "quartz_execute";

        public static string READ_FILE = "readFile";
        public static void writeLog(string filename,string content)
        {
            //判断文件夹download是否存在
            if (!Directory.Exists(LOG_DIR))
            {
                Directory.CreateDirectory(LOG_DIR);
            }

            string folderName = DateTime.Now.ToString("yyyyMMdd");
            string path = LOG_DIR + "/" + folderName;

            //判断文件夹是否存在
            if (!Directory.Exists(path)){
                Directory.CreateDirectory(path);
            }

            string file = string.Format("{0}/{1}_{2}.txt", path, filename, DateTime.Now.ToString("yyyyMMddhhmmss"));

            FileHelper.writeFile(file, content);
        }

    }
}
