using OnlineIpDA.entity;
using OnlineIpDA.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineIpDA.ui
{
    public partial class QuartzForm : Form
    {
        private int type = 1;//每天
        private DayOfWeek weekDay = DayOfWeek.Monday;
        private int hour;
        private int minute;
        private bool state;

        public delegate void DelegaeQuartz(string value);
        public event DelegaeQuartz Myevent;
        private void QuartzSetting(string value)
        {
            DelegaeQuartz handler = Myevent;
            if (handler != null)
                handler(value);
        }


        public QuartzForm()
        {
            InitializeComponent();
            
        }

        private void QuartzForm_Load(object sender, EventArgs e)
        {
            loadFromJson();

            parseCbCyc();
            hour = this.dtPicker.Value.Hour;
            minute = this.dtPicker.Value.Minute;

            if (rbClose.Checked)
            {
                this.cb_cyc.Enabled = false;
                this.dtPicker.Enabled = false;
                this.btSetting.Enabled = false;
            }

            this.rbOpen.CheckedChanged += new EventHandler(this.radioBtn_CheckedChange);
            this.rbClose.CheckedChanged += new EventHandler(this.radioBtn_CheckedChange);
        }

        private void radioBtn_CheckedChange(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
            {
                return;
            }

            switch (((RadioButton)sender).Text.ToString())
            {
                case "开启":
                    state = true;
                    rbOpen_CheckedChanged();
                    break;
                case "关闭":
                    state = false;
                    rbClose_CheckedChanged();
                    break;
            }
        }

        private void rbOpen_CheckedChanged()
        {
            this.cb_cyc.Enabled = true;
            this.dtPicker.Enabled = true;
            this.btSetting.Enabled = true;
            parseCbCyc();
        }

        #region 获取设置周期 parseCbCyc
        /// <summary>
        /// 获取设置周期
        /// </summary>
        private void parseCbCyc()
        {
            type = 1;
            switch (this.cb_cyc.Text)
            {
                case "每周一":
                    weekDay = DayOfWeek.Monday;
                    break;
                case "每周二":
                    weekDay = DayOfWeek.Tuesday;
                    break;
                case "每周三":
                    weekDay = DayOfWeek.Wednesday;
                    break;
                case "每周四":
                    weekDay = DayOfWeek.Thursday;
                    break;
                case "每周五":
                    weekDay = DayOfWeek.Friday;
                    break;
                case "每周六":
                    weekDay = DayOfWeek.Saturday;
                    break;
                case "每周日":
                    weekDay = DayOfWeek.Sunday;
                    break;
                case "每天":
                    type = 0;
                    break;
                default:
                    break;
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

        private void rbClose_CheckedChanged()
        {
            this.cb_cyc.Enabled = false;
            this.dtPicker.Enabled = false;
        }

        private void cb_cyc_SelectedIndexChanged(object sender, EventArgs e)
        {
            parseCbCyc();
        }

        private void btSetting_Click(object sender, EventArgs e)
        {
            if (rbOpen.Checked)
            {
                state = true;
                hour = this.dtPicker.Value.Hour;
                minute = this.dtPicker.Value.Minute;
                if (QuartzHelper.init(type, weekDay.ToString(), hour, minute))
                {
                    saveToJson();
                    if (type == 0)//每天
                    {
                        this.lb_quartzNotice.Text = string.Format("定时：{0}{1}点{2}分", getCbCycText(7), hour, minute);
                    }
                    else
                    {
                        this.lb_quartzNotice.Text = string.Format("定时：{0}{1}点{2}分", getCbCycText(Convert.ToInt16(weekDay)), hour, minute);
                    }
                    this.lb_quartzNotice.Visible = true;
                    QuartzSetting(this.lb_quartzNotice.Text);
                    MessageBox.Show("定时设置成功", "提示");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("定时设置失败", "提示");
                }
            }
            else
            {
                state = false;
                saveToJson();
                this.lb_quartzNotice.Visible = false;
                QuartzSetting("");
                QuartzHelper.release();
                MessageBox.Show("取消定时设置", "提示");
                this.Close();
            }
        }

        #region 保存定时数据到json文件 saveToJson
        /// <summary>
        /// 保存定时数据中数据到json文件
        /// </summary>
        /// <returns></returns>
        private bool saveToJson()
        {
            QuartzTimer qt = new QuartzTimer();
            qt.state = state;
            qt.type = type;
            qt.weekday = Convert.ToInt32(weekDay);
            qt.hour = hour;
            qt.minute = minute;

            List<QuartzTimer> list = new List<QuartzTimer>();
            list.Add(qt);

            string json = JsonHelper.Serialize(list);

            return FileHelper.writeFile(FileHelper.QUARTZ_FILE, json);
        }
        #endregion

        #region 读取数据 loadFromJson
        /// <summary>
        /// 读取数据
        /// </summary>
        private void loadFromJson()
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

                if(null == quzrtzs || quzrtzs.Count == 0)
                {
                    throw new Exception();
                }
                QuartzTimer qt = quzrtzs[0];
                if (qt.state)
                {
                    this.rbOpen.Checked = true;
                    this.rbClose.Checked = false;
                }
                else
                {
                    this.rbOpen.Checked = false;
                    this.rbClose.Checked = true;
                }
                if (qt.type == 0)//每天
                {
                    this.cb_cyc.SelectedIndex = 7;
                }
                else
                {
                    this.cb_cyc.SelectedIndex = qt.weekday;
                }

                this.dtPicker.Text = string.Format("{0}:{1}",qt.hour,qt.minute);

                if (qt.state)
                {
                    this.lb_quartzNotice.Visible = true; 
                    if(qt.type == 0)//每天
                    {
                        this.lb_quartzNotice.Text = string.Format("定时：{0}{1}点{2}分", getCbCycText(7), qt.hour, qt.minute);
                    }else{
                        this.lb_quartzNotice.Text = string.Format("定时：{0}{1}点{2}分", getCbCycText(qt.weekday), qt.hour, qt.minute);
                    }
                }
                else
                {
                    this.lb_quartzNotice.Visible = false;
                }
                
            }
            catch (Exception)
            {
                this.cb_cyc.SelectedIndex = 0;
            }
        }
        #endregion

    }
}
