/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    class Program
    {
        const int TaskAmount = 100;
        const int MaxIterationsCount = 1000;
        public static List<int> taskList = new List<int>();

        static void Main(string[] args)
        {
            // Console.WriteLine(".Net Mentoring Program. Multi threading V1.");
            // Console.WriteLine("1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.");
            // Console.WriteLine("Each Task should iterate from 1 to 1000 and print into the console the following string:");
            // Console.WriteLine("“Task #0 - {iteration number}”.");
            // Console.WriteLine();
            
            HundredTasks(); // Its completed all Tasks array. them are not finished

            foreach (var task in taskList)
            {
              //Thread thread = new Thread(() => CallOperation(task)); // Worker Threads proceed to each tasks to iteration
              Task taskWork = new Task(() => CallOperation(task));
              taskWork.Start();
            }
       
            Console.ReadLine();
        }

        static void HundredTasks()
        {
            // Its creates an array of 100 Tasks
            for (int i = 0; i < TaskAmount; i++)
            {
                taskList.Add(i + 1);
            }
        }

        static void CallOperation(int taskNumber)
        {
            for (int i = 0; i < MaxIterationsCount; i++)
            {
                Output(taskNumber, i + 1); // Calling Outputs
            }
        }

        static void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} - {iterationNumber}");
        }
    }
}
