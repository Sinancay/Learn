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

public class ImageTransformationService
{
     public ImageTransformationService()
     {
            var ocrengine = new TesseractEngine(@".\tessdata", "eng", EngineMode.Default);
            var img = Pix.LoadFromFile("test.jpg");
            var res = ocrengine.Process(img);
            Console.WriteLine(res.GetText());
     }

     public void Start()
     {
     }

     public void Stop()
     {
     }
}
