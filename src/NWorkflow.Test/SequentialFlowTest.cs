using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NWorkflow.Exceptions;

namespace NWorkflow.Test
{
    [TestFixture]
    public  class SequentialFlowTest
    {
        [Test]
        public void TestJobNameExist()
        {
            var flow = new SequentialFlow("flow1");
            var job1 = new FakeJob("Job1"); 
            var job11 = new FakeJob("Job1");
            try
            {
                flow.AddJob(job1);
                flow.AddJob(job11);
                Assert.True(false);
            }
            catch (JobNameExistException )
            {
                Assert.True(true);
            }
        }

        [Test]
        public void TestExecJobs()
        {
            var flow = new SequentialFlow("flow1");
            var job1 = new FakeJob("Job1");
            var job2 = new FakeJob("Job2");
            try
            {
                flow.AddJob(job1);
                flow.AddJob(job2);
                flow.RunAllJob();
                Assert.True(true);
            }
            catch (Exception )
            {
                Assert.True(false);
            }
        }

        [Test]
        public void TestExecJobsResult()
        {
            var flow = new SequentialFlow("flow1");
            var job1 = new FakeJob("Job1");
            var job2 = new FakeJob("Job2",JobResult.FAIL);
            try
            {
                flow.AddJob(job1);
                flow.AddJob(job2);
                flow.RunAllJob();
                Assert.True(true);
            }
            catch (Exception )
            {
                Assert.True(false);
            }

            Assert.AreEqual(flow.GetJobResult(job2), JobResult.FAIL);
            Assert.AreEqual(flow.GetJobResult(job1), JobResult.SUCCESS);
        }

        [Test]
        public void TestExecJobsResultByName()
        {
            var flow = new SequentialFlow("flow1");
            var job1 = new FakeJob("Job1");
            var job2 = new FakeJob("Job2",JobResult.FAIL);
            try
            {
                flow.AddJob(job1);
                flow.AddJob(job2);
                flow.RunAllJob();
                Assert.True(true);
            }
            catch (Exception )
            {
                Assert.True(false);
            }

            Assert.AreEqual(flow.GetJobResult("Job2"), JobResult.FAIL);
            Assert.AreEqual(flow.GetJobResult("Job1"), JobResult.SUCCESS);
        }


        [Test]
        public void TestExecJobsInturreptException()
        {
            var flow = new SequentialFlow("flow1");
            var job1 = new FakeJob("Job1");
            var job2 = new FakeJob("Job2", JobResult.SUCCESS);
            var job3 = new FakeJob("Job3", JobResult.SUCCESS, JobException.INTERRUPT);
            var job4 = new FakeJob("Job4", JobResult.SUCCESS);
            var job5 = new FakeJob("Job5", JobResult.SUCCESS);
            var job6 = new FakeJob("Job6", JobResult.SUCCESS);
            try
            {
                flow.AddJob(job1);
                flow.AddJob(job2);
                flow.AddJob(job3);
                flow.AddJob(job4);
                flow.AddJob(job5);
                flow.AddJob(job6);
                flow.RunAllJob();
                Assert.True(true);
            }
            catch (Exception )
            {
                Assert.True(false);
            }

            Assert.AreEqual(flow.GetJobResult("Job1"), JobResult.SUCCESS);
            Assert.AreEqual(job3.Message, "Recovery.");
            Assert.AreEqual(job2.Message, "Recovery.");
            Assert.AreEqual(job1.Message, "Recovery.");
            Assert.AreEqual(job4.Message, "NotRun.");
            Assert.AreEqual(job5.Message, "NotRun.");
        }

        [Test]
        public void TestExecJobsResumeException()
        {
            var flow = new SequentialFlow("flow1");
            var job1 = new FakeJob("Job1");
            var job2 = new FakeJob("Job2", JobResult.SUCCESS);
            var job3 = new FakeJob("Job3", JobResult.SUCCESS, JobException.RESUME);
            var job4 = new FakeJob("Job4", JobResult.SUCCESS);
            var job5 = new FakeJob("Job5", JobResult.SUCCESS);
            var job6 = new FakeJob("Job6", JobResult.SUCCESS);
            try
            {
                flow.AddJob(job1);
                flow.AddJob(job2);
                flow.AddJob(job3);
                flow.AddJob(job4);
                flow.AddJob(job5);
                flow.AddJob(job6);
                flow.RunAllJob();
                Assert.True(true);
            }
            catch (Exception )
            {
                Assert.True(false);
            }

            Assert.AreEqual(flow.GetJobResult("Job1"), JobResult.SUCCESS);
            Assert.AreEqual(flow.GetJobResult("Job3"), JobResult.NOTRUN);
            Assert.AreEqual(job3.Message, "Run.");
            Assert.AreEqual(job2.Message, "Run.");
            Assert.AreEqual(job1.Message, "Run.");
            Assert.AreEqual(job4.Message, "Run.");
            Assert.AreEqual(job5.Message, "Run.");
        }
    }
}
