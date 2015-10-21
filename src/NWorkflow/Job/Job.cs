using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using log4net;

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
            set { this.jobName = value; }
        }

        public ILog Logger
        {
            get { return this.flow.Logger; }
        }


        abstract public void DoRecover();


        public Monitoring.IMonitor Monitor
        {
            get { return this.flow.Monitor; }
        }
    }
}
