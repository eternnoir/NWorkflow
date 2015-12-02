namespace NWorkflow
{
    #region

    using System.Collections.Generic;

    using log4net;

    using NWorkflow.Monitoring;

    #endregion

    public abstract class Job : IJob
    {
        public Job(string JobName)
        {
            this.JobName = JobName;
        }

        protected Dictionary<string, object> WorkingMemeory
        {
            get
            {
                return this.Flow.WorkingMemory;
            }
        }

        public IFlow Flow { get; set; }

        public abstract void Init();

        public abstract JobResult Execute();

        public string JobName { get; set; }

        public ILog Logger
        {
            get
            {
                return this.Flow.Logger;
            }
        }

        public abstract void DoRecover();

        public IMonitor Monitor
        {
            get
            {
                return this.Flow.Monitor;
            }
        }
    }
}