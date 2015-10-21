using NWorkflow.Monitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Core;

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

        ILog Logger
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
