namespace NWorkflow.Recovery
{
    public interface IRecover
    {
        void AppendRunedJob(IJob job);

        bool DoRecover();
    }
}