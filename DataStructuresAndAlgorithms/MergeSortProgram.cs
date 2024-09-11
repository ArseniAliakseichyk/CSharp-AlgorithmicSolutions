using System;
using System.Collections.Generic;

namespace SortingApp
{
    /// <summary>
    /// Entry point and UI interaction handler
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleUI ui = new ConsoleUI();
            ui.Run();
        }
    }

    /// <summary>
    /// Handles user interaction and program flow
    /// </summary>
    public class ConsoleUI
    {
        private readonly IMergeSorter _sorter;

        public ConsoleUI()
        {
            _sorter = new MergeSorter();
        }

        public void Run()
        {
            try
            {
                ShowWelcomeMessage();
                int choice = ReadUserChoice();

                switch (choice)
                {
                    case 1:
                        HandleAutoGeneration();
                        break;
                    case 2:
                        HandleUserInput();
                        break;
                    default:
                        ShowError("Invalid option selected. Program will exit.");
                        break;
                }
            }
            catch (FormatException)
            {
                ShowError("Input was not a valid integer.");
            }
            catch (Exception ex)
            {
                ShowError($"Unexpected error: {ex.Message}");
            }
        }

        private void ShowWelcomeMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("This is a Merge Sort program.");
            Console.WriteLine("**************");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Auto-generate 10,000 unique numbers.");
            Console.WriteLine("2. Enter your own series of numbers.");
            Console.WriteLine("**************");
            Console.Write("Enter 1 or 2: ");
            Console.ResetColor();
        }

        private int ReadUserChoice()
        {
            string input = Console.ReadLine();
            if (!int.TryParse(input, out int choice) || (choice != 1 && choice != 2))
            {
                throw new FormatException();
            }
            Console.Clear();
            return choice;
        }

        private void HandleAutoGeneration()
        {
            var generator = new UniqueRandomGenerator(10000, 0, 10000);
            int[] numbers = generator.Generate();

            DisplayNumbers(numbers, "Generated array:");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\nPress any key to sort the array...");
            Console.ResetColor();
            Console.ReadKey();

            numbers = _sorter.Sort(numbers);

            DisplayNumbers(numbers, "Sorted array:");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\nSorting complete.");
            Console.ResetColor();
        }

        private void HandleUserInput()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Enter the number of elements: ");
            Console.ResetColor();

            if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
            {
                ShowError("Please enter a positive integer for count.");
                return;
            }

            var inputHandler = new UserInputHandler(count);
            int[] numbers = inputHandler.ReadNumbers();

            Console.Clear();
            DisplayNumbers(numbers, "Your input array:");

            numbers = _sorter.Sort(numbers);

            DisplayNumbers(numbers, "Sorted array:");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\nSorting complete.");
            Console.ResetColor();
        }

        private void DisplayNumbers(int[] numbers, string title)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(title);
            Console.ResetColor();

            for (int i = 0; i < numbers.Length; i++)
            {
                if (i % 20 == 0)
                {
                    Console.WriteLine();
                }

                // Formatting numbers for aligned display
                Console.Write($"{numbers[i],4}, ");
            }
            Console.WriteLine();
        }

        private void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Generates unique random integers within a range
    /// </summary>
    public class UniqueRandomGenerator
    {
        private readonly int _count;
        private readonly int _minValue;
        private readonly int _maxValue;
        private readonly Random _random;

        public UniqueRandomGenerator(int count, int minValue, int maxValue)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count), "Count must be positive.");
            if (minValue > maxValue) throw new ArgumentException("minValue must be <= maxValue.");
            if (count > maxValue - minValue + 1) throw new ArgumentException("Count exceeds the number of unique values in range.");

            _count = count;
            _minValue = minValue;
            _maxValue = maxValue;
            _random = new Random();
        }

        public int[] Generate()
        {
            HashSet<int> uniqueNumbers = new();
            int[] result = new int[_count];

            while (uniqueNumbers.Count < _count)
            {
                int num = _random.Next(_minValue, _maxValue + 1);
                if (uniqueNumbers.Add(num))
                {
                    result[uniqueNumbers.Count - 1] = num;
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Reads and validates user input numbers
    /// </summary>
    public class UserInputHandler
    {
        private readonly int _count;

        public UserInputHandler(int count)
        {
            _count = count;
        }

        public int[] ReadNumbers()
        {
            int[] numbers = new int[_count];
            for (int i = 0; i < _count; i++)
            {
                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write($"Enter value {i + 1}: ");
                    Console.ResetColor();

                    string input = Console.ReadLine();
                    if (int.TryParse(input, out int number))
                    {
                        numbers[i] = number;
                        break;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invalid input. Please enter a valid integer.");
                        Console.ResetColor();
                    }
                }
            }
            return numbers;
        }
    }

    /// <summary>
    /// Interface for sorting algorithms
    /// </summary>
    public interface IMergeSorter
    {
        int[] Sort(int[] array);
    }

    /// <summary>
    /// Implements the Merge Sort algorithm
    /// </summary>
    public class MergeSorter : IMergeSorter
    {
        public int[] Sort(int[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Length <= 1) return array;

            int[] aux = new int[array.Length];
            MergeSort(array, aux, 0, array.Length - 1);
            return array;
        }

        // Recursive merge sort implementation
        private void MergeSort(int[] arr, int[] aux, int left, int right)
        {
            if (left >= right) return;

            int mid = left + (right - left) / 2;
            MergeSort(arr, aux, left, mid);
            MergeSort(arr, aux, mid + 1, right);
            Merge(arr, aux, left, mid, right);
        }

        // Merges two sorted halves of the array
        private void Merge(int[] arr, int[] aux, int left, int mid, int right)
        {
            int i = left;
            int j = mid + 1;
            int k = left;

            while (i <= mid && j <= right)
            {
                if (arr[i] <= arr[j])
                {
                    aux[k++] = arr[i++];
                }
                else
                {
                    aux[k++] = arr[j++];
                }
            }

            while (i <= mid)
            {
                aux[k++] = arr[i++];
            }

            while (j <= right)
            {
                aux[k++] = arr[j++];
            }

            for (int idx = left; idx <= right; idx++)
            {
                arr[idx] = aux[idx];
            }
        }
    }
}
