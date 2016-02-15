namespace NWorkflow.Monitoring
{
    #region

    using System.Collections.Generic;

    #endregion

    public static class Monitoring
    {
        /// <summary>
        ///     For thread safe.
        /// </summary>
        private static readonly object syncRoot = new object();

        private static readonly Dictionary<string, IMonitor> loggerDictionary = new Dictionary<string, IMonitor>();

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