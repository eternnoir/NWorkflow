using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWorkflow
{
    abstract public class Job :IJob
    {
        private IFlow flow;
        private string jobName;

        public Job(string JobName)
        {
            jobName = JobName;
        }
        public IFlow Flow
        {
            get
            {
                return flow;
            }
            set
            {
                flow = value;
            }
        }

        abstract public void Init();

        abstract public JobResult Execute();

        protected Dictionary<string, object> WorkingMemeory
        {
            get
            {
                return flow.WorkingMemory;
            }
        }

        public string JobName
        {
            get { return jobName; }
        }

        public NLogging.ILogger Logger
        {
            get { return NLogging.Logging.GetLogger(this.JobName); }
        }
    }
}
