namespace NWorkflow.Exceptions
{
    public class ResumeJobException : JobException
    {
        public ResumeJobException(string message, IJob job)
            : base(message, job)
        {
        }
    }
}