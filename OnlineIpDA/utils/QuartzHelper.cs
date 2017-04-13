using OnlineIpDA.entity;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineIpDA.utils
{
    /// <summary>
    /// 文件名:QuartzHelper.cs
    ///	功能描述:Quartz定时工具类
    ///
    /// 作者:吕凤凯
    /// 创建时间:2016/2/26 15:14:24
    /// 
    /// </summary>
    class QuartzHelper
    {
        private static IScheduler scheduler = null;

        public class EmailJob : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                //执行任务
                if (!MainForm.bIpAd)
                {
                    LogHelper.writeLog(LogHelper.QUARTZ_EXECUTE, "执行定时任务，生成分析结果并邮件发送.");
                    //生成，但不弹提示框
                    MainForm.getInstance().generateEamil(false);
                }
                else
                {
                    LogHelper.writeLog(LogHelper.QUARTZ_EXECUTE, "上一个定时任务还未完成，请分析.");

                }
            }

        }

        /// <summary>
        /// 初始化定时器
        /// </summary>
        /// <param name="type">0：每天,1：每周</param>
        /// <param name="weekday">星期几</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        public static bool init(int type, string weekday, int hour, int minute)
        {
            try
            {
                //释放定时器
                release();

                //工厂
                ISchedulerFactory factory = new StdSchedulerFactory();
                //启动
                if (scheduler == null)
                    scheduler = factory.GetScheduler();

                //启动
                scheduler.Start();

                //描述工作
                IJobDetail job = JobBuilder.Create<EmailJob>().WithIdentity("Ipjob", "Ipjobs").Build();
                //触发器
                ITrigger trigger = null;
                if (type == 0)
                {//每天定时执行
                    trigger = TriggerBuilder.Create()
                    .WithIdentity("Iptrigger", "Ipjobs")
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour, minute))//每天8:40执行
                    .Build();
                }
                else if (type == 1)
                {//每周星期几定时执行
                    DayOfWeek wday = (DayOfWeek)System.Enum.Parse(typeof(DayOfWeek), weekday);
                    trigger = TriggerBuilder.Create()
                    .WithIdentity("Iptrigger", "Ipjobs")
                    .WithSchedule(CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(wday, hour, minute))//每周相应时间执行
                    .Build();
                }

                //执行
                scheduler.ScheduleJob(job, trigger);
            }
            catch (Exception)
            {

                return false;
           }

            return true;
        }

        /// <summary>
        /// 是否定时器
        /// </summary>
        public static void release()
        {
            if (scheduler != null && !scheduler.IsShutdown)
            {
                scheduler.Shutdown(true);
                scheduler = null;
            }
        }

        #region 加载定时器 initQuartz
        /// <summary>
        /// 加载定时器
        /// </summary>
        public static void initQuartz()
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

                release();

                init(qt.type, parseDayOfWeek(qt.weekday), qt.hour, qt.minute);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 获取设置周期
        /// </summary>
        private static string parseDayOfWeek(int d)
        {
            DayOfWeek weekDay = DayOfWeek.Monday;
            switch (d)
            {
                case 0:
                    weekDay = DayOfWeek.Sunday;
                    break;
                case 1:
                    weekDay = DayOfWeek.Monday;
                    break;
                case 2:
                    weekDay = DayOfWeek.Tuesday;
                    break;
                case 3:
                    weekDay = DayOfWeek.Wednesday;
                    break;
                case 4:
                    weekDay = DayOfWeek.Thursday;
                    break;
                case 5:
                    weekDay = DayOfWeek.Friday;
                    break;
                case 6:
                    weekDay = DayOfWeek.Saturday;
                    break;
                default:
                    break;
            }

            return weekDay.ToString();
        } 
        #endregion
    }
}
