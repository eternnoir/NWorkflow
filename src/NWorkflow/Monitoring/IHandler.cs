using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWorkflow.Monitoring
{
    public interface IHandler
    {
        void Push(Infomation info);
        void Flush();
    }
}
