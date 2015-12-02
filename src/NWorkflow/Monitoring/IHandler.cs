namespace NWorkflow.Monitoring
{
    public interface IHandler
    {
        void Push(Infomation info);

        void Flush();
    }
}