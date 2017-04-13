using OnlineIpDA.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OnlineIpDA.utils
{
    /// <summary>
    /// 文件名:EmailHelper.cs
    ///	功能描述:邮件发送工具类
    ///
    /// 作者:吕凤凯
    /// 创建时间:2016/2/28 10:47:56
    /// 
    /// </summary>
    class EmailHelper
    {
        /// <summary>
        /// 发送者
        /// </summary>
        public string mailFrom { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string[] mailToArray { get; set; }

        /// <summary>
        /// 抄送
        /// </summary>
        public string[] mailCcArray { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string mailSubject { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string mailBody { get; set; }

        /// <summary>
        /// 发件人密码
        /// </summary>
        public string mailPwd { get; set; }

        /// <summary>
        /// SMTP邮件服务器
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// 正文是否是html格式
        /// </summary>
        public bool isbodyHtml { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string[] attachmentsPath { get; set; }

        public bool Send()
        {
            bool ret = false;
            try
            {
                //初始化MailMessage实例
                MailMessage mailMessage = new MailMessage();

                // 邮件服务设置
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
                smtpClient.Host = host; //指定SMTP服务器
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new System.Net.NetworkCredential(mailFrom, mailPwd);//用户名和密码

                // 发送邮件设置
                mailMessage.From = new MailAddress(mailFrom, "Online Server IP分析工具");
                //向收件人地址集合添加邮件地址
                if (mailToArray != null)
                {
                    for (int i = 0; i < mailToArray.Length; i++)
                    {
                        mailMessage.To.Add(mailToArray[i].ToString());
                    }
                }

                //向抄送收件人地址集合添加邮件地址
                if (mailCcArray != null)
                {
                    for (int i = 0; i < mailCcArray.Length; i++)
                    {
                        mailMessage.CC.Add(mailCcArray[i].ToString());
                    }
                }
                //在有附件的情况下添加附件
                try
                {
                    if (attachmentsPath != null && attachmentsPath.Length > 0)
                    {
                        Attachment attachFile = null;
                        foreach (string path in attachmentsPath)
                        {
                            attachFile = new Attachment(path);
                            mailMessage.Attachments.Add(attachFile);
                        }
                    }
                }
                catch (Exception err)
                {
                    throw new Exception("在添加附件时有错误:" + err);
                }
                mailMessage.Subject = mailSubject;//主题
                mailMessage.Body = mailBody;//内容
                mailMessage.BodyEncoding = Encoding.UTF8;//正文编码
                mailMessage.IsBodyHtml = true;//设置为HTML格式
                mailMessage.Priority = MailPriority.Normal;//优先级

                smtpClient.Send(mailMessage); // 发送邮件

                ret = true;
            }
            catch (Exception)
            {
                ret = false;
            }

            return ret;

        }

        /// <summary>
        /// 发送分析结果邮件
        /// </summary>
        /// <returns></returns>
        public static bool sendEmail(string dir)
        {
            List<Smtp> smtps = Init.getSmtp();
            if(null == smtps || smtps.Count == 0){
                return false;
            }

            List<Email> emails = Init.getEmails();
            if (null == emails || emails.Count == 0)
            {
                return false;
            }
            int n = 0;
            for (; n < emails.Count; n++)
            {
                if (emails[n].state == "有效")
                {
                    break;
                }
            }

            if (n >= emails.Count)
            {
                return false;
            }

            //判断文件夹是否存在
            if (!Directory.Exists(dir))
            {
                return false;
            }
            //判断文件夹中是否有文件
            string[] files = Directory.GetFiles(dir);
            if (files.Length == 0)
            {
                return false;
            }
            
            //接收者邮件集合
            List<string> tos = new List<string>();
            for (int i = 0; i < emails.Count; i++)
            {
                if (emails[i].state == "有效")
                {
                    tos.Add(emails[i].email);
                }
            }

            if (tos.Count == 0)
            {
                return false;
            }

            //获取运行电脑的信息
            string computer = getSystemInfo();
            LogHelper.writeLog(LogHelper.COMPUTER, computer);
            EmailHelper email = new EmailHelper();
            email.mailFrom = smtps[0].user;
            email.mailPwd = smtps[0].pwd;
            email.mailSubject = string.Format("Online Server {0}访问记录分析结果_{1}", IpDAHelper.dateDir,DateTime.Now.ToString());
            email.mailBody = string.Format(@"附件是在<br/>{0}<br/>上运行的 'IP地址分析工具' 分析Online Server IP访问记录生成的数据图片，请查收!!!<br/>
                                            详细可以访问<a href='{1}' target='_blank'>{2}</a>",
                                            computer,
                                            "http://www.linkavaiyun.com/",
                                            "http://www.linkavaiyun.com/");
            email.isbodyHtml = true;    //是否是HTML
            email.host = smtps[0].smtp;//如果是QQ邮箱则：smtp:qq.com,依次类推

            email.mailToArray = tos.ToArray();//接收者邮件集合
            //email.mailCcArray = new string[] { };//抄送者邮件集合

            //附件
            email.attachmentsPath = files;

            try
            {
                bool ret = false;
                int cnt = 0;
                do
                {
                    cnt++;
                    ret = email.Send();

                } while (cnt < 5 && !ret);

                return ret;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private static string getSystemInfo()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                //获得主机名
                string HostName = Dns.GetHostName();
                sb.AppendLine("主机名：" + HostName + "<br/>");

                //遍历地址列表，如果电脑有多网卡只能这样遍历显示
                IPAddress[] iplist = Dns.GetHostAddresses(HostName);
                for (int i = 0; i < iplist.Length; i++)
                {
                    sb.AppendLine("IP地址"+(i+1)+"：" + iplist[i] + "<br/>");
                }
            }
            catch (Exception)
            {

            }

            return sb.ToString();
        }
    }
}
