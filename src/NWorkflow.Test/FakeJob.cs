using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NWorkflow;
using NWorkflow.Exceptions;

namespace NWorkflow.Test
{
    public class FakeJob:Job
    {
        private JobResult result;
        private JobException je;

        public FakeJob(string jobName, JobResult fakeResult = JobResult.SUCCESS, JobException je = JobException.NONE)
            : base(jobName)
        {
            result = fakeResult;
            this.Message = "NotRun.";
            this.je = je;
        }

        public override void Init()
        {
        }

        public override JobResult Execute()
        {
            this.Message = "Run.";
            switch (this.je)
            {
                case JobException.NONE:
                    break;
                case JobException.INTERRUPT:
                    throw new InterruptJobException("", this);
                case JobException.RESUME:
                    throw new ResumeJobException("", this);
            }
            return result;
        }
        public string Message { get; set; }

        public override void DoRecover()
        {
            this.Message = "Recovery.";
        }
    }

    public enum JobException
    {
      NONE,  INTERRUPT, RESUME
    }
}
