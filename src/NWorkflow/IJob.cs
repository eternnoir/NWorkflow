using NLogging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWorkflow
{
    public interface IJob
    {
        IFlow Flow
        {
            get;
            set;
        }

        string JobName
        {
            get;
        }

        ILogger Logger
        {
            get;
        }
        void Init();
        JobResult Execute();
    }
}
