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
            int[] fibonacciValues = new int[maxValue];

            Console.WriteLine("The even fibonacci values up to " + maxValue.ToString() + " include:");
            for (int loop = 0; loop <= maxValue; loop++)
            {
                int currentSequenceValue = 0;
                bool loopLessThanTwo = loop < 2;

                //If the loop index is less than 2, set the current fibonacci value equal to loop index.
                //If the loop index is more than or equal to 2, the current fibonnaci value is equal to the sum of the previous two sequence values.
                //Set the current index in the fibonacci values array to the current sequence value.
                if (loopLessThanTwo)
                {
                    currentSequenceValue = loop;
                    fibonacciValues[loop] = currentSequenceValue;
                }
                else
                {
                    currentSequenceValue = fibonacciValues[loop - 1] + fibonacciValues[loop - 2];
                    fibonacciValues[loop] = currentSequenceValue;
                }

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
            }
            return totalSum;
        }
        
        static int SumOfEvenFibonacciValuesWhile(int maxValue)
        {
            int totalSum = 0;
            int[] fibonacciValues = new int[maxValue];

            Console.WriteLine("The even fibonacci values up to " + maxValue.ToString() + " include:");
            bool isComplete = false;
            int loopIndex = 0;
            while(true){
                loopIndex++; 
            }


            for (int loop = 0; loop <= maxValue; loop++)
            {
                int currentSequenceValue = 0;
                bool loopLessThanTwo = loop < 2;

                //If the loop index is less than 2, set the current fibonacci value equal to loop index.
                //If the loop index is more than or equal to 2, the current fibonnaci value is equal to the sum of the previous two sequence values.
                //Set the current index in the fibonacci values array to the current sequence value.
                if (loopLessThanTwo)
                {
                    currentSequenceValue = loop;
                    fibonacciValues[loop] = currentSequenceValue;
                }
                else
                {
                    currentSequenceValue = fibonacciValues[loop - 1] + fibonacciValues[loop - 2];
                    fibonacciValues[loop] = currentSequenceValue;
                }

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

            Console.WriteLine("\n The sum of all the even values is " + evenFibonacciSum.ToString() + "\n");

            stopwatch.Stop();

            TimeSpan timeSpanValue = stopwatch.Elapsed;
            Console.WriteLine("Runtime for this algorithm: " + timeSpanValue.TotalMilliseconds.ToString() + "ms \n");
            Console.WriteLine("Runtime for this algorithm: " + timeSpanValue.TotalSeconds.ToString() + "seconds \n");
            stopwatch.Restart();


        }
    }

}
