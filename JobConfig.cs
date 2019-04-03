using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Quartz.Net
{
    /// <summary>
    /// 任务配置对象
    /// </summary>
    public class JobConfig
    {
        public JobConfig()
        {
            JobDataMap = new Dictionary<object, object>();
        }

        public string JobName { get; set; }//任务名称
        public string JobGroup { get; set; }//任务分组名
        public string TriggerName { get; set; }//触发器名
        public string TriggerGroup { get; set; }//触发器分组名
        public string CronExpression { get; set; }//cron表达式
        public Type JobType { get; set; }//任务类型
        public Dictionary<object,object> JobDataMap { get; set; } //任务参数map
        public string SchedulerName { get; set; }  //调度器名称（自定义指定）
    }
}
