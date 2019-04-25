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

            
            QuartzHelper.Instance. Start("");

            QuartzHelper.Instance.AddJob(new JobConfig()
            {
                JobGroup = "第一个",
                JobName = "第一个",
                TriggerGroup = "第一个01",
                TriggerName = "第一个",


                JobType = typeof(SayHelloJob),
                CronExpression = CronExpressionUtil.EverySeconds(5),
                
            });
            
        }
        public void Stop()
        {
            QuartzHelper.Instance.ClearScheduler("lsh");
        }
    }
}
