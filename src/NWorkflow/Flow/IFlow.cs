namespace NWorkflow
{
    #region

    using System.Collections.Generic;

    using log4net;

    using NWorkflow.Monitoring;
    using NWorkflow.Recovery;

    #endregion

    /// <summary>
    /// Flow's Interface.
    /// </summary>
    public interface IFlow
    {
        /// <summary>
        /// Get Recovery Mode
        /// </summary>
        RecoveryMode RecoveryMode { get; }

        /// <summary>
        /// Logger use by flow's job.
        /// </summary>
        ILog Logger { get; set; }

        /// <summary>
        /// Monitor use by flow's job. 
        /// </summary>
        IMonitor Monitor { get; set; }

        string Name { get; }

        Dictionary<string, object> WorkingMemory { get; }

        /// <summary>
        /// Get Execution result by Job Name.
        /// </summary>
        /// <param name="JobName"> Job's name which you want to get execution result. </param>
        /// <returns> Execution Result </returns>
        JobResult GetJobResult(string JobName);

        /// <summary>
        /// Get Execution result by Job.
        /// </summary>
        /// <param name="JobObj"> Job which you want to get execution result. </param>
        /// <returns> Execution Result. </returns>
        JobResult GetJobResult(IJob JobObj);

        /// <summary>
        /// Execut all job.
        /// </summary>
        /// <returns></returns>
        JobResult RunAllJob();

        JobResult RunFinalizeJob();

        JobResult RunJob(string JobName);
    }
}