using System;
using System.Diagnostics;
using System.Threading;

///Written by Ron James Theron
namespace RonJamesTheronProgram
{
    class Program
    {

        #region Problem 1 Functions
        /// <summary>
        ///  Adds up all the even values within the fibonacci sequence up to the maxValue provided.
        /// </summary>
        /// <param name="maxValue">The maximum value in the fibonacci sequence.</param>
        /// <returns>The sum of all the even values in the fibonacci sequence up to maxValue<./returns>
        
        static int SumOfEvenFibonacciValues(int maxValue)
        {
            int totalSum = 0;
            List<int> fibonacciValues = new List<int>();
            fibonacciValues.Add(0);
            fibonacciValues.Add(1);

            Console.WriteLine("The even fibonacci values up to " + maxValue.ToString() + " include:");
            int loopIndex = 2;
            while(true){
                //Calculate the current fibonacci sequence value and the current loop index
                int currentSequenceValue = fibonacciValues[loopIndex - 1] + fibonacciValues[loopIndex - 2];
                fibonacciValues.Add(currentSequenceValue);

                // If the current sequence value exceeds the maximum value inputed in the parameters,
                // break the loop as the operation is complete.
                bool exceedsMaxValue = currentSequenceValue >= maxValue;
                if (exceedsMaxValue)
                {
                    break;
                }

                //If the current sequence value is an even number, print it out and add it to the running total.
                bool isEven = currentSequenceValue % 2 == 0;
                if (isEven)
                {
                    Console.Write(currentSequenceValue.ToString() + " ");
                    totalSum += currentSequenceValue;
                }

                loopIndex++; 
            }
            return totalSum;
        }

        #endregion


        static void Main(string[] args)
        {
            // The stopwatch is used to measure runtime of the routine.
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //Problem 1
            Console.WriteLine("Problem 1 \n");

            //Take in the max fibonacci sequence value.
            int maxValue = 10000000; 
            Console.WriteLine("Input the maximum Fibonacci value:");
            maxValue = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("\n");


            //Generate the sum of all the even fibonacci values.
            int evenFibonacciSum = SumOfEvenFibonacciValues(maxValue);

            Console.WriteLine("\nThe sum of all the even values is " + evenFibonacciSum.ToString() + "\n");

            stopwatch.Stop();

            TimeSpan timeSpanValue = stopwatch.Elapsed;
            Console.WriteLine("Runtime for this algorithm: " + timeSpanValue.TotalMilliseconds.ToString() + "ms \n");
            stopwatch.Restart();


        }
    }

}
