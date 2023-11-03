/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Threading; 
using System.Collections.Generic; 

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static List<int> collections = new List<int>();
        const int limit = 10; 
        static void Main(string[] args)
        {
            // Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            // Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            // Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            // Console.WriteLine();

            // feel free to add your code

                Thread thread = new Thread(AddMethod);
                thread.Start();

            
           

            Console.ReadLine();
        }
        static void AddMethod()  
        {  
             lock (collections)
            {
                    for (int i = 0; i < limit; i++)
                    {
                        collections.Add(i);
                        Thread thread = new Thread(PrintMethod); // same second call print worker thread
                        thread.Start();
                        Thread.Sleep(1000);
                    }
                }
        }  
        static void PrintMethod()  
        {  
             lock (collections)
            {
                foreach (var item in collections)
                {
                    Console.WriteLine(item);
                }
                Thread.Sleep(1000);
            }
        }  
    }
}
