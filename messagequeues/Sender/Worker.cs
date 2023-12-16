using System;
using System.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract;
using System.Drawing;
using System.Text.Json;
using Newtonsoft.Json;

namespace Sender;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

     public class WeatherForecast
    {
        public string? Data { get; set; }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ConnectionFactory factory = new();
        factory.Uri = new Uri("amqp://guest:guest@localhost:5672"); 
        factory.ClientProvidedName = "DataCaptureService Sender App";

        IConnection cnn = factory.CreateConnection();
        IModel channel = cnn.CreateModel();

        string exchangeName = "DataCaptureServiceQueue-ex";
        string queueName = "DataCaptureServiceQueue";
         
        var argsl = new Dictionary<string, object>
            {
                {"x-max-length", 1000000},
                {"x-overflow", "reject-publish"},
                {"x-max-length-bytes", 2001048576}
            };

                    
        channel.ExchangeDeclare(exchangeName, "direct");
        channel.QueueDeclare(queueName, true, false, false, argsl);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

        for(int i=0; i<3; i++){  //pdf and jpg files send to queue here
            if(i == 0){
                 string routingKey = ".jpg";
                 channel.QueueBind(queueName, exchangeName, routingKey, null);
                 byte[] fileBytes = File.ReadAllBytes("test.jpg");
                 long length = new System.IO.FileInfo("test.jpg").Length;
                 await checkSize(length);
                 Console.WriteLine("Image is Upload Queue");
                 channel.BasicPublish(exchangeName, routingKey, basicProperties: properties, fileBytes);
            }else if(i == 1){
                 string routingKey = ".pdf";
                 channel.QueueBind(queueName, exchangeName, routingKey, null);
                 byte[] fileBytes = File.ReadAllBytes("test.pdf");
                 long length = new System.IO.FileInfo("test.pdf").Length;
                 await checkSize(length);
                 Console.WriteLine("PDF is Upload Queue");
                 channel.BasicPublish(exchangeName, routingKey, basicProperties: properties, fileBytes);
            }else{

                 string routingKey = ".mov";
                 channel.QueueBind(queueName, exchangeName, routingKey, null);
                 byte[] fileBytes = File.ReadAllBytes("test.mov");
                 String file = Convert.ToBase64String(fileBytes);
                 long length = new System.IO.FileInfo("test.mov").Length;
                 await checkSize(length);
                 Console.WriteLine("MOV File is uploaded");
                channel.BasicPublish(exchangeName, routingKey, basicProperties: properties, fileBytes);
            }
            Thread.Sleep(5000);
        }
      
        channel.Close();
        cnn.Close();
    }

    public static async Task checkSize(long size){
        if(size > 500000000) {
            Console.WriteLine("File size is not accaptable");
            throw new Exception();
        }else{
            Console.WriteLine("File size is acceptable " + size.ToString());
        }   
    }
}
