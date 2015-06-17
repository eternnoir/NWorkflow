using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLogging;
using NWorkflow.Monitoring;


namespace NWorkflow
{
    public interface IFlow
    {
        RecoveryMode RecoveryMode
        {
            set;
            get;
        }

        ILogger Logger
        {
            set;
            get;
        }

        IMonitor Monitor
        {
            set;
            get;
        }

        void RunAllJob();
        JobResult RunJon(string JobName);
    }
}
