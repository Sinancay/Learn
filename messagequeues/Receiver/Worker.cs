using System;
using System.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract;
using System.Drawing;

namespace Receiver;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ConnectionFactory factory = new();
        factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
        factory.ClientProvidedName = "DataCaptureService Sender App";

        IConnection cnn = factory.CreateConnection();
        IModel channel = cnn.CreateModel();
        string queueName = "DataCaptureServiceQueue";

             var argsl = new Dictionary<string, object>
            {
                {"x-max-length", 1000000},
                {"x-overflow", "reject-publish"},
                {"x-max-length-bytes", 2001048576} 
            };   


        channel.QueueDeclare(queueName, true, false, false, argsl);
        channel.BasicQos(0, 1, false);
      
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, args) => {  // files is received here 
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            var body = args.Body;

              string fname = @"./ReceivedFolder/" + "ReceivedFile" + args.RoutingKey;
              System.IO.File.WriteAllBytes(fname, body.ToArray());
              channel.BasicAck(args.DeliveryTag, true);
              Console.WriteLine(args.RoutingKey + " File is downloaded in ReceivedFolder...");
            
        };

        string consumerTag = channel.BasicConsume(queueName, false, consumer);
        Console.ReadLine();

        channel.BasicCancel(consumerTag);

        channel.Close();
        cnn.Close();
    }
}
