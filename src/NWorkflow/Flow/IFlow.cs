using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLogging;
using NWorkflow.Monitoring;
using NWorkflow.Recovery;


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

        string Name
        {
            get;
        }

        Dictionary<string,object> WorkingMemory
        {
            get;
        }

        JobResult GetJobResult(string JobName);
        JobResult GetJobResult(IJob JobObj);

        void RunAllJob();
        JobResult RunJob(string JobName);
    }
}
