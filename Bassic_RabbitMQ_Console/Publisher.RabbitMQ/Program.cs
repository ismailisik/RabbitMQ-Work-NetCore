using RabbitMQ.Client;
using System;
using System.Text;

namespace Publisher.RabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri=new Uri("amqp://localhost");

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("Hello_RabbitMQ", false, false, false, null);

                    var MyMessage ="Bu RabbitMQ Test Messajıdır.";

                    var bodyByte = Encoding.UTF8.GetBytes(MyMessage);

                    channel.BasicPublish("", "Hello_RabbitMQ",null,bodyByte);

                    Console.WriteLine("Messajınız İletildi");
                }

                Console.WriteLine("Çıkmak İçin Bir Tuşa Basınız...");
                Console.ReadLine();
            }
        }
    }
}
