namespace NWorkflow.Monitoring
{
    #region

    using System.Collections.Generic;

    #endregion

    public class Monitor : IMonitor
    {
        private List<IHandler> handlerList;

        private MonitorLevel level;

        private readonly object syncObj = new object();

        public Monitor(string MonitorName, MonitorLevel Level = MonitorLevel.INFO)
        {
            this.Name = MonitorName;
            this.level = Level;
            this.handlerList = new List<IHandler>();
        }

        public string Name { get; private set; }

        public void SetLevel(MonitorLevel Level)
        {
            lock (this.syncObj)
            {
                this.level = Level;
            }
        }

        public void AddHandler(IHandler handler)
        {
            lock (this.syncObj)
            {
                if (handler != null)
                {
                    this.handlerList.Add(handler);
                }
            }
        }

        public void Critical(string message)
        {
            this.PushMessage(MonitorLevel.CRITICAL, message);
        }

        public void Error(string message)
        {
            this.PushMessage(MonitorLevel.ERROR, message);
        }

        public void Warning(string message)
        {
            this.PushMessage(MonitorLevel.WARNING, message);
        }

        public void Info(string message)
        {
            this.PushMessage(MonitorLevel.INFO, message);
        }

        public void PushMessage(MonitorLevel level, string message)
        {
            var info = new Infomation(level, message);
            foreach (var handler in this.handlerList)
            {
                handler.Push(info);
            }
        }

        private bool CanLog(MonitorLevel level)
        {
            if (level < this.level)
            {
                return false;
            }

            return true;
        }
    }
}