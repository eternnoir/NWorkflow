using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NWorkflow.Exceptions;
using NWorkflow.Flows;

namespace NWorkflow.Test
{
    [TestFixture]
    public class SequentialFlowTest
    {
        public SequentialFlowTest()
        {
            log4net.Config.BasicConfigurator.Configure(
                new log4net.Appender.ConsoleAppender
                {
                    Layout = new log4net.Layout.SimpleLayout()
                });
        }

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
                Assert.Fail("FAIL");
            }
            catch (JobNameExistException)
            {
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
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void TestExecJobsResult()
        {
            var flow = new SequentialFlow("flow1");
            var job1 = new FakeJob("Job1");
            var job2 = new FakeJob("Job2", JobResult.FAIL);
            try
            {
                flow.AddJob(job1);
                flow.AddJob(job2);
                flow.RunAllJob();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.AreEqual(flow.GetJobResult(job2), JobResult.FAIL);
            Assert.AreEqual(flow.GetJobResult(job1), JobResult.SUCCESS);
        }

        [Test]
        public void TestExecJobsResultByName()
        {
            var flow = new SequentialFlow("flow1");
            var job1 = new FakeJob("Job1");
            var job2 = new FakeJob("Job2", JobResult.FAIL);
            try
            {
                flow.AddJob(job1);
                flow.AddJob(job2);
                flow.RunAllJob();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
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
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }


            Assert.AreEqual(flow.GetJobResult("Job1"), JobResult.SUCCESS);
            Assert.AreEqual(flow.GetJobResult("Job5"), JobResult.NOTRUN);
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
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.AreEqual(flow.GetJobResult("Job1"), JobResult.SUCCESS);
            Assert.AreEqual(flow.GetJobResult("Job3"), JobResult.FAIL);
            Assert.AreEqual(flow.GetJobResult("Job5"), JobResult.SUCCESS);
            Assert.AreEqual(job3.Message, "Run.");
            Assert.AreEqual(job2.Message, "Run.");
            Assert.AreEqual(job1.Message, "Run.");
            Assert.AreEqual(job4.Message, "Run.");
            Assert.AreEqual(job5.Message, "Run.");
        }

        [Test]
        public void TestFinalizeJob()
        {
            var flow = new SequentialFlow("flow1");
            var job2 = new FakeJob("Job2", JobResult.SUCCESS);
            var job3 = new FakeJob("Job3", JobResult.SUCCESS);
            var job4 = new FakeJob("Job4", JobResult.SUCCESS);
            try
            {
                flow.AddJob(job2);
                flow.AddJob(job3);
                flow.AddFinalizeJob(job4);
                flow.RunAllJob();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.AreEqual(job3.Message, "Run.");
            Assert.AreEqual(job2.Message, "Run.");
            Assert.AreEqual(job4.Message, "Run.");
        }

        [Test]
        public void TestFinalizeWithFailJob()
        {
            var flow = new SequentialFlow("flow1");
            var job2 = new FakeJob("Job2", JobResult.SUCCESS);
            var job3 = new FakeJob("Job3", JobResult.SUCCESS, JobException.INTERRUPT);
            var job4 = new FakeJob("Job4", JobResult.SUCCESS);
            try
            {
                flow.AddJob(job2);
                flow.AddJob(job3);
                flow.AddFinalizeJob(job4);
                flow.RunAllJob();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.AreEqual(flow.GetJobResult("Job2"), JobResult.SUCCESS);
            Assert.AreEqual(flow.GetJobResult("Job3"), JobResult.FAIL);
            Assert.AreEqual(flow.GetJobResult("Job4"), JobResult.SUCCESS);
            Assert.AreEqual(job3.Message, "Recovery.");
            Assert.AreEqual(job2.Message, "Recovery.");
            Assert.AreEqual(job4.Message, "Run.");
        }
    }
}