namespace NWorkflow.Exceptions
{
    public class InterruptJobException : JobException
    {
        public InterruptJobException(string message, IJob job)
            : base(message, job)
        {
        }
    }
}