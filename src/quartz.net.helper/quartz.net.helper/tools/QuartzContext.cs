using quartz.net.helper.tools;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp33.tools
{
    /// <summary>
    /// 可依赖注入
    /// </summary>
   public class QuartzContext
    {
        private  DirectSchedulerFactory _factory;//调度工厂（测试中。。。）
      

        public QuartzContext()
        {
            _factory = DirectSchedulerFactory.Instance;
        }

        /// <summary>
        /// 创建一个scheduler（自定义调度器的相关参数）
        /// </summary>
        /// <param name="schedulerConfig"></param>
        public  bool CreateScheduler(SchedulerConfig schedulerConfig)
        {
            try
            {
                var pool = new DefaultThreadPool()
                {
                    ThreadCount = schedulerConfig.ThreadCount
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
        private  IScheduler GetScheduler(string schedulerName)
        {
            //获取指定的scheduler
            IScheduler currentScheduler = null;
            if (!string.IsNullOrEmpty(schedulerName))
            {
                currentScheduler = _factory.GetScheduler(schedulerName).Result;
            }
            else
            {
                currentScheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            }

            return currentScheduler;
        }
        /// <summary>
        /// 开始调度
        /// </summary>
        /// <param name="schedulerName"></param>
        /// <returns></returns>
        public  bool Start(string schedulerName)
        {
            var currentScheduler = GetScheduler(schedulerName);
            if (!currentScheduler.IsStarted || currentScheduler.IsShutdown)
            {
                currentScheduler.Start();
            }
            return true;
        }
        /// <summary>
        /// 关闭调度
        /// </summary>
        /// <param name="schedulerName"></param>
        /// <returns></returns>
        public  bool Stop(string schedulerName)
        {
            var currentScheduler = GetScheduler(schedulerName);
            if (!currentScheduler.IsStarted || currentScheduler.IsShutdown)
            {
                currentScheduler.Shutdown();
            }
            return true;
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="jobConfig"></param>
        /// <returns></returns>
        public  bool AddJob(JobConfig jobConfig)
        {
            try
            {
                var currentScheduler = GetScheduler(jobConfig.SchedulerName);
                bool isHasJob = currentScheduler.CheckExists(JobKey.Create(jobConfig.JobName, jobConfig.JobGroup)).Result;
                bool isHasTrigger = currentScheduler.CheckExists(new TriggerKey(jobConfig.TriggerName, jobConfig.TriggerGroup)).Result;
                if (isHasTrigger && isHasJob) return false;
                ITrigger trigger = TriggerBuilder.Create()
                              .WithIdentity(jobConfig.TriggerName, jobConfig.TriggerGroup)
                              .WithCronSchedule(jobConfig.CronExpression).Build();
                IJobDetail jobDetail = JobBuilder.Create(jobConfig.JobType).SetJobData(new JobDataMap(jobConfig.JobDataMap)).WithIdentity(jobConfig.JobName, jobConfig.JobGroup).Build();
                currentScheduler.ScheduleJob(jobDetail, trigger);
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
        public  bool IsHasJob(string jobName, string jobGroup, string schedulerName = "")
        {
            try
            {
                return GetScheduler(schedulerName).CheckExists(JobKey.Create(jobName, jobGroup)).Result;
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
        public  bool IsHasTrigger(string triggerName, string triggerGroup, string schedulerName = "")
        {
            try
            {
                return GetScheduler(schedulerName).CheckExists(new TriggerKey(triggerName, triggerGroup)).Result;
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
        public  bool ClearScheduler(string schedulerName = "")
        {
            try
            {
                IScheduler currentScheduler = null;
                //获取指定的scheduler
                if (!string.IsNullOrEmpty(schedulerName))
                {
                    currentScheduler = _factory.GetScheduler(schedulerName).Result;
                }
                else
                {
                    currentScheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
                }
                if (currentScheduler != null)
                {
                    currentScheduler.Clear();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;

            }
        }

        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="jobConfig"></param>
        /// <returns></returns>
        public  bool RemoveJob(JobConfig jobConfig)
        {

            try
            {
                TriggerKey triggerKey = new TriggerKey(jobConfig.TriggerName, jobConfig.TriggerGroup);
                GetScheduler(jobConfig.SchedulerName).PauseTrigger(triggerKey);
                GetScheduler(jobConfig.SchedulerName).ResumeTrigger(triggerKey);
                GetScheduler(jobConfig.SchedulerName).DeleteJob(JobKey.Create(jobConfig.JobName, jobConfig.JobGroup));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
    }
}
