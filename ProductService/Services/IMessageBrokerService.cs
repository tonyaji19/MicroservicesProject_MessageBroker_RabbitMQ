namespace ProductService.Services
{
    public interface IMessageBrokerService
    {
        void PublishMessage(string message);
    }
}
