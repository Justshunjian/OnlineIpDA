namespace OnlineIpDA.ui
{
    partial class EmailForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmailForm));
            this.emalListView = new System.Windows.Forms.ListView();
            this.nameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.addrCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.stateCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.emailAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.user = new System.Windows.Forms.TextBox();
            this.email = new System.Windows.Forms.TextBox();
            this.state = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.emailDel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tb_pwd = new System.Windows.Forms.TextBox();
            this.tb_user = new System.Windows.Forms.TextBox();
            this.tb_smtp = new System.Windows.Forms.TextBox();
            this.bt_smtp = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // emalListView
            // 
            this.emalListView.AllowColumnReorder = true;
            this.emalListView.CheckBoxes = true;
            this.emalListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameCol,
            this.addrCol,
            this.stateCol});
            this.emalListView.FullRowSelect = true;
            this.emalListView.GridLines = true;
            this.emalListView.Location = new System.Drawing.Point(3, 2);
            this.emalListView.MultiSelect = false;
            this.emalListView.Name = "emalListView";
            this.emalListView.Size = new System.Drawing.Size(594, 211);
            this.emalListView.TabIndex = 0;
            this.emalListView.UseCompatibleStateImageBehavior = false;
            this.emalListView.View = System.Windows.Forms.View.Details;
            this.emalListView.SelectedIndexChanged += new System.EventHandler(this.emalListView_SelectedIndexChanged);
            // 
            // nameCol
            // 
            this.nameCol.Text = "姓名";
            this.nameCol.Width = 200;
            // 
            // addrCol
            // 
            this.addrCol.Text = "邮件地址";
            this.addrCol.Width = 300;
            // 
            // stateCol
            // 
            this.stateCol.Text = "状态";
            // 
            // emailAdd
            // 
            this.emailAdd.Location = new System.Drawing.Point(96, 136);
            this.emailAdd.Name = "emailAdd";
            this.emailAdd.Size = new System.Drawing.Size(75, 23);
            this.emailAdd.TabIndex = 2;
            this.emailAdd.Text = "提交";
            this.emailAdd.UseVisualStyleBackColor = true;
            this.emailAdd.Click += new System.EventHandler(this.emailAdd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "姓名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "邮件地址";
            // 
            // user
            // 
            this.user.Location = new System.Drawing.Point(70, 27);
            this.user.Name = "user";
            this.user.Size = new System.Drawing.Size(189, 21);
            this.user.TabIndex = 5;
            // 
            // email
            // 
            this.email.Location = new System.Drawing.Point(70, 63);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(189, 21);
            this.email.TabIndex = 6;
            // 
            // state
            // 
            this.state.AutoSize = true;
            this.state.Location = new System.Drawing.Point(96, 104);
            this.state.Name = "state";
            this.state.Size = new System.Drawing.Size(72, 16);
            this.state.TabIndex = 7;
            this.state.Text = "是否有效";
            this.state.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "状态";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.emailDel);
            this.groupBox1.Location = new System.Drawing.Point(275, 219);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(103, 172);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "删除操作";
            // 
            // emailDel
            // 
            this.emailDel.Location = new System.Drawing.Point(14, 75);
            this.emailDel.Name = "emailDel";
            this.emailDel.Size = new System.Drawing.Size(75, 23);
            this.emailDel.TabIndex = 0;
            this.emailDel.Text = "删除勾选项";
            this.emailDel.UseVisualStyleBackColor = true;
            this.emailDel.Click += new System.EventHandler(this.emailDel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.emailAdd);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.state);
            this.groupBox2.Controls.Add(this.user);
            this.groupBox2.Controls.Add(this.email);
            this.groupBox2.Location = new System.Drawing.Point(3, 219);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(266, 172);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "新增及修改";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tb_pwd);
            this.groupBox3.Controls.Add(this.tb_user);
            this.groupBox3.Controls.Add(this.tb_smtp);
            this.groupBox3.Controls.Add(this.bt_smtp);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(384, 219);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(213, 172);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "SMTP设置";
            // 
            // tb_pwd
            // 
            this.tb_pwd.Location = new System.Drawing.Point(47, 96);
            this.tb_pwd.Name = "tb_pwd";
            this.tb_pwd.Size = new System.Drawing.Size(156, 21);
            this.tb_pwd.TabIndex = 14;
            // 
            // tb_user
            // 
            this.tb_user.Location = new System.Drawing.Point(48, 59);
            this.tb_user.Name = "tb_user";
            this.tb_user.Size = new System.Drawing.Size(155, 21);
            this.tb_user.TabIndex = 13;
            // 
            // tb_smtp
            // 
            this.tb_smtp.Location = new System.Drawing.Point(48, 27);
            this.tb_smtp.Name = "tb_smtp";
            this.tb_smtp.Size = new System.Drawing.Size(155, 21);
            this.tb_smtp.TabIndex = 12;
            // 
            // bt_smtp
            // 
            this.bt_smtp.Location = new System.Drawing.Point(65, 136);
            this.bt_smtp.Name = "bt_smtp";
            this.bt_smtp.Size = new System.Drawing.Size(75, 23);
            this.bt_smtp.TabIndex = 9;
            this.bt_smtp.Text = "设置";
            this.bt_smtp.UseVisualStyleBackColor = true;
            this.bt_smtp.Click += new System.EventHandler(this.bt_smtp_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "密码:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "用户名:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "smtp:";
            // 
            // EmailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 401);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.emalListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "EmailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "邮件人员设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EmailForm_FormClosing);
            this.Load += new System.EventHandler(this.EmailForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView emalListView;
        private System.Windows.Forms.ColumnHeader nameCol;
        private System.Windows.Forms.ColumnHeader addrCol;
        public System.Windows.Forms.ColumnHeader stateCol;
        private System.Windows.Forms.Button emailAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox user;
        private System.Windows.Forms.TextBox email;
        private System.Windows.Forms.CheckBox state;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button emailDel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tb_pwd;
        private System.Windows.Forms.TextBox tb_user;
        private System.Windows.Forms.TextBox tb_smtp;
        private System.Windows.Forms.Button bt_smtp;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}