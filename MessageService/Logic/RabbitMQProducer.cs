using MessageService.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;


namespace RabbitMQ.Producer
{
    class RabbitMQProducer
    {
        private IConnection connectionString;
        private IModel channel;

        private ConnectionFactory factory;
        public RabbitMQProducer(string connectionstring)
        {

            factory = new ConnectionFactory
            {
                Uri = new Uri(connectionstring)
            };

            StartConnection();

        }

        public void StartConnection()
        {
            this.connectionString = factory.CreateConnection();
            this.channel = connectionString.CreateModel();
            InititializeQueues();
        }

        public void InititializeQueues()
        {
            channel.QueueDeclare("message-queue", true, exclusive: false, autoDelete: false, arguments: null);
        }
        public void Publish(MessageStatistic location)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(location));
            channel.BasicPublish("", routingKey: "message-queue", null, body);
        }
    }
}
