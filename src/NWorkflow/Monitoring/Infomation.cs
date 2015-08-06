using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWorkflow.Monitoring
{
    public class Infomation
    {
        private MonitorLevel level;
        private string message;

        public Infomation(MonitorLevel level, string message)
        {
            this.level = level;
            this.message = message;
        }

        public MonitorLevel Level
        {
            get
            {
                return this.level;
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
        }
    }
}
