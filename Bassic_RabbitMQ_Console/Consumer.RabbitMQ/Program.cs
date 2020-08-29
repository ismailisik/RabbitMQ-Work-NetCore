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
                    channel.QueueDeclare("test_queue", durable: true, false, false, null);

                    //Benim Insance'ım tek seferde kaç tane mesaj alacağını aşağıdaki gibi ayarlayabiliri.

                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    //Yukarıdaki ifade şu demek: Ben her defasında 1 tane mesaj alırım onu işledikten sonra diğerini alırım. False koyduğumuz global parametreside şu demek: Benim tek instancem belirlediğim kadar mesaj alır. Global true yapsaydım varsayalım benim bu instancem'den 5 tane vardı ve ben 10 a çektim mesaj sayımı tüm instanceler totalda 1 defada 10 messaj alabilirdi.(Yani her instance 2 şer mesaj alırdı).

                    var consumer = new EventingBasicConsumer(channel);

                    //Bir diğer önemi parametremiz autoAck (Otomatik bilgilendirme: yani ben bu mesajı işledim bilgisini otomatik olarak MQ ya bildiriyor. Ben bunu false yaparsam manuel olarak kontrol ettirmem gerekmektedir).

                    channel.BasicConsume("test_queue",autoAck: false, consumer);

                    //Publisher tarafından gönderilemn messajları Recived event'ı ile dinlemeye başladık.
                    consumer.Received += (model, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        Console.WriteLine($"Mesajınız: {message}");

                        //Alttaki ifade ben mesajı halletim bana yenisini gönderebilirsin mesajını MQ'ya iletir.
                        channel.BasicAck(ea.DeliveryTag, multiple: false); //Bu mesajı MQ(brocker) alamazsa mesajı silmez. İşlenemedi demektir.
                    };

                    Console.WriteLine("Çıkmak İçin Bir Tuşa Basınız...");
                    Console.ReadLine();

                }

            }
        }
    }
}
