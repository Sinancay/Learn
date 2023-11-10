using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AsyncAwait.Task1.CancellationTokens;

static class Calculator
{
    // todo: change this method to support cancellation token
     public static async Task<int> Calculate(int n , CancellationToken token)
    {
        
        int sum = 0;

        token.Register(async () => { await CancelError(n); });

        for (var i = 0; i < n; i++)
        {
            if (token.IsCancellationRequested){
                break;
            }

            // i + 1 is to allow 2147483647 (Max(Int32)) 
            sum = sum + (i + 1);
            await Task.Delay(10, token);
        }

        return sum;
    }

    public static async Task CancelError(int n)
    {
        Console.WriteLine($"Sum for {n} cancelled...");
    }
}
