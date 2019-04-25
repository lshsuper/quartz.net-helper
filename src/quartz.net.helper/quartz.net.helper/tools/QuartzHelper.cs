using ConsoleApp33.tools;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quartz.net.helper.tools
{
    /// <summary>
    /// 调度工具类(Core)
    /// </summary>
    public class QuartzHelper
    {
        private static object _lockQuaetz = new object();
        private static QuartzContext _context;
        /// <summary>
        /// 单实例
        /// </summary>
        public static QuartzContext Instance 
        {
            get
            {
                if (_context != null)
                    return _context;
                lock (_lockQuaetz)
                {
                    if (_context != null)
                        return _context;
                    _context = new QuartzContext();
                   
                }
                return _context;
            }
        }
        
    }
}
