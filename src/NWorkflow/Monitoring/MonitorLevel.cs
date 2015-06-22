using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWorkflow.Monitoring
{
    public enum MonitorLevel : int
    {
        CRITICAL = 50,
        ERROR = 40,
        WARNING = 30,
        INFO = 20,
        NOTSET = 0,
    }
}
