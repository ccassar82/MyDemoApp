using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI
{
    public interface IMessageService
    {
        bool Enqueue(string message);
    }

    ///<Summary>
    /// This is the class that handles communication with RabbitMQ
    ///</Summary>
    public class MessageService : IMessageService
    {
        ConnectionFactory _factory;
        IConnection _conn;
        IModel _channel;
        public MessageService()
        {
            _factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            _factory.UserName = "guest";
            _factory.Password = "guest";

            //_factory = new ConnectionFactory();
            //_factory.Uri = new Uri("amqp://guest:guest@localhost:5672/");

            _conn = _factory.CreateConnection();
            _channel = _conn.CreateModel();
            _channel.QueueDeclare(queue: "MyUserQueue",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
        }

        /// <summary>
        /// This method is the main method to put message to RabbitMQ Queue
        /// </summary>
        /// <param name="messageString">The message to send to queue</param>
        /// <returns></returns>
        public bool Enqueue(string messageString)
        {
            var body = Encoding.UTF8.GetBytes("UserCreated");
            _channel.BasicPublish(exchange: "",
                                routingKey: "MyUserQueue",
                                basicProperties: null,
                                body: body);
            Console.WriteLine("Published {0} to RabbitMQ", messageString);
            return true;
        }

    }
}
