using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWorkflow
{
    abstract public class Job :IJob
    {
        private IFlow flow;

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
    }
}
