namespace NWorkflow.Exceptions
{
    #region

    using System;

    #endregion

    public class NWorkflowException : Exception
    {
        public NWorkflowException(string message)
            : base(message)
        {
        }
    }
}