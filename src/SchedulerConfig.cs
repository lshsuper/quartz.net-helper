using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Quartz.Net
{
    /// <summary>
    ///自定义调度器的配置
    /// </summary>
    public class SchedulerConfig
    {
        public string SchedulerName { get; set; }  //调度器名称
        #region Compute Attr
        public string SchedulerId
        {
            get
            {
                return SchedulerName + "Id";
            }

        }//调度器唯一id
        private int threadCount;
        public int ThreadCount
        {
            get
            {
                if (threadCount <= 0)
                {
                    threadCount = 20;
                }
                return threadCount;
            }
            set
            {
                threadCount = value;
            }
        }//线程数
        #endregion


    }
}
