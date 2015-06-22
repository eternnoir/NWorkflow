﻿using System;
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
            catch (JobNameExistException jnee)
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
            catch (Exception ex)
            {
                Assert.True(false);
            }
        }

    }
}
