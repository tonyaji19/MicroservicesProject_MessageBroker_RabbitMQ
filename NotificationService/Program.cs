using NotificationService.Consumers;

namespace NotificationService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var consumer = new ProductMessageConsumer();
            consumer.StartConsuming();
        }
    }
}
