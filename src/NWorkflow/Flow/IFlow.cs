using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using NWorkflow.Monitoring;
using NWorkflow.Recovery;
using log4net;


namespace NWorkflow
{
    public interface IFlow
    {
        RecoveryMode RecoveryMode
        {
            get;
        }

        ILog Logger
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

        JobResult RunAllJob();
        JobResult RunJob(string JobName);
    }
}
