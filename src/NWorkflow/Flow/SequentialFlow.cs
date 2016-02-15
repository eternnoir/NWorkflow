namespace NWorkflow
{
    #region

    using System;
    using System.Collections.Generic;

    using NWorkflow.Exceptions;
    using NWorkflow.Recovery;

    #endregion

    public class SequentialFlow : Flow
    {
        private readonly List<IJob> jobList;
        private readonly List<IJob> finalizeJobList;

        public SequentialFlow(string FlowName, RecoveryMode recoveryMode = RecoveryMode.STACK)
            : base(FlowName, recoveryMode)
        {
            this.jobList = new List<IJob>();
            this.finalizeJobList = new List<IJob>();
            this.jobResultDic = new Dictionary<IJob, JobResult>();
            this.jobNameDic = new Dictionary<string, IJob>();
        }

        public void AddJob(IJob job)
        {
            if (this.jobNameDic.ContainsKey(job.JobName))
            {
                throw new JobNameExistException(this, job, "Job " + job.JobName + " Exist.");
            }

            this.jobList.Add(job);
            job.Flow = this;
            this.jobResultDic.Add(job, JobResult.NOTRUN);
            this.jobNameDic.Add(job.JobName, job);
        }


        public void AddFinalizeJob(IJob job)
        {
            if (this.jobNameDic.ContainsKey(job.JobName))
            {
                throw new JobNameExistException(this, job, "Job " + job.JobName + " Exist.");
            }

            this.finalizeJobList.Add(job);
            job.Flow = this;
            this.jobResultDic.Add(job, JobResult.NOTRUN);
            this.jobNameDic.Add(job.JobName, job);
        }

        public override JobResult RunAllJob()
        {
            this.Logger.DebugFormat("Flow {0}. Start.", this.JobName);
            var result = this.RunJobList(this.jobList);
            this.RunFinalizeJob();
            return result;
        }

        public override JobResult RunFinalizeJob()
        {
            this.Logger.DebugFormat("Flow {0}. FinalizeJob Start.", this.Name);
            return this.RunJobList(this.finalizeJobList);
        }

        private JobResult RunJobList(List<IJob> jobList)
        {
            var resutlt = JobResult.SUCCESS;
            foreach (var job in this.jobList)
            {
                try
                {
                    this.Logger.DebugFormat("Flow {0}. Start Job {1}", this.JobName, job.JobName);
                    this.ProcessJob(job);
                }
                catch (ResumeJobException rje)
                {
                    resutlt = JobResult.FAIL;
                    this.Logger.DebugFormat(
                        "Flow {0}. Job {1} Fail. [Message] {2}",
                        this.JobName,
                        job.JobName,
                        rje.Message);
                }
                catch (InterruptJobException ije)
                {
                    this.Logger.DebugFormat(
                        "Flow {0}. Job {1} Fail. [Message] {2}",
                        this.JobName,
                        job.JobName,
                        ije.Message);
                    this.DoRecover();
                    resutlt = JobResult.FAIL;
                    break;
                }
                catch (Exception ex)
                {
                    this.Logger.DebugFormat(
                        "Flow {0}. Job {1} Fail. [Message] {2}",
                        this.JobName,
                        job.JobName,
                        ex.Message);
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
                this.jobResultDic[job] = JobResult.FAIL;
                throw new InterruptJobException(string.Empty, job);
            }
        }

        public override JobResult RunJob(string JobName)
        {
            if (!this.jobNameDic.ContainsKey(JobName))
            {
                throw new JobNotFoundException(this, "Job " + JobName + " Not Found.");
            }

            var job = this.jobNameDic[JobName];
            return this.ExecuteJob(job);
        }

        public override JobResult GetJobResult(string JobName)
        {
            if (!this.jobNameDic.ContainsKey(JobName))
            {
                throw new JobNotFoundException(this, "Job " + JobName + " Not Found.");
            }

            return this.GetJobResult(this.jobNameDic[JobName]);
        }

        public override JobResult GetJobResult(IJob JobObj)
        {
            if (!this.jobResultDic.ContainsKey(JobObj))
            {
                throw new JobNotFoundException(this, "Job " + JobObj.JobName + " Not Found.");
            }

            return this.jobResultDic[JobObj];
        }

        public override void Init()
        {
        }
    }
}