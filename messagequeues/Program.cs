using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Topshelf;

namespace messagequeues;

class Program
{
    static void Main(string[] args)
    {

        var exitCode = HostFactory.Run(x => 
        {
            x.Service<ImageTransformation>(s => {
                s.ConstructUsing(imageTransformationService => new ImageTransformation());
                s.WhenStarted(imageTransformationService => imageTransformationService.Start());
                s.WhenStopped(imageTransformationService => imageTransformationService.Stop());
            });

            x.RunAsLocalSystem();
            x.SetServiceName("imageTransformationService");
            x.SetDisplayName("image Transformation Service");
            x.SetDescription("DataCaptureService");
        });

        int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
        Environment.ExitCode = exitCodeValue;

        // For other Service

        // var exitCode = HostFactory.Run(x => 
        // {
        //     x.Service<ImageTransformationService>(s => {
        //         s.ConstructUsing(imageTransformationService => new ImageTransformationService());
        //         s.WhenStarted(imageTransformationService => imageTransformationService.Start());
        //         s.WhenStopped(imageTransformationService => imageTransformationService.Stop());
        //     });

        //     x.RunAsLocalSystem();
        //     x.SetServiceName("imageTransformationService");
        //     x.SetDisplayName("image Transformation Service");
        //     x.SetDescription("imageTransformationService");
        // });

        // int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
        // Environment.ExitCode = exitCodeValue;


    }
}