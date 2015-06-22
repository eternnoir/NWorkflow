using NWorkflow.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWorkflow
{
    public class SequentialFlow : Flow
    {
        private List<IJob> jobList;
        private Dictionary<IJob, JobResult> jobResultDic;
        private Dictionary<string, IJob> jobNameDic;

        public SequentialFlow(string FlowName)
            : base(FlowName)
        {
            jobList = new List<IJob>();
            jobResultDic = new Dictionary<IJob, JobResult>();
            jobNameDic = new Dictionary<string, IJob>();
        }

        public void AddJob(IJob Job)
        {
            if (jobNameDic.ContainsKey(Job.JobName))
            {
                throw new JobNameExistException(this, Job, "Job " + Job.JobName + " Exist.");
            }
            this.jobList.Add(Job);
            this.jobResultDic.Add(Job, JobResult.NOTRUN);
            this.jobNameDic.Add(Job.JobName, Job);

        }

        public override void RunAllJob()
        {
            foreach (var job in jobList)
            {
                if (!ExecuteJob(job))
                {
                    break;
                }
            }
        }

        public override JobResult RunJob(string JobName)
        {
            if (!jobNameDic.ContainsKey(JobName))
            {
                throw new JobNotFoundException(this, "Job " + JobName + " Not Found.");
            }
            var job = jobNameDic[JobName];
            job.Init();
            return job.Execute();
        }

        private bool ExecuteJob(IJob Job)
        {
            Job.Init();
            var result = Job.Execute();
            jobResultDic[Job] = result;
            if (result == JobResult.SUCCESS)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
