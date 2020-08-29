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
                    //Not: durable: true yapar isem RabbitMQ'nun kurulu olduğu server yada instance reset yese dahi silinmez(Sağlama almış olurum) Buna ek olarak tabiki property de tanımlamak ve Persistance true ye çekmek gerekmektedir. 
                    channel.QueueDeclare("test_queue", durable:true, false, false, null);

                    // var MyMessage ="Bu RabbitMQ Test Messajıdır.";

                    for (int i = 0; i < 10; i++)
                    {
                        var bodyByte = Encoding.UTF8.GetBytes($"Message--{i}");

                        //Property tanımlama...
                        var property = channel.CreateBasicProperties();
                        property.Persistent = true;

                        channel.BasicPublish("", "test_queue", property, bodyByte);

                        Console.WriteLine("Messajınız İletildi");
                    }

                }

                Console.WriteLine("Çıkmak İçin Bir Tuşa Basınız...");
                Console.ReadLine();
            }
        }
    }
}
