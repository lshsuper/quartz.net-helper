using quartz.net.helper.task;
using quartz.net.helper.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quartz.net.helper
{
  public  class SayService
    {
        public void Start()

        {

            QuartzUtil.CreateScheduler(new SchedulerConfig() {
                SchedulerName="lsh",
            });
            QuartzUtil.Start("lsh");
            QuartzUtil.AddJob(new JobConfig()
            {
                JobGroup = "jp",
                JobName = "j",
                TriggerGroup = "tg",
                TriggerName = "t",
                SchedulerName="lsh",
                JobType = typeof(SayHelloJob),
                CronExpression = CronExpressionUtil.EverySeconds(5),
                
            });

        }
        public void Stop()
        {
            QuartzUtil.ClearScheduler("lsh");
        }
    }
}
