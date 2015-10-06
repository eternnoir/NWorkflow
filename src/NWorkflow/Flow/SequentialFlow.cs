using NWorkflow.Exceptions;
using NWorkflow.Recovery;
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

        public SequentialFlow(string FlowName, RecoveryMode recoveryMode = RecoveryMode.STACK)
            : base(FlowName,recoveryMode)
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
            Job.Flow = this;
            this.jobResultDic.Add(Job, JobResult.NOTRUN);
            this.jobNameDic.Add(Job.JobName, Job);

        }

        public override JobResult RunAllJob()
        {
            Logger.DebugFormat("Flow {0}. Start.", JobName);
            JobResult resutlt = JobResult.NOTRUN;
            foreach (var job in jobList)
            {
                try
                {
                    Logger.DebugFormat("Flow {0}. Start Job {1}", JobName, job.JobName);
                    ProcessJob(job);
                }
                catch (ResumeJobException rje)
                {
                    resutlt = JobResult.FAIL;
                    Logger.DebugFormat("Flow {0}. Job {1} Fail. [Message] {2}",JobName,job.JobName,rje.Message);
                    continue;
                }
                catch (InterruptJobException ije)
                {
                    Logger.DebugFormat("Flow {0}. Job {1} Fail. [Message] {2}", JobName, job.JobName, ije.Message);
                    this.DoRecover();
                    resutlt = JobResult.FAIL;
                    break;
                }
                catch (Exception ex)
                {
                    Logger.DebugFormat("Flow {0}. Job {1} Fail. [Message] {2}", JobName, job.JobName, ex.Message);
                    this.DoRecover();
                    resutlt = JobResult.FAIL;
                    break;
                }
            }
            return resutlt;
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

        public override void Init()
        {
        }
    }
}
