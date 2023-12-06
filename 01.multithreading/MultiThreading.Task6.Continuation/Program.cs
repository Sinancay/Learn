/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        const int TaskAmount = 10;
        static void Main(string[] args)
        {
            // Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            // Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            // Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            // Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            // Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            // Console.WriteLine("Demonstrate the work of the each case with console utility.");
            // Console.WriteLine();

            // feel free to add your code
            
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;
                TaskFactory factory = new TaskFactory(token);
                Task[] subTasks = new Task[TaskAmount];

                      var parent = factory.StartNew(() => {
                            Console.WriteLine("Outer task executing.");
                            for (int i = 0; i < TaskAmount; i++)
                            {
                                Console.WriteLine($"Task # {i} PROCESS START");
                                subTasks[i] = factory.StartNew(() => {
                                DoWork(i);
                                Thread.SpinWait(500000);
                                });
                            }
                            
                        });

                    parent.Wait();
                    Console.WriteLine("Parent has completed.");
               


                factory.ContinueWhenAll(subTasks, completedTasks => {
                                                        Console.WriteLine("Completed All Sub Tasks");
                                                    }); //Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation

              if (true) {  //Cancellation progress here If you close this if statement you will see Completed All Sub Tasks message in console
                    source.Cancel();
                    Console.WriteLine("Cancelling at task ");
              }   

            Console.ReadLine();
        }

        static void DoWork(int taskCount) {
            Thread.Sleep(3000);
            Console.WriteLine($"TASK PROCESSING COMPLETED");
         }
              
    }
}
