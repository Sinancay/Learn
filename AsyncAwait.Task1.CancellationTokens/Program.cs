/*
* Study the code of this application to calculate the sum of integers from 0 to N, and then
* change the application code so that the following requirements are met:
* 1. The calculation must be performed asynchronously.
* 2. N is set by the user from the console. The user has the right to make a new boundary in the calculation process,
* which should lead to the restart of the calculation.
* 3. When restarting the calculation, the application should continue working without any failures.
*/

using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal class Program
{
    /// <summary>
    /// The Main method should not be changed at all.
    /// </summary>
    /// <param name="args"></param>
    public static CancellationTokenSource source = new CancellationTokenSource();
    public static CancellationToken token = source.Token;
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Mentoring program L2. Async/await.V1. Task 1");
        Console.WriteLine("Calculating the sum of integers from 0 to N.");
        Console.WriteLine("Use 'q' key to exit...");
        Console.WriteLine("Use 'c' key to cancel token...");
        Console.WriteLine();

        Console.WriteLine("Enter N: ");

        var input = Console.ReadLine();
        while (input.Trim().ToUpper() != "Q")
        {
            if (int.TryParse(input, out var n))
            {
                await CalculateSum(n);
            }
            else if(input.Trim().ToUpper() == "C")
            {
                // todo: add code to process cancellation and uncomment this line    
                // Console.WriteLine($"Sum for {n} cancelled...");
                source.Cancel(); 
                token.ThrowIfCancellationRequested();
                Console.WriteLine($"Sum for {n} cancelled...");
            }
            else
            {
                Console.WriteLine($"Invalid integer: '{input}'. Please try again.");
                Console.WriteLine("Enter N: ");
            }

            input = Console.ReadLine();
        }

        Console.WriteLine("Press any key to continue");
        Console.ReadLine();
    }
 
    private static async Task CalculateSum(int n)
    {
        // todo: make calculation asynchronous
        var sum = await Calculator.Calculate(n, token);
        Console.WriteLine($"Sum for {n} = {sum}.");
        Console.WriteLine();
        Console.WriteLine("Enter N: ");

        Console.WriteLine($"The task for {n} started... Enter N to cancel the request:");
    }
}
