using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NWorkflow;
using NWorkflow.Exceptions;

namespace NWorkflow.Test
{
    public class FakeJob:Job
    {
        private JobResult result;
        private JobException je;
        private int sleepTime;

        public FakeJob(string jobName, JobResult fakeResult = JobResult.SUCCESS, JobException je = JobException.NONE, int sleepTime = 0)
            : base(jobName)
        {
            result = fakeResult;
            this.Message = "NotRun.";
            this.je = je;
            this.sleepTime = sleepTime;
        }

        public override void Init()
        {
        }

        public override JobResult Execute()
        {
            this.Logger.InfoFormat("Job {0} Start.", this.JobName);
            this.Message = "Run.";
            this.Logger.InfoFormat("Job {0} Sleep {1}.", this.JobName, this.sleepTime);
            Thread.Sleep(sleepTime);
            this.Logger.InfoFormat("Job {0} awake.", this.JobName);

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
