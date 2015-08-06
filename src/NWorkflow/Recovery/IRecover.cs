using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWorkflow.Recovery
{
    public interface IRecover
    {
        void AppendRunedJob(IJob job);
        bool DoRecover();
    }
}
