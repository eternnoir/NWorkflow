using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWorkflow.Exceptions;
using NWorkflow.Recovery;

namespace NWorkflow.Flows
{
    public enum AsyncType
    {
        ASYNC,
        AWAIT
    }

    public class AsyncFlow : Flow
    {
        private readonly List<IJob> jobList;
        private readonly List<IJob> finalizeJobList;
        private List<Task<JobResult>> awaitTaskList;
        private Dictionary<IJob, AsyncType> jobTypeDic;

        public AsyncFlow(string FlowName, RecoveryMode recoveryMode = RecoveryMode.STACK) : base(FlowName, recoveryMode)
        {
            this.jobList = new List<IJob>();
            this.finalizeJobList = new List<IJob>();
            this.jobResultDic = new Dictionary<IJob, JobResult>();
            this.jobNameDic = new Dictionary<string, IJob>();
            this.awaitTaskList = new List<Task<JobResult>>();
            this.jobTypeDic = new Dictionary<IJob, AsyncType>();
        }

        public void AddJob(IJob job, AsyncType type)
        {
            if (this.jobNameDic.ContainsKey(job.JobName))
            {
                throw new JobNameExistException(this, job, "Job " + job.JobName + " Exist.");
            }

            this.jobList.Add(job);
            job.Flow = this;
            this.jobResultDic.Add(job, JobResult.NOTRUN);
            this.jobNameDic.Add(job.JobName, job);
            this.jobTypeDic.Add(job, type);
        }


        public void AddFinalizeJob(IJob job, AsyncType type)
        {
            if (this.jobNameDic.ContainsKey(job.JobName))
            {
                throw new JobNameExistException(this, job, "Job " + job.JobName + " Exist.");
            }

            this.finalizeJobList.Add(job);
            job.Flow = this;
            this.jobResultDic.Add(job, JobResult.NOTRUN);
            this.jobNameDic.Add(job.JobName, job);
            this.jobTypeDic.Add(job, type);
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
            foreach (var job in jobList)
            {
                try
                {
                    this.Logger.DebugFormat("Flow {0}. Start Job {1}", this.JobName, job.JobName);
                    this.ProcessJob(job);
                }
                catch (ResumeJobException rje)
                {
                    resutlt = JobResult.FAIL;
                    this.jobResultDic[job] = JobResult.FAIL;
                    this.Logger.DebugFormat(
                        "Flow {0}. Job {1} Fail. [Message] {2}",
                        this.JobName,
                        job.JobName,
                        rje.Message);
                }
                catch (InterruptJobException ije)
                {
                    this.jobResultDic[job] = JobResult.FAIL;
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
                    this.jobResultDic[job] = JobResult.FAIL;
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
            AwaitAllJob();
            return resutlt;
        }

        private async void ProcessJob(IJob job)
        {
            switch (this.jobTypeDic[job])
            {
                case AsyncType.ASYNC:
                    this.awaitTaskList.Add(Task.Run(() => this.ProcessAsyncJob(job)));
                    break;
                case AsyncType.AWAIT:
                    this.ProcessAwaitJob(job);
                    break;
                default:
                    throw new Exception("Async Type not found.");
            }
        }

        private void AwaitAllJob()
        {
            this.Logger.DebugFormat("Start Await all async jobs.");
            Task.WaitAll(this.awaitTaskList.ToArray());
            if (awaitTaskList.Any(task => task.Result != JobResult.SUCCESS))
            {
                throw new InterruptJobException("Await task Fail.", this);
            }
        }

        private JobResult ProcessAwaitJob(IJob job)
        {
            var resutlt = JobResult.SUCCESS;
            this.AwaitAllJob();
            if (this.ExecuteJob(job) != JobResult.SUCCESS)
            {
                this.jobResultDic[job] = JobResult.FAIL;
                throw new InterruptJobException(string.Empty, job);
            }
            return resutlt;
        }

        private async Task<JobResult> ProcessAsyncJob(IJob job)
        {
            var resutlt = JobResult.SUCCESS;
            try
            {
                this.ExecuteJob(job);
            }
            catch (Exception ex)
            {
                this.jobResultDic[job] = JobResult.FAIL;
                this.Logger.DebugFormat(
                    "Flow {0}. Job {1} Fail. [Message] {2}",
                    this.JobName,
                    job.JobName,
                    ex.Message);
                resutlt = JobResult.FAIL;
            }
            return resutlt;
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