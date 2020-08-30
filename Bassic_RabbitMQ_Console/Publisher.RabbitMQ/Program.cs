using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

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

                    //Burada bir queue declare ettik...
                    //channel.QueueDeclare("test_queue", durable:true, false, false, null);

                    //***RabbitMQ Exchange tipleri incelenecektir***//
                    #region Fanout Exchange
                    ////Exchange Type Fanout: Publisher tarafından oluşturulan mesajları kendisini dinleyen tüm consumer lara gönderir.
                    //channel.ExchangeDeclare("log", type: ExchangeType.Fanout, durable: true);

                    //// var MyMessage ="Bu RabbitMQ Test Messajıdır.";

                    //for (int i = 0; i < 10; i++)
                    //{
                    //    var bodyByte = Encoding.UTF8.GetBytes($"Message--{i}");

                    //    //Property tanımlama...
                    //    var property = channel.CreateBasicProperties();
                    //    property.Persistent = true;

                    //    //Exchange tanımladıysan ona göre burada onu vermen gerekmektedir.

                    //    //Not:RouteKey vermedim çünkü benim exchange tipim fanout bu servisime subscribe olan tüm dinleyicilere gönderecek mesajı.
                    //    Thread.Sleep(1000);
                    //    channel.BasicPublish("log", routingKey: "", property, bodyByte);

                    //    Console.WriteLine("Messajınız İletildi");
                    //}

                    #endregion

                    #region Direct Exchange
                    //Exchange Type Direct: Publisher tarafından oluşturulan mesaj kendini dinleyen ve routingKey'leri eşleşen consumerlara gönderir.
                    channel.ExchangeDeclare("direct-exchange", type: ExchangeType.Direct, durable: true);

                    var logNames = Enum.GetValues(typeof(LogNames));

                    for (int i = 0; i < 10; i++)
                    {
                        Random rnd = new Random();
                        LogNames name =(LogNames)logNames.GetValue(rnd.Next(logNames.Length));
                        var bodyByte = Encoding.UTF8.GetBytes($"LOG--{name.ToString()}");
                 
                        //Property tanımlama...
                        var property = channel.CreateBasicProperties();
                        property.Persistent = true;

                        //Exchange tanımladıysan ona göre burada onu vermen gerekmektedir.

                        //Not:RouteKey direct exchange de önemi belirli routeKey lere sahip consumerlara mesajı ileticek.
                        Thread.Sleep(1000);
                        channel.BasicPublish("direct-exchange", routingKey: name.ToString(), property, bodyByte);

                        Console.WriteLine($"Messajınız İletildi Log:--{name.ToString()}");
                    }
                    #endregion


                }

                Console.WriteLine("Çıkmak İçin Bir Tuşa Basınız...");
                Console.ReadLine();
            }
        }
    }
    public enum LogNames
    {
        Critical=1,
        Error=2,
        Info=3,
        Warning=4
    }
}
