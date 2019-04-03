using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Quartz.Net
{
    /// <summary>
    /// 定时调度表达式构建类
    /// </summary>
   public class CronExpressionUtil
    {
       
        /// <summary>
        /// 每隔多少秒执行一次
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static  string EverySeconds(int seconds)
        {
            return string.Format("0/{0} * * * * ?",seconds);
        }
        /// <summary>
        /// 每隔多少分钟执行一次
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static string EveryMinutes(int minutes)
        {
            return string.Format("0 0/{0} * * * ?",minutes);
        }
        /// <summary>
        /// 每隔多少小时执行一次
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static string EveryHours(int hours)
        {
            return string.Format("0 0 0/{0} * * ?",hours);
        }
        /// <summary>
        /// 构建只执行一次的调度表达式
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
    }
}
