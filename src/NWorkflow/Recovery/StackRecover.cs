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
                    // TODO Write some log, monitor
                    result = false;
                }
                catch (InterruptJobException ije)
                {
                    // TODO Write some log, monitor
                    result = false;
                    break;
                }
                catch (Exception ex)
                {
                    // TODO Write some log, monitor
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