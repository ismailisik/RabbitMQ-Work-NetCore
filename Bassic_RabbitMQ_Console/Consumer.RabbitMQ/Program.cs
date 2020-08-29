using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer.RabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://localhost");

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("Hello_RabbitMQ", false, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);
                    channel.BasicConsume("Hello_RabbitMQ", true, consumer);

                    //Publisher tarafından gönderilemn messajları Recived event'ı ile dinlemeye başladık.
                    consumer.Received += (model, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        Console.WriteLine($"Mesajınız: {message}");
                    };

                    Console.WriteLine("Çıkmak İçin Bir Tuşa Basınız...");
                    Console.ReadLine();

                }

            }
        }
    }
}
