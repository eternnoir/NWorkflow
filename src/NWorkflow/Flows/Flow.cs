namespace NWorkflow.Flows
{
    #region

    using System.Collections.Generic;

    using log4net;

    using NWorkflow.Monitoring;
    using NWorkflow.Recovery;

    #endregion

    /// <summary>
    ///     Base Flow class. This class implement some basic behavior flow needed.
    ///     If you want implement you own flow. You can inherit this class.
    /// </summary>
    public abstract class Flow : IFlow, IJob
    {
        protected Dictionary<string, IJob> jobNameDic;

        protected Dictionary<IJob, JobResult> jobResultDic;
        protected IRecover recover;

        /// <summary>
        /// Flow constructor.
        /// </summary>
        /// <param name="FlowName"> Flow Name. </param>
        /// <param name="recoveryMode"> RecoveryMode which used in this flow. </param>
        protected Flow(string FlowName, RecoveryMode recoveryMode = RecoveryMode.STACK)
        {
            this.Name = FlowName;
            this.WorkingMemory = new Dictionary<string, object>();
            this.Monitor = Monitoring.GetMonitor(this.Name);
            this.logger = LogManager.GetLogger(this.Name);
            this.RecoveryMode = recoveryMode;
            this.recover = RecoverFactory.GetRecovery(this.RecoveryMode, this);
        }

        public Dictionary<string, object> WorkingMemory { get; private set; }

        public RecoveryMode RecoveryMode { get; private set; }

        public ILog Logger
        {
            get
            {
                if (this.parentFlow != null)
                {
                    return this.parentFlow.Logger;
                }

                return this.logger;
            }

            set
            {
                this.logger = value;
            }
        }

        public IMonitor Monitor { get; set; }

        public abstract JobResult RunAllJob();

        public abstract JobResult RunJob(string JobName);

        public abstract JobResult RunFinalizeJob();

        public string Name { get; private set; }

        public abstract JobResult GetJobResult(string JobName);

        public abstract JobResult GetJobResult(IJob JobObj);

        private ILog logger;

        private IFlow parentFlow;


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
            get
            {
                return this.Name;
            }

            set
            {
                this.Name = value;
            }
        }

        /// <summary>
        /// Initialize. This method will be called before call Execute method.
        /// </summary>
        public abstract void Init();

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
            this.jobResultDic[Job] = result;
            return result;
        }
    }
}