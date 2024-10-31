using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace NotificationService.Consumers
{
    public class ProductMessageConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public ProductMessageConsumer()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "product_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received message: {message}");
            };
            _channel.BasicConsume(queue: "product_queue", autoAck: true, consumer: consumer);
            Console.WriteLine("Consumer started. Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
