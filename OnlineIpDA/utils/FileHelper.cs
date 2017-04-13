using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineIpDA.utils
{
    /// <summary>
    /// 文件名:FileHelper.cs
    ///	功能描述:文本文件工具类
    ///
    /// 作者:吕凤凯
    /// 创建时间:2016/2/25 14:23:34
    /// 
    /// </summary>
    class FileHelper
    {
        public static string EMAIL_FILE = "emails.json";
        public static string QUARTZ_FILE = "quartz.json";
        public static string SMTP_FILE = "smtp.json";
        public static string LOG_FILE = "log.txt";
        #region 保存文件到硬盘 writeFile
        /// <summary>
        /// 保存文件到硬盘
        /// </summary>
        /// <param name="path">文件的路径名称</param>
        /// <param name="content">文件内容</param>
        /// <returns>保存是否成功</returns>
        public static bool writeFile(string path, string content)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(content);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        } 
        #endregion

        #region 保存文件到硬盘 writeFile
        /// <summary>
        /// 保存文件到硬盘
        /// </summary>
        /// <param name="path">文件的路径名称</param>
        /// <param name="content">文件内容</param>
        /// <returns>保存是否成功</returns>
        public static bool writeFileAppend(string path, string content)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(content);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 读取文件 readFile
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path">读取文件路径</param>
        /// <returns>返回读取的文件内容</returns>
        public static string readFile(string path)
        {
            StringBuilder sb = new StringBuilder();
            try
            {

                FileStream fs = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(fs, false);
                
                string line = "";
                while ((line = sr.ReadLine()) != null )
                {
                    sb.AppendLine(line);
                }
                sr.Close();
                fs.Close();
            }
            catch (Exception)
            {
                return "";
            }

            return sb.ToString();
        } 
        #endregion

        #region 读取文件内容行数 readFileContentRows
        /// <summary>
        /// 读取文件内容行数
        /// </summary>
        /// <param name="path">读取文件路径</param>
        /// <returns>内容行数</returns>
        public static long readFileContentRows(string path, bool cancel)
        {
            long rows = 0;
            try
            {
                //FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                //StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
                //string line = "";
                //while ((line = sr.ReadLine()) != null && !cancel)
                //{
                //    rows++;
                //}
                //sr.Close();
                //fs.Close();

                string toPath = string.Format("{0}\\log_{1}.log", path.Substring(0,path.LastIndexOf("\\")),DateTime.Now.ToString("yyyyMMddhhmmss"));
                bool ret = CopyFile(path, toPath,1024*1024*10);
                if (ret)
                {
                    LogHelper.writeLog(LogHelper.READ_FILE, string.Format("拷贝文件 {0} 成功", toPath));
                    //清空path文件的内容
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    fs.SetLength(0);
                    fs.Close();
                    //读取拷贝文件toPath的内容
                    fs = new FileStream(toPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
                    string line = "";
                    while ((line = sr.ReadLine()) != null && !cancel)
                    {
                        rows++;
                    }
                    sr.Close();
                    fs.Close();
                }
                else
                {
                    LogHelper.writeLog(LogHelper.READ_FILE, string.Format("拷贝文件 {0} 失败", toPath));
                }
            }
            catch (Exception e)
            {
                LogHelper.writeLog(LogHelper.READ_FILE, string.Format("程序运行过程中发生错误,错误信息如下:\n{0}\n发生错误的程序集为:{1}\n发生错误的具体位置为:\n{2}", e.Message, e.Source, e.StackTrace));

            }

            return rows;
        }
        #endregion

        #region 复制大文件 CopyFile
        /// <summary>
        /// 复制大文件
        /// </summary>
        /// <param name="fromPath">源文件的路径</param>
        /// <param name="toPath">文件保存的路径</param>
        /// <param name="eachReadLength">每次读取的长度</param>
        /// <returns>是否复制成功</returns>
        private static bool CopyFile(string fromPath, string toPath, int eachReadLength)
        {
            //将源文件 读取成文件流
            FileStream fromFile = new FileStream(fromPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            //已追加的方式 写入文件流
            FileStream toFile = new FileStream(toPath, FileMode.Append, FileAccess.Write);
            //实际读取的文件长度
            int toCopyLength = 0;
            //如果每次读取的长度小于 源文件的长度 分段读取
            if (eachReadLength < fromFile.Length)
            {
                byte[] buffer = new byte[eachReadLength];
                long copied = 0;
                while (copied <= fromFile.Length - eachReadLength)
                {
                    toCopyLength = fromFile.Read(buffer, 0, eachReadLength);
                    fromFile.Flush();
                    toFile.Write(buffer, 0, eachReadLength);
                    toFile.Flush();
                    //流的当前位置
                    toFile.Position = fromFile.Position;
                    copied += toCopyLength;

                }
                int left = (int)(fromFile.Length - copied);
                toCopyLength = fromFile.Read(buffer, 0, left);
                fromFile.Flush();
                toFile.Write(buffer, 0, left);
                toFile.Flush();

            }
            else
            {
                //如果每次拷贝的文件长度大于源文件的长度 则将实际文件长度直接拷贝
                byte[] buffer = new byte[fromFile.Length];
                fromFile.Read(buffer, 0, buffer.Length);
                fromFile.Flush();
                toFile.Write(buffer, 0, buffer.Length);
                toFile.Flush();

            }
            fromFile.Close();
            toFile.Close();
            return true;
        } 
        #endregion
    }
}
