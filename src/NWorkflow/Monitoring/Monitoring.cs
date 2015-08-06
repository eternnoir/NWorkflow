using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWorkflow.Monitoring
{
    static class Monitoring
    {

        /// <summary>
        /// For thread safe.
        /// </summary>
        private static object syncRoot = new object();

        private static Dictionary<string, IMonitor> loggerDictionary = new Dictionary<string, IMonitor>();
        public static IMonitor GetMonitor(string MonitorName)
        {
            lock (syncRoot)
            {
                if (!loggerDictionary.ContainsKey(MonitorName))
                {
                    loggerDictionary.Add(MonitorName, new Monitor(MonitorName));
                }
                return loggerDictionary[MonitorName];
            }
        }

    }
}
