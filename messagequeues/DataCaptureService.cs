
using System;
using System.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract;
using System.Drawing;

namespace messagequeues;

public class DataCapture
{
   public DataCapture()
   {
        ConnectionFactory factory = new();
        factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
        factory.ClientProvidedName = "DataCaptureService Sender App";

        IConnection cnn = factory.CreateConnection();
        IModel channel = cnn.CreateModel();

        string exchangeName = "DataCaptureService";
        string routingKey = "demo-key";
        string queueName = "DataCaptureServiceQueue";

        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        channel.QueueDeclare(queueName, false, false, false, null);
        channel.QueueBind(queueName, exchangeName, routingKey, null);

        byte[] imageBytes = File.ReadAllBytes("test.jpg");
        Console.WriteLine("Image Uploaded"); 
        channel.BasicPublish(exchangeName, routingKey, null, imageBytes);
        Thread.Sleep(1000);

      
        channel.Close();
        cnn.Close();
   }

    public void Start(){
        
    }


    public void Stop(){
        ConnectionFactory factory = new();
        factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
        factory.ClientProvidedName = "DataCaptureService Sender App";

        IConnection cnn = factory.CreateConnection();
        IModel channel = cnn.CreateModel();

        string exchangeName = "DataCaptureService";
        string routingKey = "demo-key";
        string queueName = "DataCaptureServiceQueue";

        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        channel.QueueDeclare(queueName, false, false, false, null);
        channel.QueueBind(queueName, exchangeName, routingKey, null);
        channel.BasicQos(0, 1, false);
      
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, args) => {
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            var body = args.Body.ToString();
            byte[] message = Encoding.ASCII.GetBytes(body);

           
            string fname = @"./" + "test" + ".jpg";
            System.IO.File.WriteAllBytes(fname, message);
            Console.WriteLine($"Image Received From Queue");
            channel.BasicAck(args.DeliveryTag, false);
        };
        string consumerTag = channel.BasicConsume(queueName, false, consumer);
        Console.ReadLine();

        channel.BasicCancel(consumerTag);

        channel.Close();
        cnn.Close();
    }
}


