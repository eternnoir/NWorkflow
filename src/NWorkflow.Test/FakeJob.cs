using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NWorkflow;

namespace NWorkflow.Test
{
    public class FakeJob:Job
    {
        public FakeJob(string jobName) : base(jobName) { }
        public override void Init()
        {
        }

        public override JobResult Execute()
        {
            return JobResult.SUCCESS;
        }
    }
}
