using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Zhaoxi.Helper
{
    /// <summary>
    /// RabbitMQ 操作类
    /// </summary>
    public class RabbitMQProductionConsumer
    {
        /// <summary>
        /// 创建多个生产者
        /// </summary>
        /// <param name="num">第几个生产者</param>
        public static void CreateMutiProducer(int num = 1)
        {
            var factory = new ConnectionFactory();
            factory.HostName = "http://localhost";
            factory.UserName = "guest";
            factory.Password = "guest";
            using (var connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "MutiProducerMessage", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.ExchangeDeclare(exchange: "MutiProducerMessageExchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                    channel.QueueBind(queue: "MutiProducerMessage", exchange: "MutiProducerMessageExchange", routingKey: string.Empty, arguments: null);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("生产者001准备就绪");
                    int i = 1;
                    while (true)
                    {
                        //多个生产者，这里的生产者01就是生产者n
                        string message = $"生产者{num} 消息{i}";
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "MutiProducerMessageExchange", routingKey: string.Empty, basicProperties: null, body: body);
                        Console.WriteLine($"消息{message}已发送");
                        i++;
                        Thread.Sleep(200);
                    }
                }
            }
        }

        /// <summary>
        /// 生产者接收到请求时Received事件
        /// </summary>
        public static void ReveicedProducerEvent()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "http://localhost";
            factory.UserName = "guest";
            factory.Password = "guest";
            using (var connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    try
                    {
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            //如果有消息需要处理，就触发这个事件
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body.ToArray());
                            Console.WriteLine($"消费者01接受的消息是{message}");
                        };
                        channel.BasicConsume(queue: "OnlyProducerMessage", autoAck: true, consumer: consumer);
                        Console.WriteLine($"Press [enter] to exit");
                        Console.ReadLine();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
        }

        /// <summary>
        /// 创建单个生产者
        /// </summary>
        public static void CreateSingleProducer()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "http://localhost";
            factory.UserName = "guest";
            factory.Password = "guest";
            using (var connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "OnlyProducerMessage", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.ExchangeDeclare(exchange: "OnlyProducerMessageExchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                    channel.QueueBind(queue: "OnlyProducerMessage", exchange: "OnlyProducerMessageExchange", routingKey: string.Empty, arguments: null);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("生产者ProductionDemo准备就绪");


                    int i = 1;
                    while (true)
                    {
                        string message = $"消息{i}";
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "OnlyProducerMessageExchange", routingKey: string.Empty, basicProperties: null, body: body);
                        Console.WriteLine($"消息{message}已发送");
                        i++;
                        Thread.Sleep(200);
                    }

                }
            }
        }
    }
}
