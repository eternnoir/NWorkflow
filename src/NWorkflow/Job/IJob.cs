using NLogging;
using NWorkflow.Monitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWorkflow
{
    public interface IJob
    {
        IFlow Flow
        {
            get;
            set;
        }

        string JobName
        {
            get;
            set;
        }

        ILogger Logger
        {
            get;
        }

        IMonitor Monitor
        {
            get;
        }
        void Init();
        JobResult Execute();
        void DoRecover();
    }
}
