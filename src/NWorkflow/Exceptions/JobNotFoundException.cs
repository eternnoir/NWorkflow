namespace NWorkflow.Exceptions
{
    public class JobNotFoundException : NWorkflowException
    {
        public JobNotFoundException(IFlow flow, string message)
            : base(message)
        {
            this.Flow = flow;
        }

        public IFlow Flow { get; private set; }
    }
}