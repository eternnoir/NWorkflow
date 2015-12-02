namespace NWorkflow.Monitoring
{
    public interface IMonitor
    {
        /// <summary>
        ///     Name getter.
        /// </summary>
        string Name { get; }

        void SetLevel(MonitorLevel level);

        void AddHandler(IHandler handler);

        void Critical(string message);

        void Error(string message);

        void Warning(string message);

        void Info(string message);

        void PushMessage(MonitorLevel level, string message);
    }
}