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
                    try
                    {
                        ProcessJob(job);
                    }
                    catch (ResumeJobException rje)
                    {
                        continue;
                    }
                    catch (InterruptJobException ije)
                    {
                        this.recover.DoRecover();
                        break;
                    }
                    catch (Exception ex)
                    {
                        this.recover.DoRecover();
                        break;
                    }
                }

        }

        private void ProcessJob(IJob job)
        {
            if (this.ExecuteJob(job) != JobResult.SUCCESS)
            {
                jobResultDic[job] = JobResult.FAIL;
                throw new InterruptJobException("",job);
            }
        }

        public override JobResult RunJob(string JobName)
        {
            if (!jobNameDic.ContainsKey(JobName))
            {
                throw new JobNotFoundException(this, "Job " + JobName + " Not Found.");
            }
            var job = jobNameDic[JobName];
            return this.ExecuteJob(job); 
        }


        public override JobResult GetJobResult(string JobName)
        {
            if (!jobNameDic.ContainsKey(JobName))
            {
                throw new JobNotFoundException(this, "Job " + JobName + " Not Found.");
            }
            return GetJobResult(jobNameDic[JobName]);
        }

        public override JobResult GetJobResult(IJob JobObj)
        {
            if (!jobResultDic.ContainsKey(JobObj))
            {
                throw new JobNotFoundException(this, "Job " + JobObj.JobName + " Not Found.");
            }
            return jobResultDic[JobObj];
        }
    }
}
