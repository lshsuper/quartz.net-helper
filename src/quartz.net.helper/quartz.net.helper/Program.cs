
using ConsoleApp33.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace quartz.net.helper.task
{
    class Program
    {
        static void Main(string[] args)
        {

            HostFactory.Run(x =>
            {
                x.Service<SayService>(s =>
                {
                    s.ConstructUsing(settings => new SayService());
                    s.WhenStarted(tr => tr.Start());
                    s.WhenStopped(tr => tr.Stop());
                });

                x.RunAsLocalSystem();

                x.SetDescription("定时报告时间01");
                x.SetDisplayName("时间报告器01");
                x.SetServiceName("TimeReporter01");
            });

            Console.WriteLine("ok");
            Console.ReadKey();
        }
    }
}
