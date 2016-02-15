namespace NWorkflow.Recovery
{
    #region

    using System;
    using System.Diagnostics;

    using NWorkflow.Exceptions;

    #endregion

    public static class RecoverFactory
    {
        public static IRecover GetRecovery(RecoveryMode mode, IFlow flow)
        {
            IRecover ret = null;
            switch (mode)
            {
                case RecoveryMode.ONCE:
                    throw new NotImplementedException();

                case RecoveryMode.STACK:
                    ret = new StackRecover(flow);
                    break;

                default:
                    throw new RecoverModeException("Not support recovery mode.");
            }

            Debug.Assert(ret != null, "Return IRecover must have value");  // ret must have value.
            return ret;
        }
    }
}