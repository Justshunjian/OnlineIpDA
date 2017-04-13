namespace OnlineIpDA.ui
{
    partial class QuartzForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuartzForm));
            this.rbOpen = new System.Windows.Forms.RadioButton();
            this.rbClose = new System.Windows.Forms.RadioButton();
            this.dtPicker = new System.Windows.Forms.DateTimePicker();
            this.cb_cyc = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btSetting = new System.Windows.Forms.Button();
            this.lb_quartzNotice = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbOpen
            // 
            this.rbOpen.AutoSize = true;
            this.rbOpen.Checked = true;
            this.rbOpen.Location = new System.Drawing.Point(15, 13);
            this.rbOpen.Name = "rbOpen";
            this.rbOpen.Size = new System.Drawing.Size(47, 16);
            this.rbOpen.TabIndex = 0;
            this.rbOpen.TabStop = true;
            this.rbOpen.Text = "开启";
            this.rbOpen.UseVisualStyleBackColor = true;
            // 
            // rbClose
            // 
            this.rbClose.AutoSize = true;
            this.rbClose.Location = new System.Drawing.Point(103, 13);
            this.rbClose.Name = "rbClose";
            this.rbClose.Size = new System.Drawing.Size(47, 16);
            this.rbClose.TabIndex = 1;
            this.rbClose.Text = "关闭";
            this.rbClose.UseVisualStyleBackColor = true;
            // 
            // dtPicker
            // 
            this.dtPicker.CustomFormat = "HH:mm";
            this.dtPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPicker.Location = new System.Drawing.Point(93, 168);
            this.dtPicker.Name = "dtPicker";
            this.dtPicker.ShowUpDown = true;
            this.dtPicker.Size = new System.Drawing.Size(142, 21);
            this.dtPicker.TabIndex = 2;
            // 
            // cb_cyc
            // 
            this.cb_cyc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_cyc.FormattingEnabled = true;
            this.cb_cyc.Items.AddRange(new object[] {
            "每周日",
            "每周一",
            "每周二",
            "每周三",
            "每周四",
            "每周五",
            "每周六",
            "每天"});
            this.cb_cyc.Location = new System.Drawing.Point(93, 118);
            this.cb_cyc.Name = "cb_cyc";
            this.cb_cyc.Size = new System.Drawing.Size(142, 20);
            this.cb_cyc.TabIndex = 3;
            this.cb_cyc.SelectedIndexChanged += new System.EventHandler(this.cb_cyc_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "周期:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 174);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "时间:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbOpen);
            this.panel1.Controls.Add(this.rbClose);
            this.panel1.Location = new System.Drawing.Point(62, 53);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(154, 44);
            this.panel1.TabIndex = 6;
            // 
            // btSetting
            // 
            this.btSetting.Location = new System.Drawing.Point(105, 221);
            this.btSetting.Name = "btSetting";
            this.btSetting.Size = new System.Drawing.Size(75, 23);
            this.btSetting.TabIndex = 7;
            this.btSetting.Text = "设置";
            this.btSetting.UseVisualStyleBackColor = true;
            this.btSetting.Click += new System.EventHandler(this.btSetting_Click);
            // 
            // lb_quartzNotice
            // 
            this.lb_quartzNotice.AutoSize = true;
            this.lb_quartzNotice.Location = new System.Drawing.Point(38, 18);
            this.lb_quartzNotice.Name = "lb_quartzNotice";
            this.lb_quartzNotice.Size = new System.Drawing.Size(0, 12);
            this.lb_quartzNotice.TabIndex = 8;
            // 
            // QuartzForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 256);
            this.Controls.Add(this.lb_quartzNotice);
            this.Controls.Add(this.btSetting);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_cyc);
            this.Controls.Add(this.dtPicker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "QuartzForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "定时设置";
            this.Load += new System.EventHandler(this.QuartzForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbOpen;
        private System.Windows.Forms.RadioButton rbClose;
        private System.Windows.Forms.DateTimePicker dtPicker;
        private System.Windows.Forms.ComboBox cb_cyc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btSetting;
        private System.Windows.Forms.Label lb_quartzNotice;
    }
}