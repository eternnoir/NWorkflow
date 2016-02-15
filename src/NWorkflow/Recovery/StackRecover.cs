namespace NWorkflow.Recovery
{
    #region

    using System;
    using System.Collections.Generic;
    using NWorkflow.Exceptions;

    #endregion

    public class StackRecover : IRecover
    {
        private IFlow flow;

        private readonly Stack<IJob> runedJobs;

        public StackRecover(IFlow flow)
        {
            this.flow = flow;
            this.runedJobs = new Stack<IJob>();
        }

        public bool DoRecover()
        {
            var result = true;
            while (this.runedJobs.Count > 0)
            {
                var job = this.runedJobs.Pop();
                try
                {
                    job.DoRecover();
                }
                catch (ResumeJobException rje)
                {
                    flow.Logger.DebugFormat(
                        "Flow {0}. Job {1} Recover Fail. [Message] {2}",
                        job.JobName,
                        job.JobName,
                        rje.Message);
                    result = false;
                }
                catch (InterruptJobException ije)
                {
                    flow.Logger.DebugFormat(
                        "Flow {0}. Job {1} Recover Fail. [Message] {2}",
                        job.JobName,
                        job.JobName,
                        ije.Message);
                    result = false;
                    break;
                }
                catch (Exception ex)
                {
                    flow.Logger.DebugFormat(
                        "Flow {0}. Job {1} Recover Fail. [Message] {2}",
                        job.JobName,
                        job.JobName,
                        ex.Message);
                    result = false;
                    break;
                }
            }

            return result;
        }

        public void AppendRunedJob(IJob job)
        {
            this.runedJobs.Push(job);
        }
    }
}