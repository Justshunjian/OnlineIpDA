using OnlineIpDA.entity;
using OnlineIpDA.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineIpDA
{
    /// <summary>
    /// 文件名:Init.cs
    ///	功能描述:初始化加载类
    ///
    /// 作者:吕凤凯
    /// 创建时间:2016/2/27 17:23:29
    /// 
    /// </summary>
    class Init
    {
        private static List<Email> emails = new List<Email>();

        private static List<Smtp> smtps = new List<Smtp>();

        /// <summary>
        /// 获取已保存的邮件人员信息
        /// </summary>
        /// <returns></returns>
        public static List<Email> getEmails()
        {
            if (emails == null)
            {
                if (!init())
                {
                    return null;
                }
            }

            return emails;
        }

        /// <summary>
        /// 获取已保存的SMTP信息
        /// </summary>
        /// <returns></returns>
        public static List<Smtp> getSmtp()
        {
            if (smtps == null)
            {
                if (!init())
                {
                    return null;
                }
            }

            return smtps;
        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        public static bool init()
        {
            try
            {
                //读取邮件人员信息
                string json = FileHelper.readFile(FileHelper.EMAIL_FILE);
                if (!"".Equals(json))//表示读取到数据
                {
                    emails = (List<Email>)JsonHelper.Deserialize<Email>(json);
                }

                //读取邮件人员信息
                json = FileHelper.readFile(FileHelper.SMTP_FILE);
                if (!"".Equals(json))//表示读取到数据
                {
                    smtps = (List<Smtp>)JsonHelper.Deserialize<Smtp>(json);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
