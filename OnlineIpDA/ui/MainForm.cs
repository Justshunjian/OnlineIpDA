using OnlineIpDA.entity;
using OnlineIpDA.ui;
using OnlineIpDA.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineIpDA
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 开始转换时间
        /// </summary>
        private DateTime startTime;
        /// <summary>
        /// 是否正在转换中
        /// </summary>
        public static bool bIpAd = false;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connectionString = "";
        private int mSelectIndex;

        private static MainForm instance = new MainForm();
        private MainForm()
        {
            InitializeComponent();
        }

        public static MainForm getInstance()
        {
            return instance;
        }

        #region 导入解析IP数据库文件 ipDbButton_Click

        public void parseIpDb(object path)
        {

            bool ret = IpParser.getInstance().parseIpDBFile((string)path);

            //回调函数
            parseCallback(ret);

        }

        private void parseCallback(bool parseRest)
        {
            if (parseRest)
            {
                setButtonEnable(true,this.sqlLinkButton);
                setCbEnable(true, this.serverNameCb);
                setCbEnable(true, this.identifyCb);
                setprocessStatusSpext("纯真IP数据库解析成功...");
                setLbText("纯真IP数据库导入成功",this.lb_ipNotice);
            }
            else
            {
                setprocessStatusSpext("纯真IP数据库解析失败...");
                setLbText("纯真IP数据库导入失败", this.lb_ipNotice);
            }
            setButtonEnable(true, this.ipDbButton);
            setButtonEnable(true, this.ipDownloadBut);

        }

        private void ipDbButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "(*.txt)|*.txt";
            fd.Title = "请选择本地IP数据源文件";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                this.ipDbPath.Text = fd.FileName;
                setprocessStatusSpext("正在解析IP数据库文件,请稍后...");

                setButtonEnable(false,this.ipDbButton);
                setButtonEnable(false, this.ipDownloadBut);

                Thread t = new Thread(parseIpDb);
                t.Start(fd.FileName);

            }
        } 
        #endregion

        #region 数据库连接 sqlLinkButton_Click
        /// <summary>
        /// 数据库连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sqlLinkButton_Click(object sender, EventArgs e)
        {
            if (this.sqlLinkButton.Text == "断开")
            {
                SqlDbHelper.close();
                //取消IP Parse线程
                IpParser.bCancel = true;
                //取消统计线程
                IpDAHelper.getInstance().cancelDA(true);

                setButtonText("连接", this.sqlLinkButton);
                setprocessStatusSpext("数据库已断开");
                setButtonEnable(false, this.generateBut);
                setButtonEnable(false, this.generateEmailBut);

                setButtonEnable(true, this.ipDbButton);
                setButtonEnable(true, this.ipDownloadBut);

                setTsmEnable(false, this.sendTimerSetting);
                this.serverNameCb.Enabled = true;
                this.identifyCb.Enabled = true;

                if (this.identifyCb.SelectedIndex == 1)//"Sql Server 身份验证"
                {
                    this.sqlUserName.Enabled = true;
                    this.sqlPwd.Enabled = true;
                }

            }
            else
            {
                if (this.serverNameCb == null || this.serverNameCb.Text.Trim().Length == 0)
                {
                    MessageBox.Show("请选择服务器", "提示");
                    return;
                }

                if (this.identifyCb.SelectedIndex == 0)//"Windows 身份验证"
                {
                    connectionString = string.Format(@"Data Source={0};Initial Catalog=MvcApp_Server_V10;Integrated Security=True",this.serverNameCb.Text);
                }
                else if (this.identifyCb.SelectedIndex == 1)//"Sql Server 身份验证"
                {
                    //connectionString = string.Format(@"Server={0};Database=MvcApp_Server_V10;User ID={1};Password={2}",this.serverNameCb.Text,this.sqlUserName.Text,this.sqlPwd.Text);
                    connectionString = string.Format(@"Data Source={0};Initial Catalog=MvcApp_Server_V10;User ID={1};Password={2}",this.serverNameCb.Text,this.sqlUserName.Text,this.sqlPwd.Text);
                    //Server=58.96.181.68;Database=MvcApp_Server_V10;User ID=sa;Password=ws280-1
                }
                else
                {
                    MessageBox.Show("请选择正确的身份验证方式", "错误警告");
                }

                setprocessStatusSpext("正在连接数据库...");
                setButtonEnable(false, this.sqlLinkButton);

                setCbEnable(false, this.serverNameCb);
                setCbEnable(false, this.identifyCb);

                setTbEnable(false, this.sqlUserName);
                setTbEnable(false, this.sqlPwd);

                Thread t = new Thread(linkDb);
                t.Start(connectionString);


                if (!this.serverNameCb.Items.Contains(this.serverNameCb.Text))
                {
                    this.serverNameCb.Items.Add(this.serverNameCb.Text);
                }
                
            }
        }

        private void linkDb(object connString)
        {
            //操作数据库
            //判断数据库服务器是否存在
            if (SqlDbHelper.open((string)connString))
            {
                setButtonText("断开",this.sqlLinkButton);
                setprocessStatusSpext("数据库已连接");

                setButtonEnable(true, this.generateBut);
                setButtonEnable(true, this.generateEmailBut);

                setButtonEnable(false, this.ipDbButton);
                setButtonEnable(false, this.ipDownloadBut);

                setTsmEnable(true, this.sendTimerSetting);

                IpDAHelper.getInstance().cancelDA(false);
            }
            else
            {
                setprocessStatusSpext("数据库连接失败...");
                MessageBox.Show("数据库连接失败", "警告");

                setCbEnable(true, this.serverNameCb);
                setCbEnable(true, this.identifyCb);

                setButtonEnable(true, this.ipDbButton);
                setButtonEnable(true, this.ipDownloadBut);

                if (mSelectIndex == 1)//"Sql Server 身份验证"
                {
                    setTbEnable(true, this.sqlUserName);
                    setTbEnable(true, this.sqlPwd);
                }
            }
            setButtonEnable(true, this.sqlLinkButton); 
        }
        #endregion

        #region 初始化 MainForm_Load
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //初始化加载数据
            Init.init();

            //加载定时器
            QuartzHelper.initQuartz();

            //读取定时数据
            loadQuartz();

            //读取Log分析文件路径
            LogPath.Text = FileHelper.readFile(FileHelper.LOG_FILE);
            IpDAHelper.logFile = LogPath.Text.Replace("\r\n","");
            //
            IpParser.getInstance().init();

            //初始化数据身份验证
            this.identifyCb.SelectedIndex = 0;

            setprocessStatusSpext("数据库未连接");
            //初始化按钮状态
            setButtonEnable(false, this.sqlLinkButton);
            setButtonEnable(false, this.generateBut);
            setButtonEnable(false, this.generateEmailBut);

            setTsmEnable(false,this.sendTimerSetting);

            //初始化时间
            this.curTimeStatusSp.Text = DateTime.Now.ToString();

            setCbEnable(false, this.serverNameCb);
            setCbEnable(false, this.identifyCb);

            //"Windows 身份验证"
            if (this.identifyCb.SelectedIndex == 0)
            {
                this.sqlUserName.Enabled = false;
                this.sqlPwd.Enabled = false;
            }
        } 
        #endregion

        #region 窗口关闭 MainForm_FormClosing
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定退出吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
            {
                //取消IP Parse线程
                IpParser.bCancel = true;
                //释放sql连接
                SqlDbHelper.close();
                //取消统计线程
                IpDAHelper.getInstance().cancelDA(true);

                this.Dispose();
                //Application.Exit();

                System.Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        } 
        #endregion

        #region 时间操作 TimerCtrl_Tick
        private void TimerCtrl_Tick(object sender, EventArgs e)
        {
            this.curTimeStatusSp.Text = DateTime.Now.ToString();

            //进行时间上提示
            if (bIpAd)
            {
                setprocessStatusSpext(string.Format("正在生成分析结果(耗时:{0})，请等待...", DateStringFromNow()));
            }
        }

        private string DateStringFromNow()
        {
            TimeSpan
            span = DateTime.Now - startTime;

            return Convert.ToDateTime( span.ToString() ).ToString("HH:mm:ss");
        }
        #endregion

        #region 数据库身份验证选择
        private void identifyCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            mSelectIndex = this.identifyCb.SelectedIndex;
            if (mSelectIndex == 0)//"Windows 身份验证"
            {
                this.sqlUserName.Enabled = false;
                this.sqlPwd.Enabled = false;
            }
            else if (mSelectIndex == 1)//"Sql Server 身份验证"
            {
                this.sqlUserName.Enabled = true;
                this.sqlPwd.Enabled = true;
            }
        } 
        #endregion

        #region 生成分析结果 generateBut_Click
        /// <summary>
        /// 生成分析结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateBut_Click(object sender, EventArgs e)
        {
            //开始转换标志
            startTime = DateTime.Now;
            bIpAd = true;
            setButtonEnable(false, this.generateBut);
            setButtonEnable(false, this.generateEmailBut);

            setButtonEnable(false, this.ipDbButton);
            setButtonEnable(false, this.ipDownloadBut);

            setButtonEnable(false, this.LogPathButton);

            setTsmEnable(false,this.sendTimerSetting);
            setprocessStatusSpext("正在生成分析结果(耗时:0)，请等待...");
            //启动线程进行分析结果
            Thread t = new Thread(generate);
            t.Start();

        }

        #region 分析结果并生成图片 generate
        /// <summary>
        /// 分析结果并生成图片
        /// </summary>
        private void generate()
        {
            bool ret = IpDAHelper.getInstance().generateIpDA();
            //结束转换标志
            bIpAd = false;

            if (ret)
            {
                setprocessStatusSpext(string.Format("生成分析结果成功,耗时:{0}", DateStringFromNow()));
                MessageBox.Show("生成分析结果图片成功", "提示");

            }
            else
            {
                setprocessStatusSpext("生成分析结果失败，请尝试重新生成");
                MessageBox.Show("生成分析结果图片失败", "提示");
            }
            setTsmEnable(true, this.sendTimerSetting);
            setButtonEnable(true, this.generateBut);
            setButtonEnable(true, this.generateEmailBut);
            //setButtonEnable(true, this.ipDbButton);
            //setButtonEnable(true, this.ipDownloadBut);

            setButtonEnable(true, this.LogPathButton);

            //打开文件夹
            DirectoryInfo info = new DirectoryInfo(IpDAHelper.DIR + "/" + IpDAHelper.dateDir);
            System.Diagnostics.Process.Start("explorer.exe", info.FullName);
        }
        #endregion
        #endregion

        #region 生成分析结果并邮件发送  generateEmailBut_Click
        /// <summary>
        /// 生成分析结果并邮件发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateEmailBut_Click(object sender, EventArgs e)
        {
            generateEamil(true);
        }

        #region 分析结果生成图片并邮件发送 generate
        /// <summary>
        /// 分析结果生成图片并邮件发送
        /// </summary>
        /// <param name="isShow"></param>
        public void generateEamil(bool isShow)
        {
            if (!SqlDbHelper.isAlive())
            {
                if (isShow)
                {
                    MessageBox.Show("没有连接数据库", "提示");
                }
                
                LogHelper.writeLog(LogHelper.SQL_CONNECT_FAIL, "没有连接数据库");
                return;
            }
            //开始转换标志
            startTime = DateTime.Now;
            bIpAd = true;
            setButtonEnable(false, this.generateBut);
            setButtonEnable(false, this.generateEmailBut);
            setTsmEnable(false, this.emailSetting);
            setTsmEnable(false, this.sendTimerSetting);
            setButtonEnable(false, this.ipDbButton);
            setButtonEnable(false, this.ipDownloadBut);

            setButtonEnable(false, this.LogPathButton);

            setprocessStatusSpext("定时生成分析结果(耗时:0)，请等待...");

            List<Email> emails = Init.getEmails();
            if (null == emails)
            {
                if (isShow)
                {
                    MessageBox.Show("没有邮件人员信息，请先添加邮件人员", "提示");
                }
                LogHelper.writeLog(LogHelper.IP_GENERATE_EMAIL_ERR, "没有邮件人员信息，请先添加邮件人员");
                return;
            }

            //进行分析，并发送邮件
            Thread t = new Thread(generateEamilExecute);
            t.Start(isShow);
        }
        /// <summary>
        /// 分析结果并生成图片
        /// </summary>
        private void generateEamilExecute(object show)
        {
            bool ret = IpDAHelper.getInstance().generateIpDA();
            bool isShow = (bool)show;
            try 
	        {	        
		        if (ret)
                {
                    setprocessStatusSpext(string.Format("定时生成分析结果成功,耗时:{0}", DateStringFromNow()));
                    
                    //判断文件夹是否存在
                    string savePath = IpDAHelper.DIR + "/" + IpDAHelper.dateDir;
                    if (!Directory.Exists(savePath))
                    {
                        bIpAd = false;
                        if (isShow)
                        {
                            MessageBox.Show("存放结果文件夹：" + IpDAHelper.dateDir + "不存在", "提示");
                        }
                        
                        throw new Exception("存放结果文件夹：" + IpDAHelper.dateDir + "不存在");
                    }
                    //进行邮件发送
                    if (EmailHelper.sendEmail(savePath))
                    {
                        //结束转换标志
                        bIpAd = false;
                        setprocessStatusSpext(string.Format("定时生成分析结果并发送邮件成功,总耗时:{0}", DateStringFromNow()));
                        if (isShow)
                        {
                            MessageBox.Show("分析结果发送邮件成功", "提示");
                        }
                        LogHelper.writeLog(LogHelper.IP_GENERATE_EMAIL_SUCCESS, string.Format("在{0}时分析结果发送邮件成功",DateTime.Now ));

                    }
                    else
                    {
                        bIpAd = false;
                        if (isShow)
                        {
                            MessageBox.Show("分析结果发送邮件失败", "提示");
                        }
                        throw new Exception("分析结果发送邮件失败");
                    }
                }
                else
                {
                    throw new Exception("分析以及结果发送邮件失败");
                }
	        }
	        catch (Exception e)
	        {
		        //结束转换标志
                bIpAd = false;
                LogHelper.writeLog(LogHelper.IP_GENERATE_EMAIL_ERR, string.Format("程序运行过程中发生错误,错误信息如下:\n{0}\n发生错误的程序集为:{1}\n发生错误的具体位置为:\n{2}", e.Message, e.Source, e.StackTrace));
                setprocessStatusSpext("定时生成分析结果失败，请尝试重新生成");
            }
            finally
            {
                setButtonEnable(true, this.generateBut);
                setButtonEnable(true, this.generateEmailBut);
                setTsmEnable(true, this.emailSetting);
                setTsmEnable(true, this.sendTimerSetting);
                //setButtonEnable(true, this.ipDbButton);
                //setButtonEnable(true, this.ipDownloadBut);

                setButtonEnable(true, this.LogPathButton);

                //打开文件夹
                DirectoryInfo info = new DirectoryInfo(IpDAHelper.DIR + "/" + IpDAHelper.dateDir);
                System.Diagnostics.Process.Start("explorer.exe", info.FullName);
            }
        }
        #endregion
        #endregion

        #region 人员邮件设置 emailSetting_Click
        /// <summary>
        /// 人员邮件设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void emailSetting_Click(object sender, EventArgs e)
        {
            EmailForm ef = new EmailForm();
            ef.ShowDialog();
        } 
        #endregion

        #region 定时发送设置 sendTimerSetting_Click
        /// <summary>
        /// 定时发送设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendTimerSetting_Click(object sender, EventArgs e)
        {
            QuartzForm qf = new QuartzForm();
            qf.Myevent += setQuartzInfo;
            qf.ShowDialog();
        }
        #endregion

        #region 关于 about_Click
        /// <summary>
        /// 关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void about_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format("版本:1.0\n发布时间:2016/2/26"), "IP分析工具");
        } 
        #endregion

        #region 防止Label控件假死 
        delegate void processStatusDelegate(string str);
        private void setprocessStatusSpext(string str)
        {
            if (label1.InvokeRequired)
            {
                Invoke(new processStatusDelegate(setprocessStatusSpext), new string[] { str });
            }
            else
            {
                this.processStatusSp.Text = str;
            }
        } 
        #endregion

        #region 防止Label控件假死
        delegate void labDelegate(string str,Label lb);
        private void setLbText(string str, Label lb)
        {
            if (label1.InvokeRequired)
            {
                Invoke(new labDelegate(setLbText), new object[] { str, lb });
            }
            else
            {
                lb.Text = str;
            }
        }
        #endregion

        #region 安全操作CheckBox控件
        delegate void cbEnableDelegate(bool enable, ComboBox cb);
        private void setCbEnable(bool enable, ComboBox cb)
        {
            if (label1.InvokeRequired)
            {
                Invoke(new cbEnableDelegate(setCbEnable), new object[] { enable, cb });
            }
            else
            {
                cb.Enabled = enable;
            }
        }
        #endregion

        #region 安全操作TextBox控件
        delegate void tbEnableDelegate(bool enable, TextBox tb);
        private void setTbEnable(bool enable, TextBox tb)
        {
            if (label1.InvokeRequired)
            {
                Invoke(new tbEnableDelegate(setTbEnable), new object[] { enable, tb });
            }
            else
            {
                tb.Enabled = enable;
            }
        }
        #endregion

        #region 安全操作Button控件
        delegate void buttonEnableDelegate(bool enable,Button but);
        private void setButtonEnable(bool enable, Button but)
        {
            if (label1.InvokeRequired)
            {
                Invoke(new buttonEnableDelegate(setButtonEnable), new object[] { enable, but });
            }
            else
            {
                but.Enabled = enable;
            }
        }

        delegate void buttonTextDelegate(string content, Button but);
        private void setButtonText(string content, Button but)
        {
            if (label1.InvokeRequired)
            {
                Invoke(new buttonTextDelegate(setButtonText), new object[] { content, but });
            }
            else
            {
                but.Text = content;
            }
        }
        #endregion

        #region 安全操作ToolStripMenuItem控件
        delegate void tsmEnableDelegate(bool enable, ToolStripMenuItem tsm);
        private void setTsmEnable(bool enable, ToolStripMenuItem tsm)
        {
            if (label1.InvokeRequired)
            {
                Invoke(new tsmEnableDelegate(setTsmEnable), new object[] { enable, tsm });
            }
            else
            {
                tsm.Enabled = enable;
            }
        }
        #endregion


        #region 网络下载IP数据库，并解析
        /// <summary>
        /// 网络下载IP数据库，并解析
        /// </summary>
        /// <param name="path"></param>
        public void downloadParseIpDb()
        {
            bool ret = IpParser.getInstance().downloadParseIp();

            if (ret)
            {
                setButtonEnable(true, this.sqlLinkButton);
                setCbEnable(true, this.serverNameCb);
                setCbEnable(true, this.identifyCb);
                setprocessStatusSpext("ipblock IP数据库下载并解析成功...");
                setLbText("ipblock IP数据库下载导入成功", this.lb_ipNotice);
            }
            else
            {
                setprocessStatusSpext("ipblock IP数据库下载及失败...");
                setLbText("ipblock IP数据库下载导入失败", this.lb_ipNotice);
            }
            setButtonEnable(true, this.ipDbButton);
            setButtonEnable(true, this.ipDownloadBut);
        }

        /// <summary>
        /// 网络下载http://ipblock.chacuo.net/中的IP数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ipDownloadBut_Click(object sender, EventArgs e)
        {
            setprocessStatusSpext("正在下载ipblock IP数据库文件,请稍后...");
            setButtonEnable(false, this.ipDbButton);
            setButtonEnable(false, this.ipDownloadBut);
            Thread t = new Thread(downloadParseIpDb);
            t.Start();
        } 
        #endregion


        #region 窗口最小化到托盘
        private void notifyIcon_Click(object sender, EventArgs e)
        {
            //设置窗体为正常状态  
            this.WindowState = FormWindowState.Normal;  
            //激活窗体  
            this.Activate();  
            //托盘设置为不可见  
            this.notifyIcon.Visible = false;  
            //程序在Window任务栏中显示  
            this.ShowInTaskbar = true;  
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //如果当前状态的状态为最小化，则显示状态栏的程序托盘  
            if (this.WindowState == FormWindowState.Minimized)  
            {  
                //不在Window任务栏中显示  
                this.ShowInTaskbar = false;  
                //使图标在状态栏中显示  
                this.notifyIcon.Visible = true;  
            }  
        } 
        #endregion

        #region 读取定时数据 loadQuartz
        /// <summary>
        /// 读取定时数据
        /// </summary>
        private void loadQuartz()
        {
            try
            {
                List<QuartzTimer> quzrtzs = null;
                //读取邮件人员信息
                string json = FileHelper.readFile(FileHelper.QUARTZ_FILE);
                if (!"".Equals(json))//表示读取到数据
                {
                    quzrtzs = (List<QuartzTimer>)JsonHelper.Deserialize<QuartzTimer>(json);
                }

                if (null == quzrtzs || quzrtzs.Count == 0)
                {
                    throw new Exception();
                }
                QuartzTimer qt = quzrtzs[0];
                

                if (qt.state)
                {
                    this.lb_quartz.Visible = true;
                    if (qt.type == 0)//每天
                    {
                        this.lb_quartz.Text = string.Format("定时：{0}{1}点{2}分 生成分析结果并发送邮件", getCbCycText(7), qt.hour, qt.minute);
                    }
                    else
                    {
                        this.lb_quartz.Text = string.Format("定时：{0}{1}点{2}分 生成分析结果并发送邮件", getCbCycText(qt.weekday), qt.hour, qt.minute);
                    }
                }
                else
                {
                    this.lb_quartz.Visible = false;
                }

            }
            catch (Exception)
            {
                this.lb_quartz.Visible = false;
            }
        }

        private void setQuartzInfo(string value)
        {
            if(!string.IsNullOrEmpty(value)){
                this.lb_quartz.Text = value + " 生成分析结果并发送邮件";
                this.lb_quartz.Visible = true;
            }
            else
            {
                this.lb_quartz.Visible = false;
            }
           
        }

        /// <summary>
        /// 根据id获取周期文本字串
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private string getCbCycText(int index)
        {
            string ret = "";
            switch (index)
            {
                case 0:
                    ret = "每周日";
                    break;
                case 1:
                    ret = "每周一";
                    break;
                case 2:
                    ret = "每周二";
                    break;
                case 3:
                    ret = "每周三";
                    break;
                case 4:
                    ret = "每周四";
                    break;
                case 5:
                    ret = "每周五";
                    break;
                case 6:
                    ret = "每周六";
                    break;
                case 7:
                    ret = "每天";
                    break;
                default:
                    break;
            }

            return ret;
        }
        #endregion

        private void openFolderBut_Click(object sender, EventArgs e)
        {

            // 获取程序的基目录。  System.AppDomain.CurrentDomain.BaseDirectory
            //Console.WriteLine(System.AppDomain.CurrentDomain.BaseDirectory);

            // 获取模块的完整路径。 System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName
            //Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            // 获取和设置当前目录(该进程从中启动的目录)的完全限定目录。 System.Environment.CurrentDirectory
            //Console.WriteLine(System.Environment.CurrentDirectory);

            // 获取应用程序的当前工作目录。 System.IO.Directory.GetCurrentDirectory()
            //Console.WriteLine(System.IO.Directory.GetCurrentDirectory());

            // 获取和设置包括该应用程序的目录的名称。 System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase
            //Console.WriteLine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase);

            // 获取启动了应用程序的可执行文件的路径。 System.Windows.Forms.Application.StartupPath
            //Console.WriteLine(System.Windows.Forms.Application.StartupPath);

            // 获取启动了应用程序的可执行文件的路径及文件名。 System.Windows.Forms.Application.ExecutablePath
            //Console.WriteLine(System.Windows.Forms.Application.ExecutablePath);

            DirectoryInfo info;
            //打开文件夹
            if (Directory.Exists(IpDAHelper.DIR + "/" + IpDAHelper.dateDir))
            {
                info = new DirectoryInfo(IpDAHelper.DIR + "/" + IpDAHelper.dateDir);
            }
            else if (Directory.Exists(IpDAHelper.DIR))
            {
                info = new DirectoryInfo(IpDAHelper.DIR);
            }
            else
            {
                info = new DirectoryInfo(System.Environment.CurrentDirectory);
            }
            
            System.Diagnostics.Process.Start("explorer.exe", info.FullName);
        }

        private void LogPathButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "选择Log文件";
            dialog.Filter = "(*.log)|*.log|(*.txt)|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LogPath.Text = dialog.FileName;
                IpDAHelper.logFile = dialog.FileName;
                //写文件
                FileHelper.writeFile(FileHelper.LOG_FILE, dialog.FileName);
            }
        }
    }
}
