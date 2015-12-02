namespace NWorkflow
{
    #region

    using log4net;

    using NWorkflow.Monitoring;

    #endregion

    public interface IJob
    {
        IFlow Flow { get; set; }

        string JobName { get; set; }

        ILog Logger { get; }

        IMonitor Monitor { get; }

        void Init();

        JobResult Execute();

        void DoRecover();
    }
}