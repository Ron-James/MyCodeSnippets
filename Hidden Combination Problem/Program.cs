using System.Diagnostics;

///Written by Ron James Theron
namespace RonJamesTheronProgram
{
    // A data structure I desinged to store 2 integers. It is used in the problem 2 algorythm.
    class IntegerPair
    {
        private int _first = 0;
        private int _second = 0;

        public int First { get => _first; set => _first = value; }
        public int Second { get => _second; set => _second = value; }

        public IntegerPair(int first, int seccond)
        {
            this.First = first;
            this.Second = seccond;
        }

        public new string ToString()
        {
            return First.ToString() + ", " + Second.ToString();

        }
        /// <returns>Returns the sum of the first and second integers in the pair.</returns>
        public int Sum()
        {
            return First + Second;
        }
        /// <returns>Returns the product of the first and second integers in the pair.</returns>
        public int Product()
        {
            return First * Second;
        }
        


    }
    class Program
    {

        #region Problem 2 Functions
        /// <summary>
        ///  Prints the number combinatation whose sum is equal to the targetSum if it can be found in the input array. 
        ///  Also prints the product of the two integers in the integer pair.
        /// </summary>
        /// <param name="arr">The set of integers you want to search through.</param>
        /// <param name="data">Temporary array to store indices of each integer pair.</param>
        /// <param name="targetSum">The sum you are looking for in the integer set.</param>
        /// <param name="start">The starting index of the integer set.</param>
        /// <param name="end">the ending index of the integer set.</param>
        /// <param name="lastPair">an integer pair variable to store the most recent combination.</param>

        static void FindTargetSumInArrayUtility(int[] data, int[] arr, int targetSum,
                int index, int start, int end, ref IntegerPair lastPair)
        {


            // Since index has become 2, current combination is
            // ready to be checked and will be printed if the sum is equal to targetSum.
            // Data array stores each of the two values in currentIntegerPair variable. 
            if (index == 2)
            {

                IntegerPair currentIntegerPair = new IntegerPair(data[0], data[1]);
                lastPair = currentIntegerPair;
                bool sumEqalToTargetSum = lastPair.Sum() == targetSum;

                //Uncomment to see each possible integer combination listed printed out before the target pair is found
                //Console.WriteLine(currentIntegerPair.ToString() + " \n");
                if (sumEqalToTargetSum)
                {
                    Console.WriteLine(lastPair.First.ToString() + " and " + lastPair.Second.ToString() + " combine to form " + targetSum.ToString());
                    Console.WriteLine("The product of these two integers is " + lastPair.Product().ToString() + "\n");

                }
                return;
            }

            // Replace data at position index with all possible items in the input array.
            // The condition "end-i+1 >= 2-index" ensures each element will form a combination 
            // with the remainig items.
            // Place the array value at position i in the data array at position index.
            // Each time checks if the last number pair is equal to the targetSum parameter.
            // If the sum is equal to targetSum break the loop and the operation is complete,
            // else recur, adding one to the index value which started at 0.
            for (int i = start; i <= end && end - i + 1 >= 2 - index; i++)
            {

                data[index] = arr[i];

                if (lastPair.Sum() == targetSum)
                {
                    return;
                }
                else
                {
                    FindTargetSumInArrayUtility(data, arr, targetSum, index + 1, i + 1, end, ref lastPair);
                }

            }

            return;
        }


        /// <summary>
        ///  Calls the recursive function FindTargetSumInArrayUtility(). 
        ///  Use this to generate all possible number combinations of size 2 within a set of integers until the target sum is found.
        /// </summary>
        /// <param name="arr">The integer set which you want to search through.</param>
        /// <param name="targetSum">The target sum you want to find in the array.</param>
        /// <param name="n">The length of the integer set (arr).</param>
        /// <param name="lastPair">A integer pair object to store each possible combination of integers within the array.</param>
        public static void FindTargetSumInArray(int[] arr, int targetSum, int n, ref IntegerPair lastPair)
        {
            // The main function that generates all combinations of size 2
            // in arr[] of size n with no repetitions.
            Array.Sort(arr);
            // Allocate memory
            int[] data = new int[3];
            // Call the recursive function
            FindTargetSumInArrayUtility(data, arr, targetSum, 0, 0, n - 1, ref lastPair);

        }

        /// <summary>
        ///  Generates an integer array from the lines in a given text file.
        /// </summary>
        /// <param name="path">The local path of the file you want to read.</param>
        /// <returns>An integer array made up of the lines in the given text file.</returns>
        static int[] ReadIntegerLinesInFile(String path)
        {
            try
            {
                int counter = 0;
                List<int> numbers = new List<int>();
                // Read the file and display it line by line.  
                foreach (string line in System.IO.File.ReadLines(path))
                {
                    //System.Console.WriteLine(line);
                    int num = Convert.ToInt32(line);
                    numbers.Add(num);
                    counter++;
                }
                return numbers.ToArray();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found at " + path);
                throw;
            }


        }
        #endregion


        static void Main(string[] args)
        {
            // The stopwatch is used to measure runtime of the routine.
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //Problem 2
            Console.WriteLine("Hidden Combination Problem \n");

            //Generate an integer array using the path to the text file.
            string path = @"problem2.txt";
            int[] arr = ReadIntegerLinesInFile(path);

            //Set the target sum, in this case its 2424.
            int targetSum = 2424;
            IntegerPair lastPair = new IntegerPair(0, 0);

            //call the algorithm which prints out the pair of integers if found and stores the values is lastPair.
            FindTargetSumInArray(arr, targetSum, arr.Length, ref lastPair);

            //If the integer pair lastPair does not sum to the target sum, then it does not exist in the file.
            bool targetSumFound = lastPair.Sum() == targetSum;
            if (!targetSumFound)
            {
                Console.WriteLine(targetSum.ToString() + " not found in text file provided \n");
            }

            stopwatch.Stop();
            TimeSpan timeSpanValue = stopwatch.Elapsed;
            Console.WriteLine("Runtime for problem 2 algorythm: " + timeSpanValue.TotalMilliseconds.ToString() + "ms \n");


        }
    }

}
