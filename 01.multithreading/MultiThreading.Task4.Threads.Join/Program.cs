/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static Semaphore s = new Semaphore(1, 10);  // Available=1; Capacity=10

        static void Main(string[] args)
        {
            // Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            // Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            // Console.WriteLine("Implement all of the following options:");
            // Console.WriteLine();
            // Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            // Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();
            
            // feel free to add your code
            
            Thread t = new Thread(() => Worker(10));
            t.Name = "1";
            t.Start();

            Console.ReadLine();
        }

        private static void Worker(int count)
        {
                if (count == 0) {
                    Console.WriteLine("Last Thread its end");
                }else {
                        s.WaitOne();
                            Console.WriteLine("Thread " + count + " Processing");
                            count -= 1;
                            Thread t = new Thread(() => Worker(count));
                            t.Start();
                        s.Release();
                    try 
                    {
                        t.Join();
                    } 
                    catch (Exception e) 
                    {
                       Console.WriteLine("Well... That didn't go as planned!");
                    }
            }
        }
    }
}
