using OnlineIpDA.entity;
using OnlineIpDA.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace OnlineIpDA.ui
{
    public partial class EmailForm : Form
    {
        public EmailForm()
        {
            InitializeComponent();
        }

        private void emailAdd_Click(object sender, EventArgs e)
        {
            this.emalListView.BeginUpdate();   //数据更新，UI暂时挂起
            Regex regex = new Regex(@"^[a-z0-9A-Z\._%+-]+@[a-z0-9A-Z\._%+-]+\.([a-zA-Z]){2,4}$");
            
            foreach (ListViewItem item in emalListView.Items)
            {
                if (item.SubItems[1].Text.Equals(this.email.Text))
                {
                    item.SubItems[0].Text = this.user.Text;
                    if (this.state.Checked)
                    {
                        item.SubItems[2].Text = "有效";
                    }
                    else
                    {
                        item.SubItems[2].Text = "无效";
                    }
                    
                    this.emalListView.EndUpdate();  //结束数据处理，UI界面一次性绘制
                    MessageBox.Show(this.email.Text+" 的信息修改成功","提示");
                    return;
                }
            }
            if (!regex.Match(this.email.Text).Success)
            {
                this.emalListView.EndUpdate();  //结束数据处理，UI界面一次性绘制
                MessageBox.Show(this.email.Text + " 不符合邮件格式", "提示");
                return;
            }

            ListViewItem lvi = new ListViewItem();
            lvi.Text = this.user.Text;
            lvi.SubItems.Add(this.email.Text);

            if (this.state.Checked)
            {
                lvi.SubItems.Add("有效");
            }
            else
            {
                lvi.SubItems.Add("无效");
            }
            this.emalListView.Items.Add(lvi);
            

            this.emalListView.EndUpdate();  //结束数据处理，UI界面一次性绘制

        }

        private void emalListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (emalListView.SelectedItems.Count > 0)
            {
                ListViewItem lvi = emalListView.SelectedItems[0];
                this.user.Text = lvi.SubItems[0].Text;
                this.email.Text = lvi.SubItems[1].Text;

                if (lvi.SubItems[2].Text.Equals("有效"))
                {
                    this.state.Checked = true;
                }
                else
                {
                    this.state.Checked = false;
                }
            }
        }

        private void EmailForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //保存文件数据
            saveToJson(this.emalListView);

            //重新加载数据
            Init.init();
        }

        private void EmailForm_Load(object sender, EventArgs e)
        {
            //加载数据
            loadFromJson();

            loadSmtpFromJson();
        }

        #region 保存ListView中数据到json文件 saveToJson
        /// <summary>
        /// 保存ListView中数据到json文件
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        private bool saveToJson(ListView lv)
        {
            List<Email> emails = new List<Email>();
            foreach (ListViewItem item in emalListView.Items)
            {
                Email e = new Email();
                e.name = item.SubItems[0].Text;
                e.email = item.SubItems[1].Text;
                e.state = item.SubItems[2].Text;
                emails.Add(e);
            }

            string json = JsonHelper.Serialize(emails);

            return FileHelper.writeFile(FileHelper.EMAIL_FILE, json);
        } 
        #endregion

        #region 读取数据，并填充ListView控件 loadFromJson
        /// <summary>
        /// 读取数据，并填充ListView控件
        /// </summary>
        private void loadFromJson()
        {
            List<Email> emails = Init.getEmails();
            if (null == emails)
            {
                return;
            }

            this.emalListView.BeginUpdate();   //数据更新，UI暂时挂起

            foreach (Email e in emails)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = e.name;
                lvi.SubItems.Add(e.email);
                lvi.SubItems.Add(e.state);
                this.emalListView.Items.Add(lvi);
            }

            this.emalListView.EndUpdate();
        } 
        #endregion

        private void emailDel_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Count:" + emalListView.CheckedItems.Count);
            if (emalListView.CheckedItems.Count == 0)
            {
                MessageBox.Show("未选中要删除行","提示");
                return;
            }
            if (MessageBox.Show("确定要删除选中行", "提示",MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                foreach (ListViewItem item in emalListView.Items)
                {
                    //删除选中项
                    if (item.Checked)
                    {
                        emalListView.Items.Remove(item);
                    }
                }
            }
        }

        private void bt_smtp_Click(object sender, EventArgs e)
        {
            if (saveSmtpToJson())
            {
                MessageBox.Show("SMTP设置成功", "提示");
            }
            else
            {
                MessageBox.Show("SMTP设置失败", "提示");
            }
        }

        #region 保存定时数据到json文件 saveSmtpToJson
        /// <summary>
        /// 保存定时数据中数据到json文件
        /// </summary>
        /// <returns></returns>
        private bool saveSmtpToJson()
        {
            Smtp smtp = new Smtp();
            smtp.smtp = this.tb_smtp.Text;
            smtp.user = this.tb_user.Text;
            smtp.pwd = this.tb_pwd.Text;

            List<Smtp> list = new List<Smtp>();
            list.Add(smtp);

            string json = JsonHelper.Serialize(list);

            return FileHelper.writeFile(FileHelper.SMTP_FILE, json);
        }
        #endregion

        #region 读取数据 loadSmtpFromJson
        /// <summary>
        /// 读取数据
        /// </summary>
        private void loadSmtpFromJson()
        {
            try
            {
                List<Smtp> smtps = Init.getSmtp();

                if (null == smtps || smtps.Count == 0)
                {
                    throw new Exception();
                }
                Smtp s = smtps[0];
                this.tb_smtp.Text = s.smtp;
                this.tb_user.Text = s.user;
                this.tb_pwd.Text = s.pwd;
            }
            catch (Exception)
            {
                
            }
        }
        #endregion

    }
}
