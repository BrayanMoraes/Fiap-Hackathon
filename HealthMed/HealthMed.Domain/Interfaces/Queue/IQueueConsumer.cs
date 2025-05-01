namespace HealthMed.Domain.Interfaces.Queue
{
    public interface IQueueConsumer
    {
        Task StartConsuming();
    }
}
