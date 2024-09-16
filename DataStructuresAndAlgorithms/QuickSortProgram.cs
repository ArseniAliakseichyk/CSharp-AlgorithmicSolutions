using System;
using System.Collections.Generic;

namespace C_2
{
    // Main app class to run program workflow
    internal class QuickSortProgram
    {
        private readonly INumberProvider _numberProvider;
        private readonly IArraySorter _sorter;
        private readonly IOutputHandler _outputHandler;

        public QuickSortProgram(INumberProvider numberProvider, IArraySorter sorter, IOutputHandler outputHandler)
        {
            _numberProvider = numberProvider;
            _sorter = sorter;
            _outputHandler = outputHandler;
        }

        public void Run()
        {
            try
            {
                _outputHandler.ShowMenu();

                int choice = _numberProvider.ReadChoice();

                int[] numbers;

                switch (choice)
                {
                    case 1:
                        numbers = _numberProvider.GenerateUniqueNumbers(10000, 0, 10000);
                        _outputHandler.DisplayArray(numbers, "Generated array:");
                        _outputHandler.PromptContinue("Press any key to sort...");
                        break;

                    case 2:
                        numbers = _numberProvider.ReadUserNumbers();
                        _outputHandler.DisplayArray(numbers, "Your input array:");
                        break;

                    default:
                        _outputHandler.ShowError("Invalid choice! Program will exit.");
                        return;
                }

                int[] sorted = _sorter.Sort(numbers);

                _outputHandler.DisplayArray(sorted, "Sorted array:");
                _outputHandler.ShowMessage("Program finished.");
            }
            catch (Exception ex)
            {
                _outputHandler.ShowError($"Error: {ex.Message}");
            }
        }
    }

    // Interface for providing numbers (input/generation)
    interface INumberProvider
    {
        int ReadChoice();
        int[] GenerateUniqueNumbers(int count, int min, int max);
        int[] ReadUserNumbers();
    }

    // Implementation for number input and generation
    class NumberProvider : INumberProvider
    {
        private readonly IOutputHandler _outputHandler;

        public NumberProvider(IOutputHandler outputHandler)
        {
            _outputHandler = outputHandler;
        }

        public int ReadChoice()
        {
            while (true)
            {
                _outputHandler.Write("Enter 1 or 2: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int choice) && (choice == 1 || choice == 2))
                {
                    return choice;
                }
                _outputHandler.ShowError("Invalid input. Please enter 1 or 2.");
            }
        }

        public int[] GenerateUniqueNumbers(int count, int min, int max)
        {
            if (count < 1 || max < min || max - min + 1 < count)
                throw new ArgumentException("Invalid range or count for unique number generation.");

            var uniqueNumbers = new HashSet<int>();
            var rnd = new Random();
            int[] result = new int[count];

            int i = 0;
            while (uniqueNumbers.Count < count)
            {
                int num = rnd.Next(min, max + 1);
                if (uniqueNumbers.Add(num))
                {
                    result[i++] = num;
                }
            }

            return result;
        }

        public int[] ReadUserNumbers()
        {
            int n = 0;
            while (true)
            {
                _outputHandler.Write("How many numbers will you enter? ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out n) && n > 0)
                    break;

                _outputHandler.ShowError("Please enter a positive integer.");
            }

            int[] numbers = new int[n];
            for (int i = 0; i < n; i++)
            {
                while (true)
                {
                    _outputHandler.Write($"Enter number {i + 1}: ");
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out int value))
                    {
                        numbers[i] = value;
                        break;
                    }
                    _outputHandler.ShowError("Invalid number. Please enter a valid integer.");
                }
            }
            return numbers;
        }
    }

    // Interface for sorting algorithms
    interface IArraySorter
    {
        int[] Sort(int[] array);
    }

    // QuickSort implementation
    class QuickSorter : IArraySorter
    {
        public int[] Sort(int[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            QuickSort(array, 0, array.Length - 1);
            return array;
        }

        private void QuickSort(int[] arr, int low, int high)
        {
            if (low < high)
            {
                int pivotIndex = Partition(arr, low, high);
                QuickSort(arr, low, pivotIndex);
                QuickSort(arr, pivotIndex + 1, high);
            }
        }

        private int Partition(int[] arr, int low, int high)
        {
            int pivot = arr[(low + high) / 2];
            int i = low - 1;
            int j = high + 1;

            while (true)
            {
                do { i++; } while (arr[i] < pivot);
                do { j--; } while (arr[j] > pivot);

                if (i >= j) return j;

                (arr[i], arr[j]) = (arr[j], arr[i]);
            }
        }
    }

    // Interface for all console outputs
    interface IOutputHandler
    {
        void ShowMenu();
        void DisplayArray(int[] arr, string title);
        void ShowMessage(string message);
        void ShowError(string message);
        void Write(string message);
        void PromptContinue(string message);
    }

    // Implementation of console output
    class ConsoleOutputHandler : IOutputHandler
    {
        public void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("This is a QuickSort program");
            Console.WriteLine("**************");
            Console.WriteLine("Choose an option (1 or 2):");
            Console.WriteLine("1. Generate 10,000 random numbers.");
            Console.WriteLine("2. Enter a series of numbers yourself.");
            Console.WriteLine("**************");
            Console.ResetColor();
        }

        public void DisplayArray(int[] arr, string title)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(title);
            Console.ResetColor();

            for (int i = 0; i < arr.Length; i++)
            {
                if (i > 0 && i % 20 == 0) Console.WriteLine();

                // Formatting numbers for alignment
                string formatted = arr[i] switch
                {
                    < 10 => $"{arr[i],3}, ",
                    < 100 => $"{arr[i],4}, ",
                    < 1000 => $"{arr[i],5}, ",
                    _ => $"{arr[i],6}, "
                };

                Console.Write(formatted);
            }
            Console.WriteLine();
        }

        public void ShowMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Write(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(message);
            Console.ResetColor();
        }

        public void PromptContinue(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.ReadKey(true);
        }
    }

    // Program entry point
    internal class Program
    {
        static void Main()
        {
            var outputHandler = new ConsoleOutputHandler();
            var numberProvider = new NumberProvider(outputHandler);
            var sorter = new QuickSorter();

            var app = new QuickSortProgram(numberProvider, sorter, outputHandler);
            app.Run();
        }
    }
}
