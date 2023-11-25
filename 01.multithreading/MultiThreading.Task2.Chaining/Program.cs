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

            // feel free to add your code
            await Task.Run( () =>  FourTasks()); // Worker Task first creates an array of 10 random integer.

            for (int i = 0; i < TaskAmount; i++)
            {
              await Task.Run( () =>  MultipliesOperation(i)); // Worker Task second multiplies this array with another random integer.
            }
            
            await Task.Run( () =>  SortArray()); // Worker Task third sort and print updated array.

            await Task.Run( () =>  AvarageArray()); // Worker Task fourth average array.    

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
