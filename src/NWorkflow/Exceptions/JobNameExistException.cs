namespace NWorkflow.Exceptions
{
    public class JobNameExistException : NWorkflowException
    {
        public JobNameExistException(IFlow Flow, IJob Job, string message)
            : base(message)
        {
            this.Job = Job;
            this.Flow = Flow;
        }

        public IJob Job { get; private set; }

        public IFlow Flow { get; private set; }
    }
}