using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ListenerService
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            factory.UserName = "guest";
            factory.Password = "guest";
            IConnection conn = factory.CreateConnection();

            //ConnectionFactory _factory = new ConnectionFactory();
            //_factory.Uri = new Uri("amqp://guest:guest@rabbitmq:5672/");
            //IConnection conn = _factory.CreateConnection();

            IModel channel = conn.CreateModel();
            channel.QueueDeclare(queue: "MyUserQueue",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine(message);
            };
            channel.BasicConsume(queue: "MyUserQueue",
                                    autoAck: true,
                                    consumer: consumer);

            // put thread to sleep to wait until event is fired upon retreival of new message from RabbitMQ
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
        }
    }
}
