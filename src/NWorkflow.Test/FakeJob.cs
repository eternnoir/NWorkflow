using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NWorkflow;

namespace NWorkflow.Test
{
    public class FakeJob:Job
    {
        private JobResult result;

        public FakeJob(string jobName, JobResult fakeResult = JobResult.SUCCESS)
            : base(jobName)
        {

            result = fakeResult;
        }
        public override void Init()
        {
        }

        public override JobResult Execute()
        {
            return result;
        }
    }
}
