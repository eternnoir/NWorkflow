﻿using NLogging;
using NWorkflow.Monitoring;
using NWorkflow.Recovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWorkflow
{
    public abstract class Flow : IFlow, IJob
    {
        private string flowName;
        private ILogger logger;
        private IMonitor monitor;
        private Dictionary<string, object> workingMemory;
        protected IRecover recover;
        private RecoveryMode recoveryMode;
        private IFlow parentFlow;
        protected Dictionary<IJob, JobResult> jobResultDic;
        protected Dictionary<string, IJob> jobNameDic;

        public Flow(string FlowName, RecoveryMode recoveryMode = RecoveryMode.STACK)
        {
            flowName = FlowName;
            workingMemory = new Dictionary<string, object>();
            monitor = Monitoring.Monitoring.GetMonitor(this.Name);
            logger = Logging.GetLogger(this.Name);
            this.recoveryMode = recoveryMode;
            recover = RecoverFactory.GetRecovery(this.RecoveryMode, this);
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
            get
            {
                return this.recoveryMode;
            }
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
        abstract public void Init();

        public JobResult Execute()
        {
            return this.RunAllJob();
        }

        public void DoRecover()
        {
            this.recover.DoRecover();
        }

        protected JobResult ExecuteJob(IJob Job)
        {
            this.recover.AppendRunedJob(Job);
            Job.Init();
            var result = Job.Execute();
            jobResultDic[Job] = result;
            return result;
        }

        abstract public JobResult RunAllJob();

        abstract public JobResult RunJob(string JobName);

        public string Name
        {
            get { return flowName; }
        }

        abstract public JobResult GetJobResult(string JobName);

        abstract public JobResult GetJobResult(IJob JobObj);

        IFlow IJob.Flow
        {
            get
            {
                return this.parentFlow;
            }
            set
            {
                this.parentFlow = value;
            }
        }

        public string JobName
        {
            get { return this.flowName; }
        }
    }

}
