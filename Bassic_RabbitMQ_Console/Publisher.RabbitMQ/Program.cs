using Publisher.RabbitMQ.Entity;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
                    ////Exchange Type Direct: Publisher tarafından oluşturulan mesaj kendini dinleyen ve routingKey'leri eşleşen consumerlara gönderir.
                    //channel.ExchangeDeclare("direct-exchange", type: ExchangeType.Direct, durable: true);

                    //var logNames = Enum.GetValues(typeof(LogNames));

                    //for (int i = 0; i < 10; i++)
                    //{
                    //    Random rnd = new Random();
                    //    LogNames name =(LogNames)logNames.GetValue(rnd.Next(logNames.Length));
                    //    var bodyByte = Encoding.UTF8.GetBytes($"LOG--{name.ToString()}");

                    //    //Property tanımlama...
                    //    var property = channel.CreateBasicProperties();
                    //    property.Persistent = true;

                    //    //Exchange tanımladıysan ona göre burada onu vermen gerekmektedir.

                    //    //Not:RouteKey direct exchange de önemi belirli routeKey lere sahip consumerlara mesajı ileticek.
                    //    Thread.Sleep(1000);
                    //    channel.BasicPublish("direct-exchange", routingKey: name.ToString(), property, bodyByte);

                    //    Console.WriteLine($"Messajınız İletildi Log:--{name.ToString()}");
                    //}
                    #endregion

                    #region Topic Exchange
                    ////Exchange Type Topic: Publisher tarafından oluşturulan routingKey complex yapıda olabilir(Warning.Critical.Error v.b). Ben eğer belirli bir konuda(ör/: Warning) mesaj yollayan tüm publisher ları dinlemek istiyorsam routeKey'inde benim ilgilendiğim konu  var mı bakmalıyım... Bu tarz case'lerde Topic Exchange kullanılır.  
                    //channel.ExchangeDeclare("topic-exchange", type: ExchangeType.Topic, durable: true);

                    //var logNames = Enum.GetValues(typeof(LogNames));

                    //for (int i = 0; i < 10; i++)
                    //{
                    //    Random rnd = new Random();
                    //    LogNames name_1 = (LogNames)logNames.GetValue(rnd.Next(logNames.Length));
                    //    LogNames name_2 = (LogNames)logNames.GetValue(rnd.Next(logNames.Length));
                    //    LogNames name_3 = (LogNames)logNames.GetValue(rnd.Next(logNames.Length));

                    //    var routeKey = $"{name_1.ToString()}.{name_2.ToString()}.{name_3.ToString()}";

                    //    var bodyByte = Encoding.UTF8.GetBytes($"LOG--{name_1.ToString()}--{name_2.ToString()}--{name_3.ToString()}");

                    //    //Property tanımlama...
                    //    var property = channel.CreateBasicProperties();
                    //    property.Persistent = true;

                    //    //Exchange tanımladıysan ona göre burada onu vermen gerekmektedir.

                    //    //Not:RouteKey direct exchange de önemi belirli routeKey lere sahip consumerlara mesajı ileticek.
                    //    Thread.Sleep(1000);
                    //    channel.BasicPublish("topic-exchange", routingKey: routeKey, property, bodyByte);

                    //    Console.WriteLine($"Messajınız İletildi Log:--{routeKey}");
                    //}
                    #endregion

                    #region Header Exchange
                    ////Exchange Type Header: Publisher tarafından header oluşturulur. Bu header key value şeklinde belirlenir(key:"Value").Önceki exchange type lardan farkı routingKey üzerinden değil header üzerinden kendisini dinlemek isteyenler dinleyebilir. Topic exchange ye göre daha esneklik sağlar.
                    //channel.ExchangeDeclare("header-exchange", type: ExchangeType.Headers,true);

                    ////Header oluşturalım..
                    //var properties = channel.CreateBasicProperties();
                    //Dictionary<string, object> headers = new Dictionary<string, object>();
                    //headers.Add("status", false);
                    //headers.Add("format", "Image");

                    //properties.Headers = headers;

                    ////Header oluşturdum Channel'i publish etmem lazım
                    //channel.BasicPublish("header-exchange", String.Empty, properties, Encoding.UTF8.GetBytes("Bu Header Exchange Mesajıdır.."));

                    #endregion


                    //***RabbitMQ CompexType ile çalışma***//

                    channel.QueueDeclare("Compex_Type", true,false,false,null);


                    var TestList = new List<Test>() 
                    {
                        new Test{Id=1,Name="İsmail",LastName="IŞIK",Address="Bayrampaşa/İstanbul"},
                        new Test{Id=2,Name="Harun",LastName="Güzel",Address="Bayrampaşa/İstanbul"},
                        new Test{Id=3,Name="Jordan",LastName="IŞIK",Address="Bayrampaşa/İstanbul"}
                    };

                    var JsonTestList = JsonSerializer.Serialize(TestList);
                    var base64TestList = Encoding.UTF8.GetBytes(JsonTestList);
                  
                    channel.BasicPublish("",routingKey: "Compex_Type", null, base64TestList);

                }
                
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
