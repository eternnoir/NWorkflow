using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWorkflow.Exceptions;
using NWorkflow.Flows;

namespace NWorkflow.Test
{
    [TestFixture]
    public class AsyncFlowTest
    {
        public AsyncFlowTest()
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
            var flow = new AsyncFlow("Asyncflow1");
            var job1 = new FakeJob("AsyncJob1");
            var job11 = new FakeJob("AsyncJob1");
            try
            {
                flow.AddJob(job1, AsyncType.ASYNC);
                flow.AddJob(job11, AsyncType.ASYNC);
                Assert.Fail("FAIL");
            }
            catch (JobNameExistException)
            {
            }
        }

        [Test]
        public void TestExecJobs()
        {
            var flow = new AsyncFlow("Asyncflow1");
            var job1 = new FakeJob("AsyncJob1", sleepTime: 5*1000);
            var job2 = new FakeJob("AsyncJob2", sleepTime: 5*1000);
            try
            {
                flow.AddJob(job1, AsyncType.ASYNC);
                flow.AddJob(job2, AsyncType.ASYNC);
                flow.RunAllJob();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}