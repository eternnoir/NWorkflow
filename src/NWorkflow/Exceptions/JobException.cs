namespace NWorkflow.Exceptions
{
    public class JobException : NWorkflowException
    {
        private IJob job;

        public JobException(string message, IJob job)
            : base(message)
        {
            this.job = job;
        }
    }
}