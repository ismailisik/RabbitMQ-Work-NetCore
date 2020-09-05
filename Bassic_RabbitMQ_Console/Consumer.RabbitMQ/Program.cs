using Consumer.RabbitMQ.Entity;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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

                    //***RabbitMQ Exchange Types***//

                    #region Fanout Exchange Consumer
                    ////channel.QueueDeclare("test_queue", durable: true, false, false, null);

                    ////Exchange: fanout
                    //channel.ExchangeDeclare("log", type: ExchangeType.Fanout, durable: true);

                    ////Ben fanout olarak exchangemi belirledim ve bir publisher oluşturdum ancak o queue ye bir cunsumer bind etmem gerekli.
                    //var queueName = channel.QueueDeclare().QueueName; //Rondom bir queue name üretir.
                    //channel.QueueBind(queue: queueName, "log", routingKey: "");

                    ////Benim Insance'ım tek seferde kaç tane mesaj alacağını aşağıdaki gibi ayarlayabiliri.

                    //channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    ////Yukarıdaki ifade şu demek: Ben her defasında 1 tane mesaj alırım onu işledikten sonra diğerini alırım. False koyduğumuz global parametreside şu demek: Benim tek instancem belirlediğim kadar mesaj alır. Global true yapsaydım varsayalım benim bu instancem'den 5 tane vardı ve ben 10 a çektim mesaj sayımı tüm instanceler totalda 1 defada 10 messaj alabilirdi.(Yani her instance 2 şer mesaj alırdı).

                    //var consumer = new EventingBasicConsumer(channel);

                    ////Bir diğer önemi parametremiz autoAck (Otomatik bilgilendirme: yani ben bu mesajı işledim bilgisini otomatik olarak MQ ya bildiriyor. Ben bunu false yaparsam manuel olarak kontrol ettirmem gerekmektedir).

                    ////channel.BasicConsume("test_queue",autoAck: false, consumer);

                    ////Fanout Exchange
                    //channel.BasicConsume(queueName, false, consumer);

                    ////Publisher tarafından gönderilemn messajları Recived event'ı ile dinlemeye başladık.
                    //consumer.Received += (model, ea) =>
                    //{
                    //    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    //    Console.WriteLine($"Mesajınız: {message}");

                    //    //Alttaki ifade ben mesajı halletim bana yenisini gönderebilirsin mesajını MQ'ya iletir.
                    //    channel.BasicAck(ea.DeliveryTag, multiple: false); //Bu mesajı MQ(brocker) alamazsa mesajı silmez. İşlenemedi demektir.
                    //};

                    //Console.WriteLine("Çıkmak İçin Bir Tuşa Basınız...");
                    //Console.ReadLine();
                    #endregion

                    #region Direct Exchange Consumer
                    ////channel.QueueDeclare("test_queue", durable: true, false, false, null);

                    ////Exchange: Direct
                    //channel.ExchangeDeclare("direct-exchange", type: ExchangeType.Direct, durable: true);

                    ////Ben Direct olarak exchangemi belirledim ve bir publisher oluşturdum ancak bu publisheri spesifik routeKey ile queue ye bir consumer bind etmem gerekli.

                    // var queueName = channel.QueueDeclare().QueueName; //Rondom bir queue name üretir.

                    //foreach (var item in Enum.GetNames(typeof(LogNames)))
                    //{

                    //    channel.QueueBind(queue: queueName, "direct-exchange", routingKey: item);
                    //}

                    ////Benim Insance'ım tek seferde kaç tane mesaj alacağını aşağıdaki gibi ayarlayabiliri.

                    //channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    ////Yukarıdaki ifade şu demek: Ben her defasında 1 tane mesaj alırım onu işledikten sonra diğerini alırım. False koyduğumuz global parametreside şu demek: Benim tek instancem belirlediğim kadar mesaj alır. Global true yapsaydım varsayalım benim bu instancem'den 5 tane vardı ve ben 10 a çektim mesaj sayımı tüm instanceler totalda 1 defada 10 messaj alabilirdi.(Yani her instance 2 şer mesaj alırdı).

                    //var consumer = new EventingBasicConsumer(channel);

                    ////Bir diğer önemi parametremiz autoAck (Otomatik bilgilendirme: yani ben bu mesajı işledim bilgisini otomatik olarak MQ ya bildiriyor. Ben bunu false yaparsam manuel olarak kontrol ettirmem gerekmektedir).

                    ////channel.BasicConsume("test_queue",autoAck: false, consumer);

                    ////Direct Exchange
                    //channel.BasicConsume(queueName, false, consumer);

                    //Console.WriteLine("Critical ve Error logralrını bekliyorum...");

                    ////Publisher tarafından gönderilemn messajları Recived event'ı ile dinlemeye başladık.
                    //consumer.Received += (model, ea) =>
                    //{
                    //    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    //    Console.WriteLine($"LogMessage---{message}");

                    //    //Alttaki ifade ben mesajı halletim bana yenisini gönderebilirsin mesajını MQ'ya iletir.
                    //    channel.BasicAck(ea.DeliveryTag, multiple: false); //Bu mesajı MQ(brocker) alamazsa mesajı silmez. İşlenemedi demektir.
                    //};

                    //Console.WriteLine("Çıkmak İçin Bir Tuşa Basınız...");
                    //Console.ReadLine();
                    #endregion

                    #region Topic Exchange Consumer
                    ////channel.QueueDeclare("test_queue", durable: true, false, false, null);

                    ////Exchange: Topic 
                    //channel.ExchangeDeclare("topic-exchange", type: ExchangeType.Topic, durable: true);

                    ////Ben Topic olarak exchangemi belirledim ve bir publisher oluşturdum ancak bu publisheri spesifik routeKey ile queue ye bir consumer bind etmem gerekli.

                    //var queueName = channel.QueueDeclare().QueueName; //Rondom bir queue name üretir.

                    ////var routingKey = "Info.*.Warning"; //Başı Info ortası önemli değil sonuda Warning olanları dile.
                    ////var routingKey = "#.Warning"; //Sonu .Warning olanları dinle.
                    ////var routingKey = "*.Error.*"; //Başta ve sonda ne olursa olsun ortada Error olanları dinle.
                    //var routingKey = "Warning.#"; //Warning ile başlayanları dinle.

                    //channel.QueueBind(queue: queueName, "topic-exchange", routingKey: routingKey);


                    ////Benim Insance'ım tek seferde kaç tane mesaj alacağını aşağıdaki gibi ayarlayabiliri.

                    //channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    ////Yukarıdaki ifade şu demek: Ben her defasında 1 tane mesaj alırım onu işledikten sonra diğerini alırım. False koyduğumuz global parametreside şu demek: Benim tek instancem belirlediğim kadar mesaj alır. Global true yapsaydım varsayalım benim bu instancem'den 5 tane vardı ve ben 10 a çektim mesaj sayımı tüm instanceler totalda 1 defada 10 messaj alabilirdi.(Yani her instance 2 şer mesaj alırdı).

                    //var consumer = new EventingBasicConsumer(channel);

                    ////Bir diğer önemi parametremiz autoAck (Otomatik bilgilendirme: yani ben bu mesajı işledim bilgisini otomatik olarak MQ ya bildiriyor. Ben bunu false yaparsam manuel olarak kontrol ettirmem gerekmektedir).

                    ////channel.BasicConsume("test_queue",autoAck: false, consumer);

                    ////Direct Exchange
                    //channel.BasicConsume(queueName, false, consumer);

                    //Console.WriteLine("Logralrını bekliyorum...");

                    ////Publisher tarafından gönderilemn messajları Recived event'ı ile dinlemeye başladık.
                    //consumer.Received += (model, ea) =>
                    //{
                    //    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    //    Console.WriteLine($"LogMessage---{message}");

                    //    //Alttaki ifade ben mesajı halletim bana yenisini gönderebilirsin mesajını MQ'ya iletir.
                    //    channel.BasicAck(ea.DeliveryTag, multiple: false); //Bu mesajı MQ(brocker) alamazsa mesajı silmez. İşlenemedi demektir.
                    //};

                    //Console.WriteLine("Çıkmak İçin Bir Tuşa Basınız...");
                    //Console.ReadLine();
                    #endregion

                    #region Header Exchange Consumer
                    ////channel.QueueDeclare("test_queue", durable: true, false, false, null);

                    ////Exchange: Header 
                    //channel.ExchangeDeclare("header-exchange", type: ExchangeType.Headers,durable: true);

                    ////Ben Header olarak exchangemi belirledim ve bir publisher oluşturdum ancak bu publisheri spesifik header'ı ile queue ye bir consumer bind etmem gerekli.

                    //channel.QueueDeclare("Kuyruk_1", false, false, false, null);

                    ////Publisher tarafında oluşturduğum key-value (dictionary) çiftlerimi  buraya taşıyorum.
                    //Dictionary<string, object> headers = new Dictionary<string, object>();
                    //headers.Add("status", true);
                    //headers.Add("format", "Image");
                    ////Önemli Not: ben oluşturduğum headers'a eğer "x-match"-"all" şeklinde bir key-value çifti atarsam bu durumda benim bu tanımladığım tüm headers (key-value çifteri) ile eşleşen publisher'ı dinler. Eğer "x-match"-"any" şeklinde bir key-value çifti atarsam bu durm da da herhangi bir headers (key-value çifteri) ile eşleşebiliyor ise direkt dinler hepsinin uyuşmasına bakmaz. Consume tarafında tanımlanmalıdır.
                    //headers.Add("x-match", "all"); // Ben burada hepsinin eşleşmesini bekliyorum.

                    //channel.QueueBind("Kuyruk_1", "header-exchange", String.Empty,headers);


                    ////Benim Insance'ım tek seferde kaç tane mesaj alacağını aşağıdaki gibi ayarlayabiliri.

                    ////channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    ////Yukarıdaki ifade şu demek: Ben her defasında 1 tane mesaj alırım onu işledikten sonra diğerini alırım. False koyduğumuz global parametreside şu demek: Benim tek instancem belirlediğim kadar mesaj alır. Global true yapsaydım varsayalım benim bu instancem'den 5 tane vardı ve ben 10 a çektim mesaj sayımı tüm instanceler totalda 1 defada 10 messaj alabilirdi.(Yani her instance 2 şer mesaj alırdı).

                    //var consumer = new EventingBasicConsumer(channel);

                    ////Bir diğer önemi parametremiz autoAck (Otomatik bilgilendirme: yani ben bu mesajı işledim bilgisini otomatik olarak MQ ya bildiriyor. Ben bunu false yaparsam manuel olarak kontrol ettirmem gerekmektedir).

                    ////channel.BasicConsume("test_queue",autoAck: false, consumer);

                    ////Direct Exchange
                    //channel.BasicConsume("Kuyruk_1", false, consumer);

                    //Console.WriteLine("Mesajınız  bekliyorum...");

                    ////Publisher tarafından gönderilemn messajları Recived event'ı ile dinlemeye başladık.
                    //consumer.Received += (model, ea) =>
                    //{
                    //    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    //    Console.WriteLine($"Gelen Mesaj---{message}");

                    //    //Alttaki ifade ben mesajı halletim bana yenisini gönderebilirsin mesajını MQ'ya iletir.
                    //    channel.BasicAck(ea.DeliveryTag, multiple: false); //Bu mesajı MQ(brocker) alamazsa mesajı silmez. İşlenemedi demektir.
                    //};

                    //Console.ReadLine();
                    #endregion

                    //***RabbitMQ CompexType ile Çalışma***//

                    channel.QueueDeclare("Compex_Type", true, false, false, null);
                  

                    var consumer = new EventingBasicConsumer(channel);
                    channel.BasicConsume("Compex_Type", true, consumer);

                    consumer.Received += (model, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        var compexType = JsonSerializer.Deserialize<List<Test>>(message);

                        foreach (var item in compexType)
                        {
                            Console.WriteLine($"Id: {item.Id} Name: {item.Name} LastName: {item.LastName} Address: {item.Address}");
                        }
                    };


                }
                Console.ReadLine();

            }
        }
    }

    public enum LogNames
    {
        Critical,
        Error
    }
}
