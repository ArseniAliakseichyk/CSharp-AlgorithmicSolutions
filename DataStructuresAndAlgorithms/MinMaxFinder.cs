using System;

namespace DZ_C2
{
    // Class responsible for finding min and max using recursive divide and conquer
    public class MinMaxFinder
    {
        private int[] _numbers;

        public MinMaxFinder(int[] numbers)
        {
            Numbers = numbers;
        }

        // Encapsulated property with validation
        public int[] Numbers
        {
            get => _numbers;
            private set
            {
                if (value == null || value.Length == 0)
                    throw new ArgumentException("Array must not be empty.");
                _numbers = value;
            }
        }

        // Public method to get minimum number
        public int GetMin()
        {
            return FindMin(0, Numbers.Length - 1);
        }

        // Public method to get maximum number
        public int GetMax()
        {
            return FindMax(0, Numbers.Length - 1);
        }

        // Recursive method to find min in a range
        private int FindMin(int left, int right)
        {
            if (left == right)
                return Numbers[left];

            int mid = (left + right) / 2;
            int minLeft = FindMin(left, mid);
            int minRight = FindMin(mid + 1, right);

            return (minLeft < minRight) ? minLeft : minRight;
        }

        // Recursive method to find max in a range
        private int FindMax(int left, int right)
        {
            if (left == right)
                return Numbers[left];

            int mid = (left + right) / 2;
            int maxLeft = FindMax(left, mid);
            int maxRight = FindMax(mid + 1, right);

            return (maxLeft > maxRight) ? maxLeft : maxRight;
        }
    }

    // Class responsible for user interaction and input validation
    public class ConsoleHandler
    {
        public void Run()
        {
            Console.WriteLine("Welcome! This program finds min and max from a series of numbers.");
            Console.WriteLine("**************");
            Console.WriteLine("Choose option 1 or 2:");
            Console.WriteLine("1. Use predefined series (3, 2, 0, 9, 6, 2, 1, 8).");
            Console.WriteLine("2. Enter your own series.");
            Console.WriteLine("**************");
            Console.Write("Enter 1 or 2: ");

            string input = Console.ReadLine();

            try
            {
                if (!int.TryParse(input, out int choice) || (choice != 1 && choice != 2))
                {
                    Console.WriteLine("Invalid option. Program will exit.");
                    return;
                }

                int[] numbers = choice == 1 ? GetPredefinedNumbers() : GetUserNumbers();

                var finder = new MinMaxFinder(numbers);
                Console.Clear();
                DisplayNumbers(numbers);

                Console.WriteLine("****************");
                Console.WriteLine($"Minimum element: {finder.GetMin()}");
                Console.WriteLine($"Maximum element: {finder.GetMax()}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        private int[] GetPredefinedNumbers()
        {
            return new int[] { 3, 2, 0, 9, 6, 2, 1, 8 };
        }

        private int[] GetUserNumbers()
        {
            Console.Write("How many numbers will you enter? ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
                throw new ArgumentException("Number of elements must be a positive integer.");

            int[] arr = new int[n];
            for (int i = 0; i < n; i++)
            {
                Console.Write($"Enter value {i + 1}: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int val))
                {
                    throw new ArgumentException($"Invalid integer input at position {i + 1}.");
                }
                arr[i] = val;
            }
            return arr;
        }

        // Display the series in readable format
        private void DisplayNumbers(int[] numbers)
        {
            Console.Write("Your number series: (");
            for (int i = 0; i < numbers.Length; i++)
            {
                if (i > 0)
                    Console.Write(", ");
                Console.Write(numbers[i]);
            }
            Console.WriteLine(")");
        }
    }

    internal class Program
    {
        static void Main()
        {
            var consoleHandler = new ConsoleHandler();
            consoleHandler.Run();
        }
    }
}
