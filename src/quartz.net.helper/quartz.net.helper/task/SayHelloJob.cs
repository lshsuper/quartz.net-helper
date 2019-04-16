using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quartz.net.helper.task
{
    /// <summary>
    /// 调度任务
    /// </summary>
    public class SayHelloJob : IJob
    {
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public Task Execute(IJobExecutionContext context)
        {

            Console.WriteLine(context.Scheduler.SchedulerName);
            return Task.FromResult(0);
        }
    }
}
