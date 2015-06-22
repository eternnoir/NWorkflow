using NLogging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWorkflow
{
    public class Flow : IFlow
    {
        private ILogger logger;
        

        public RecoveryMode RecoveryMode
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ILogger Logger
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Monitoring.IMonitor Monitor
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void RunAllJob()
        {
            throw new NotImplementedException();
        }

        public JobResult RunJon(string JobName)
        {
            throw new NotImplementedException();
        }
    }
}
