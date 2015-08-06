using NWorkflow.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWorkflow.Recovery
{
    public class StackRecover : IRecover
    {
        private IFlow flow;
        private Stack<IJob> runedJobs;

        public StackRecover(IFlow flow)
        {
            this.flow = flow;
            this.runedJobs = new Stack<IJob>();
        }
        public bool DoRecover()
        {
            bool result = true;
            while (runedJobs.Count > 0)
            {
                var job = runedJobs.Pop();
                try
                {
                    job.DoRecover();
                }
                catch (ResumeJobException rje)
                {
                    //TODO Write some log, monitor
                    result = false;
                    continue;
                }
                catch (InterruptJobException ije)
                {
                    //TODO Write some log, monitor
                    result = false;
                    break;
                }
                catch (Exception ex)
                {
                    //TODO Write some log, monitor
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
