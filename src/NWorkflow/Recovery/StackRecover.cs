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
            //TODO not done
            throw new NotImplementedException();
        }

        public void AppendRunedJob(IJob job)
        {
            this.runedJobs.Push(job);
        }
    }
}
