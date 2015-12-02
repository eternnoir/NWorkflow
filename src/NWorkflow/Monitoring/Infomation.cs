namespace NWorkflow.Monitoring
{
    public class Infomation
    {
        public Infomation(MonitorLevel level, string message)
        {
            this.Level = level;
            this.Message = message;
        }

        public MonitorLevel Level { get; private set; }

        public string Message { get; private set; }
    }
}