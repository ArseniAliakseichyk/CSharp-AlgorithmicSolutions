using System;
using System.Collections.Generic;
using System.Linq;

namespace MergeCostApp
{
    // Interface for merge strategies
    public interface IMergeStrategy
    {
        int Calculate(int[] numbers);
    }

    // Class for tracking operation statistics
    public class MergeStats
    {
        public int Swaps { get; private set; }
        public int Merges { get; private set; }

        public void IncrementSwaps() => Swaps++;
        public void IncrementMerges() => Merges++;
        public void Reset()
        {
            Swaps = 0;
            Merges = 0;
        }
    }

    // Strategy for calculating minimal merge cost
    public class MinMergeStrategy : IMergeStrategy
    {
        private readonly MergeStats _stats;

        public MinMergeStrategy(MergeStats stats)
        {
            _stats = stats;
        }

        public int Calculate(int[] numbers)
        {
            Array.Sort(numbers); // Bubble sort alternative
            return CalculateMerge(numbers);
        }

        private int CalculateMerge(int[] arr)
        {
            if (arr.Length == 1) return 0;

            BubbleSort(arr, ascending: true);

            int sum = arr[0] + arr[1];
            _stats.IncrementMerges();

            int[] next = new int[arr.Length - 1];
            next[0] = sum;
            Array.Copy(arr, 2, next, 1, arr.Length - 2);

            return sum + CalculateMerge(next);
        }

        private void BubbleSort(int[] arr, bool ascending)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    bool condition = ascending ? arr[j] > arr[j + 1] : arr[j] < arr[j + 1];
                    if (condition)
                    {
                        (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                        _stats.IncrementSwaps();
                    }
                }
            }
        }
    }

    // Strategy for calculating maximal merge cost
    public class MaxMergeStrategy : IMergeStrategy
    {
        private readonly MergeStats _stats;

        public MaxMergeStrategy(MergeStats stats)
        {
            _stats = stats;
        }

        public int Calculate(int[] numbers)
        {
            Array.Sort(numbers, (a, b) => b.CompareTo(a));
            return CalculateMerge(numbers);
        }

        private int CalculateMerge(int[] arr)
        {
            if (arr.Length == 1) return 0;

            BubbleSort(arr, ascending: false);

            int sum = arr[0] + arr[1];
            _stats.IncrementMerges();

            int[] next = new int[arr.Length - 1];
            next[0] = sum;
            Array.Copy(arr, 2, next, 1, arr.Length - 2);

            return sum + CalculateMerge(next);
        }

        private void BubbleSort(int[] arr, bool ascending)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    bool condition = ascending ? arr[j] > arr[j + 1] : arr[j] < arr[j + 1];
                    if (condition)
                    {
                        (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                        _stats.IncrementSwaps();
                    }
                }
            }
        }
    }

    // Class for validating and parsing user input
    public static class Validator
    {
        public static bool TryParseInput(string input, out int[] numbers, out string error)
        {
            error = "";
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            List<int> result = new List<int>();

            foreach (var part in parts)
            {
                if (int.TryParse(part, out int num))
                {
                    result.Add(num);
                }
                else
                {
                    error = $"Invalid number format: '{part}'";
                    numbers = null;
                    return false;
                }
            }

            if (result.Count < 2)
            {
                error = "At least two numbers are required.";
                numbers = null;
                return false;
            }

            numbers = result.ToArray();
            return true;
        }
    }

    // Handles all console input/output
    public static class InputHandler
    {
        public static int[] GetUserNumbers()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("  Podaj liczby: ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                string input = Console.ReadLine();
                Console.ResetColor();

                if (Validator.TryParseInput(input, out var numbers, out var error))
                {
                    return numbers;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Błąd: " + error);
                Console.ResetColor();
            }
        }

        public static void ShowIntro()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  Witamy w programie ustalającym minimalny i maksymalny \n  koszt łączenia podanego przez użytkownika ciągu liczb.");
            Console.ResetColor();
            Console.WriteLine("\n  Przykład: Podaj liczby: 3 2 1 5 4");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/");
            Console.ResetColor();
        }
    }

    // Main orchestrator class
    public class MergeCalculator
    {
        private readonly MergeStats _stats = new MergeStats();

        public void Run()
        {
            InputHandler.ShowIntro();

            int[] inputNumbers = InputHandler.GetUserNumbers();

            var minCalc = new MinMergeStrategy(_stats);
            int minCost = minCalc.Calculate((int[])inputNumbers.Clone());

            Console.WriteLine($"\n  Minimalny koszt sklejenia: {minCost}");
            Console.WriteLine($"  Wyszukiwania: {_stats.Swaps}, Sklejenia: {_stats.Merges}");

            _stats.Reset();

            var maxCalc = new MaxMergeStrategy(_stats);
            int maxCost = maxCalc.Calculate((int[])inputNumbers.Clone());

            Console.WriteLine($"\n  Maksymalny koszt sklejenia: {maxCost}");
            Console.WriteLine($"  Wyszukiwania: {_stats.Swaps}, Sklejenia: {_stats.Merges}");

            Console.WriteLine("\n  Aby zakończyć program, naciśnij dowolny przycisk...");
            Console.ReadKey();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var calc = new MergeCalculator();
            calc.Run();
        }
    }
}
