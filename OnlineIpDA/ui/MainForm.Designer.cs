namespace OnlineIpDA
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.ipDbPath = new System.Windows.Forms.TextBox();
            this.ipDbButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sqlLinkButton = new System.Windows.Forms.Button();
            this.sqlPwd = new System.Windows.Forms.TextBox();
            this.sqlUserName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.serverNameCb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.identifyCb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lb_ipNotice = new System.Windows.Forms.Label();
            this.ipDownloadBut = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.statusSp = new System.Windows.Forms.StatusStrip();
            this.curTimeStatusSp = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.processStatusSp = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.openFolderBut = new System.Windows.Forms.Button();
            this.lb_quartz = new System.Windows.Forms.Label();
            this.generateBut = new System.Windows.Forms.Button();
            this.generateEmailBut = new System.Windows.Forms.Button();
            this.TimerCtrl = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.emailSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.sendTimerSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.about = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.LogPathButton = new System.Windows.Forms.Button();
            this.LogPath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusSp.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "纯真IP数据库导入:";
            // 
            // ipDbPath
            // 
            this.ipDbPath.Location = new System.Drawing.Point(122, 36);
            this.ipDbPath.Name = "ipDbPath";
            this.ipDbPath.ReadOnly = true;
            this.ipDbPath.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ipDbPath.Size = new System.Drawing.Size(560, 21);
            this.ipDbPath.TabIndex = 0;
            // 
            // ipDbButton
            // 
            this.ipDbButton.Location = new System.Drawing.Point(688, 34);
            this.ipDbButton.Name = "ipDbButton";
            this.ipDbButton.Size = new System.Drawing.Size(58, 23);
            this.ipDbButton.TabIndex = 2;
            this.ipDbButton.Text = "导入";
            this.ipDbButton.UseVisualStyleBackColor = true;
            this.ipDbButton.Click += new System.EventHandler(this.ipDbButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.sqlLinkButton);
            this.groupBox1.Controls.Add(this.sqlPwd);
            this.groupBox1.Controls.Add(this.sqlUserName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.serverNameCb);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.identifyCb);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(9, 182);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(751, 178);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据库设置";
            // 
            // sqlLinkButton
            // 
            this.sqlLinkButton.Location = new System.Drawing.Point(589, 86);
            this.sqlLinkButton.Name = "sqlLinkButton";
            this.sqlLinkButton.Size = new System.Drawing.Size(107, 23);
            this.sqlLinkButton.TabIndex = 4;
            this.sqlLinkButton.Text = "连接";
            this.sqlLinkButton.UseVisualStyleBackColor = true;
            this.sqlLinkButton.Click += new System.EventHandler(this.sqlLinkButton_Click);
            // 
            // sqlPwd
            // 
            this.sqlPwd.Location = new System.Drawing.Point(95, 144);
            this.sqlPwd.Name = "sqlPwd";
            this.sqlPwd.Size = new System.Drawing.Size(381, 21);
            this.sqlPwd.TabIndex = 3;
            this.sqlPwd.Text = "ws280-1";
            // 
            // sqlUserName
            // 
            this.sqlUserName.Location = new System.Drawing.Point(95, 108);
            this.sqlUserName.Name = "sqlUserName";
            this.sqlUserName.Size = new System.Drawing.Size(381, 21);
            this.sqlUserName.TabIndex = 2;
            this.sqlUserName.Text = "sa";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(57, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "密码:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "登录名:";
            // 
            // serverNameCb
            // 
            this.serverNameCb.FormattingEnabled = true;
            this.serverNameCb.Items.AddRange(new object[] {
            "SZDLVFK\\SQLEXPRESS",
            "58.96.181.68"});
            this.serverNameCb.Location = new System.Drawing.Point(95, 31);
            this.serverNameCb.Name = "serverNameCb";
            this.serverNameCb.Size = new System.Drawing.Size(381, 20);
            this.serverNameCb.TabIndex = 0;
            this.serverNameCb.Text = "SZDLVFK\\SQLEXPRESS";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "服务器名称:";
            // 
            // identifyCb
            // 
            this.identifyCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.identifyCb.FormattingEnabled = true;
            this.identifyCb.Items.AddRange(new object[] {
            "Windows 身份验证",
            "Sql Server 身份验证"});
            this.identifyCb.Location = new System.Drawing.Point(95, 68);
            this.identifyCb.Name = "identifyCb";
            this.identifyCb.Size = new System.Drawing.Size(381, 20);
            this.identifyCb.TabIndex = 1;
            this.identifyCb.SelectedIndexChanged += new System.EventHandler(this.identifyCb_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "身份验证:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lb_ipNotice);
            this.groupBox2.Controls.Add(this.ipDownloadBut);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.ipDbPath);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.ipDbButton);
            this.groupBox2.Location = new System.Drawing.Point(9, 37);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(751, 139);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "IP数据源设置(二选一)";
            // 
            // lb_ipNotice
            // 
            this.lb_ipNotice.AutoSize = true;
            this.lb_ipNotice.ForeColor = System.Drawing.Color.Red;
            this.lb_ipNotice.Location = new System.Drawing.Point(292, 112);
            this.lb_ipNotice.Name = "lb_ipNotice";
            this.lb_ipNotice.Size = new System.Drawing.Size(89, 12);
            this.lb_ipNotice.TabIndex = 5;
            this.lb_ipNotice.Text = "未导入IP数据库";
            // 
            // ipDownloadBut
            // 
            this.ipDownloadBut.Location = new System.Drawing.Point(195, 75);
            this.ipDownloadBut.Name = "ipDownloadBut";
            this.ipDownloadBut.Size = new System.Drawing.Size(75, 23);
            this.ipDownloadBut.TabIndex = 4;
            this.ipDownloadBut.Text = "下载并导入";
            this.ipDownloadBut.UseVisualStyleBackColor = true;
            this.ipDownloadBut.Click += new System.EventHandler(this.ipDownloadBut_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "ipblock IP数据库下载导入:";
            // 
            // statusSp
            // 
            this.statusSp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.curTimeStatusSp,
            this.toolStripStatusLabel1,
            this.processStatusSp});
            this.statusSp.Location = new System.Drawing.Point(0, 556);
            this.statusSp.Name = "statusSp";
            this.statusSp.Size = new System.Drawing.Size(767, 26);
            this.statusSp.TabIndex = 6;
            // 
            // curTimeStatusSp
            // 
            this.curTimeStatusSp.Name = "curTimeStatusSp";
            this.curTimeStatusSp.Size = new System.Drawing.Size(0, 21);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(720, 21);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // processStatusSp
            // 
            this.processStatusSp.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.processStatusSp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.processStatusSp.Name = "processStatusSp";
            this.processStatusSp.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.processStatusSp.Size = new System.Drawing.Size(32, 21);
            this.processStatusSp.Text = "few";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.openFolderBut);
            this.groupBox3.Controls.Add(this.lb_quartz);
            this.groupBox3.Controls.Add(this.generateBut);
            this.groupBox3.Controls.Add(this.generateEmailBut);
            this.groupBox3.Location = new System.Drawing.Point(9, 448);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(751, 99);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "IP地址分析操作";
            // 
            // openFolderBut
            // 
            this.openFolderBut.Location = new System.Drawing.Point(112, 29);
            this.openFolderBut.Name = "openFolderBut";
            this.openFolderBut.Size = new System.Drawing.Size(103, 23);
            this.openFolderBut.TabIndex = 3;
            this.openFolderBut.Text = "打开结果文件夹";
            this.openFolderBut.UseVisualStyleBackColor = true;
            this.openFolderBut.Click += new System.EventHandler(this.openFolderBut_Click);
            // 
            // lb_quartz
            // 
            this.lb_quartz.AutoSize = true;
            this.lb_quartz.Location = new System.Drawing.Point(229, 73);
            this.lb_quartz.Name = "lb_quartz";
            this.lb_quartz.Size = new System.Drawing.Size(53, 12);
            this.lb_quartz.TabIndex = 2;
            this.lb_quartz.Text = "定时信息";
            // 
            // generateBut
            // 
            this.generateBut.Location = new System.Drawing.Point(294, 29);
            this.generateBut.Name = "generateBut";
            this.generateBut.Size = new System.Drawing.Size(106, 23);
            this.generateBut.TabIndex = 1;
            this.generateBut.Text = "生成分析结果";
            this.generateBut.UseVisualStyleBackColor = true;
            this.generateBut.Click += new System.EventHandler(this.generateBut_Click);
            // 
            // generateEmailBut
            // 
            this.generateEmailBut.Location = new System.Drawing.Point(481, 29);
            this.generateEmailBut.Name = "generateEmailBut";
            this.generateEmailBut.Size = new System.Drawing.Size(176, 23);
            this.generateEmailBut.TabIndex = 0;
            this.generateEmailBut.Text = "生成分析结果并发送邮件";
            this.generateEmailBut.UseVisualStyleBackColor = true;
            this.generateEmailBut.Click += new System.EventHandler(this.generateEmailBut_Click);
            // 
            // TimerCtrl
            // 
            this.TimerCtrl.Enabled = true;
            this.TimerCtrl.Interval = 1000;
            this.TimerCtrl.Tick += new System.EventHandler(this.TimerCtrl_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.emailSetting,
            this.sendTimerSetting,
            this.about});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(767, 25);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // emailSetting
            // 
            this.emailSetting.Name = "emailSetting";
            this.emailSetting.Size = new System.Drawing.Size(68, 21);
            this.emailSetting.Text = "邮件设置";
            this.emailSetting.Click += new System.EventHandler(this.emailSetting_Click);
            // 
            // sendTimerSetting
            // 
            this.sendTimerSetting.Name = "sendTimerSetting";
            this.sendTimerSetting.Size = new System.Drawing.Size(68, 21);
            this.sendTimerSetting.Text = "定时设置";
            this.sendTimerSetting.Click += new System.EventHandler(this.sendTimerSetting_Click);
            // 
            // about
            // 
            this.about.Name = "about";
            this.about.Size = new System.Drawing.Size(44, 21);
            this.about.Text = "关于";
            this.about.Click += new System.EventHandler(this.about_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "IP地址分析工具";
            this.notifyIcon.Visible = true;
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.LogPathButton);
            this.groupBox4.Controls.Add(this.LogPath);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Location = new System.Drawing.Point(9, 375);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(751, 67);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Log分析设置";
            // 
            // LogPathButton
            // 
            this.LogPathButton.Location = new System.Drawing.Point(676, 27);
            this.LogPathButton.Name = "LogPathButton";
            this.LogPathButton.Size = new System.Drawing.Size(69, 23);
            this.LogPathButton.TabIndex = 2;
            this.LogPathButton.Text = "设置";
            this.LogPathButton.UseVisualStyleBackColor = true;
            this.LogPathButton.Click += new System.EventHandler(this.LogPathButton_Click);
            // 
            // LogPath
            // 
            this.LogPath.Location = new System.Drawing.Point(132, 29);
            this.LogPath.Name = "LogPath";
            this.LogPath.ReadOnly = true;
            this.LogPath.Size = new System.Drawing.Size(536, 21);
            this.LogPath.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "请选择Log文件路径:";
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 582);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.statusSp);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IP地址分析工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusSp.ResumeLayout(false);
            this.statusSp.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ipDbPath;
        private System.Windows.Forms.Button ipDbButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox serverNameCb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox identifyCb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.StatusStrip statusSp;
        private System.Windows.Forms.Button sqlLinkButton;
        private System.Windows.Forms.TextBox sqlPwd;
        private System.Windows.Forms.TextBox sqlUserName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button generateBut;
        private System.Windows.Forms.Button generateEmailBut;
        internal System.Windows.Forms.Timer TimerCtrl;
        private System.Windows.Forms.ToolStripStatusLabel curTimeStatusSp;
        private System.Windows.Forms.ToolStripStatusLabel processStatusSp;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem about;
        private System.Windows.Forms.ToolStripMenuItem emailSetting;
        private System.Windows.Forms.ToolStripMenuItem sendTimerSetting;
        private System.Windows.Forms.Button ipDownloadBut;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lb_ipNotice;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Label lb_quartz;
        private System.Windows.Forms.Button openFolderBut;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button LogPathButton;
        private System.Windows.Forms.TextBox LogPath;
        private System.Windows.Forms.Label label7;
    }
}

