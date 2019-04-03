using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Quartz.Net
{
    /// <summary>
    /// 调度工具类(Core)
    /// </summary>
    public class QuartzUtil
    {

        private static DirectSchedulerFactory _factory;//调度工厂（测试中。。。）
        private static IScheduler Scheduler { get; set; }
        static QuartzUtil()
        {
            _factory =DirectSchedulerFactory.Instance;
            Scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            Scheduler.Start();
        }
        #region Extend(测试中...)
        /// <summary>
        /// 创建一个scheduler（自定义调度器的相关参数）
        /// </summary>
        /// <param name="schedulerConfig"></param>
        public static bool CreateScheduler(SchedulerConfig schedulerConfig)
        {
            try
            {
                var pool = new DefaultThreadPool() {
                    ThreadCount= schedulerConfig.ThreadCount
                };
                _factory.CreateScheduler(schedulerConfig.SchedulerName, schedulerConfig.SchedulerId, pool, new RAMJobStore());
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }
        /// <summary>
        /// 获取scheduler（策略：如果没有指定名称的调度器，那就使用默认的调度器）
        /// </summary>
        /// <param name="schedulerName"></param>
        /// <returns></returns>
        private static IScheduler GetScheduler(string schedulerName)
        {
            //获取指定的scheduler
            IScheduler currentScheduler = _factory.GetScheduler(schedulerName==null?"":schedulerName).Result;
            if (currentScheduler == null)
            {
                currentScheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            }
            currentScheduler.Start();
            return currentScheduler;
        }
        #endregion
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="jobConfig"></param>
        /// <returns></returns>
        public static bool AddJob(JobConfig jobConfig)
        {
            try
            {
                bool isHasJob = Scheduler.CheckExists(JobKey.Create(jobConfig.JobName, jobConfig.JobGroup)).Result;
                bool isHasTrigger = Scheduler.CheckExists(new TriggerKey(jobConfig.TriggerName, jobConfig.TriggerGroup)).Result;
                if (isHasTrigger&&isHasJob) return false;
                ITrigger trigger = TriggerBuilder.Create()
                              .WithIdentity(jobConfig.TriggerName, jobConfig.TriggerGroup)
                              .WithCronSchedule(jobConfig.CronExpression).Build();
                IJobDetail jobDetail = JobBuilder.Create(jobConfig.JobType).SetJobData(new JobDataMap(jobConfig.JobDataMap)).WithIdentity(jobConfig.JobName, jobConfig.JobGroup).Build();
                Scheduler.ScheduleJob(jobDetail, trigger);
                return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }


        }
        /// <summary>
        /// 是否有job
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="jobGroup"></param>
        /// <param name="schedulerName"></param>
        /// <returns></returns>
        public static bool IsHasJob(string jobName, string jobGroup, string schedulerName = "")
        {
            try
            {
                return Scheduler.CheckExists(JobKey.Create(jobName, jobGroup)).Result;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        /// <summary>
        /// 是否有触发器
        /// </summary>
        /// <param name="triggerName"></param>
        /// <param name="triggerGroup"></param>
        /// <param name="schedulerName"></param>
        /// <returns></returns>
        public static bool IsHasTrigger(string triggerName, string triggerGroup, string schedulerName = "")
        {
            try
            {
                return Scheduler.CheckExists(new TriggerKey(triggerName, triggerGroup)).Result;
            }
            catch (Exception ex)
            {
                return false;

            }
        }
        /// <summary>
        /// 清空调度器(策略:默认清理的是默认的调度器,指定调度器名称将会清理指定的调度器)
        /// </summary>
        /// <returns></returns>
        public static bool ClearScheduler(string schedulerName="")
        {
            try
            {
                //获取指定的scheduler
                IScheduler currentScheduler = _factory.GetScheduler(schedulerName == null ? "" : schedulerName).Result;
                if (currentScheduler == null)
                {
                    currentScheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
                }
                currentScheduler.Clear();
                return true;
            }
            catch (Exception ex)
            {
                return false;
               
            }
        }
        /// <summary>
        /// 构建cons表达式
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string BuildConsExpression(DateTime dateTime)
        {
            if (dateTime.CompareTo(DateTime.Now) <= 0)
            {
                //过期的任务会延迟二十秒进行调度
                dateTime = DateTime.Now.AddSeconds(20);
            }
            return string.Format("{0} {1} {2} {3} {4} ? {5}", dateTime.Second, dateTime.Minute, dateTime.Hour, dateTime.Day, dateTime.Month, dateTime.Year);
        }
        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="jobConfig"></param>
        /// <returns></returns>
        public static bool RemoveJob(JobConfig jobConfig)
        {

            try
            {
                TriggerKey triggerKey = new TriggerKey(jobConfig.TriggerName, jobConfig.TriggerGroup);
                Scheduler.PauseTrigger(triggerKey);
                Scheduler.ResumeTrigger(triggerKey);
                Scheduler.DeleteJob(JobKey.Create(jobConfig.JobName, jobConfig.JobGroup));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
        /// <summary>
        /// 构建触发器
        /// </summary>
        /// <param name="triggerName"></param>
        /// <param name="groupName"></param>
        /// <param name="cronExpression"></param>
        /// <returns></returns>
        public static ITrigger BuildTrigger(string triggerName, string groupName, string cronExpression)
        {
            ITrigger trigger = TriggerBuilder.Create()
                                .WithIdentity(triggerName, groupName)
                                .WithCronSchedule(cronExpression).Build();
            return trigger;
        }
        /// <summary>
        /// 构建任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="groupName"></param>
        /// <param name="jobType"></param>
        /// <returns></returns>
        public static IJobDetail BuildJob(string jobName, string groupName, Type jobType)
        {
            IJobDetail jobDetail = JobBuilder.Create(jobType).WithIdentity(jobName, groupName).Build();
            return jobDetail;
        }
        /// <summary>
        /// 绑定任务与触发器
        /// </summary>
        /// <param name="jobDetail"></param>
        /// <param name="trigger"></param>
        public static void BindJobAndTrigger(IJobDetail jobDetail, ITrigger trigger)
        {

            Scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
