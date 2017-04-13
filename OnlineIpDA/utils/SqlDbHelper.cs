using OnlineIpDA.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineIpDA
{
    /// <summary>
    /// 文件名:SqlDbHelper.cs
    ///	功能描述:MS SQL工具类
    ///
    /// 作者:吕凤凯
    /// 创建时间:2016/2/20 13:37:22
    /// 
    /// </summary>
    class SqlDbHelper
    {
        private static SqlConnection conn;
        private static string mConnectionString;
        private SqlDbHelper(){}

        /// <summary>
        /// 判断数据库服务器是否存在
        /// </summary>
        /// <returns> true:存在</br> false:不存在</returns>
        public static bool open(string connectionString)
        {
            mConnectionString = connectionString;
            conn = new SqlConnection(connectionString);
            
            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
            
        }

        public static bool isAlive()
        {
            bool ret = false;

            try
            {
                close();
                ret = open(mConnectionString);
            }
            catch (Exception)
            {
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public static void close()
        {
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
                conn = null;
            }
        }

        /// <summary>
        /// 根据参数查询数据库表内容
        /// </summary>
        /// <param name="cmdText">SQL 语句</param>
        /// <param name="values">查询参数</param>
        /// <returns>查询表结果DataTable</returns>
        public static DataTable executeNonQuery(string cmdText, SqlParameter[] values)
        {
            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    if (values == null)
                    {
                        cmd.Parameters.AddRange(new SqlParameter[] { });
                    }
                    else
                    {
                        cmd.Parameters.AddRange(values);
                    }
                        

                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sd.Fill(dt);
                     
                    return dt;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="backUpPath">备份指定路径</param>
        /// <returns></returns>
        public static bool backUpDB(string backUpPath)
        {
            try
            {
                if (SqlDbHelper.isAlive())
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = conn;
                        cmd.CommandText = string.Format(@"backup database MvcApp_Server_V10 to disk='{0}'", backUpPath);
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("备份成功！");
                        LogHelper.writeLog(LogHelper.BACKUP_DB_LOG, "数据库'MvcApp_Server_V10'备份成功");
                        return true;
                    }
                }
                else
                {
                    throw new Exception("数据库连接打开失败");
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("备份失败！");
                LogHelper.writeLog(LogHelper.BACKUP_DB_LOG, string.Format("程序运行过程中发生错误,错误信息如下:\n{0}\n发生错误的程序集为:{1}\n发生错误的具体位置为:\n{2}", e.Message, e.Source, e.StackTrace));
                
            }
            return false;
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="table">表名</param>
        public static void delete(string table)
        {
            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    cmd.CommandText = string.Format(@"delete from {0} where [访问时间] < '{1}'", table,DateTime.Today.ToString());
                    cmd.ExecuteNonQuery();
                    Console.WriteLine(table+"表 删除成功！");
                    LogHelper.writeLog(LogHelper.TABLE_DELETE_LOG, table + "表 删除成功！");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(table+"表 删除失败！");
                LogHelper.writeLog(LogHelper.TABLE_DELETE_LOG+"_"+table, string.Format("程序运行过程中发生错误,错误信息如下:\n{0}\n发生错误的程序集为:{1}\n发生错误的具体位置为:\n{2}", e.Message, e.Source, e.StackTrace));

            }
        }

    }
}
