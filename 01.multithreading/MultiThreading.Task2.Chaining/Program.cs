/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        const int TaskAmount = 4;
        public static List<int> taskList = new List<int>();

        static async Task Main(string[] args)
        {
            // Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            // Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            // Console.WriteLine("First Task – creates an array of 10 random integer.");
            // Console.WriteLine("Second Task – multiplies this array with another random integer.");
            // Console.WriteLine("Third Task – sorts this array by ascending.");
            // Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            // Console.WriteLine();

            // feel free to add your code
            Task task1 = new Task(FourTasks); // Worker Task first creates an array of 10 random integer.
            task1.Start();
            task1.Wait();

            for (int i = 0; i < TaskAmount; i++)
            {
              Task task2 = new Task(() => MultipliesOperation(i)); // Worker Task second multiplies this array with another random integer.
              task2.Start();
              task2.Wait();
            }
            
            
            Task task3 = new Task(() => SortArray()); // Worker Task third sort and print updated array.
            task3.Start();
            task3.Wait();

            Task task4 = new Task(() => AvarageArray()); // Worker Task fourth average array.
            task4.Start();
            task4.Wait();
            

            Console.ReadLine();
        }

        static void PrintArray() // Array Print Operation
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Updated Array Printed");
            foreach (var task in taskList)
            {
                Console.WriteLine(task);
            }
            Console.WriteLine("-------------------------------------");
        }

        static void MultipliesOperation(int taskIndex)
        {
            Random rndm = new Random();
            int rndmValue = rndm.Next(50);
            int oldValue = taskList[taskIndex];
            taskList[taskIndex] *= rndmValue;
            Console.WriteLine($"Array item {oldValue} Multiplies {rndmValue} = {taskList[taskIndex]}");
        }

        static void FourTasks()
        {
            // Its creates an array with rondom value
            Random rndm = new Random();
            for (int i = 0; i < TaskAmount; i++)
            {
                taskList.Add(rndm.Next(1, 10));
            }
            PrintArray();
        }

        static void SortArray()
        {
            taskList = taskList.OrderBy(i => i).ToList();
            PrintArray();
        }

        static void AvarageArray()
        {
            Console.WriteLine($"Array avarage {taskList.Average()}");
        }

    }
}
