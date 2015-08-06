using NLogging;
using NWorkflow.Monitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWorkflow
{
    public abstract class Flow : IFlow
    {
        private string flowName;
        private ILogger logger;
        private IMonitor monitor;
        private Dictionary<string, object> workingMemory;

        public Flow(string FlowName)
        {
            flowName = FlowName;
            workingMemory = new Dictionary<string, object>();
            monitor = Monitoring.Monitoring.GetMonitor(this.Name);
            logger = Logging.GetLogger(this.Name);
            this.RecoveryMode = RecoveryMode.STACK;
        }

        public Dictionary<string, object> WorkingMemory
        {
            get
            {
                return workingMemory;
            }
        }
        public RecoveryMode RecoveryMode
        {
            get;
            set;
        }

        public ILogger Logger
        {
            get
            {
                return logger;
            }
            set
            {
                this.logger = value;
            }
        }

        public Monitoring.IMonitor Monitor
        {
            get
            {
                return this.monitor;
            }
            set
            {
                this.monitor = value;
            }
        }

        abstract public void RunAllJob();

        abstract public JobResult RunJob(string JobName);

        public string Name
        {
            get { return flowName; }
        }


        abstract public JobResult GetJobResult(string JobName);

        abstract public JobResult GetJobResult(IJob JobObj);
    }

}
