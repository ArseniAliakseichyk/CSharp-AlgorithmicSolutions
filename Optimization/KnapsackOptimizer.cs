using System;
using System.Collections.Generic;

namespace KnapsackApp
{
    // Represents a single item with weight and value
    public class Item
    {
        public int Weight { get; private set; }
        public int Value { get; private set; }

        public Item(int value, int weight)
        {
            if (value <= 0 || weight <= 0)
            {
                throw new ArgumentException("Value and weight must be positive integers.");
            }

            Value = value;
            Weight = weight;
        }
    }

    // Handles all input and validation
    public static class InputHandler
    {
        public static int ReadPositiveInt(string prompt)
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                string? input = Console.ReadLine();
                Console.ResetColor();

                if (int.TryParse(input, out result) && result > 0)
                {
                    return result;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Invalid input. Please enter a positive integer.");
                Console.ResetColor();
            }
        }

        public static List<Item> ReadItems(int count)
        {
            var items = new List<Item>();

            Console.WriteLine("\n  ---Enter item values---");
            var values = new int[count];

            for (int i = 0; i < count; i++)
            {
                values[i] = ReadPositiveInt($"  Enter value for item {i + 1}: ");
            }

            Console.WriteLine("\n  ---Enter item weights---");

            for (int i = 0; i < count; i++)
            {
                int weight = ReadPositiveInt($"  Enter weight for item {i + 1}: ");
                items.Add(new Item(values[i], weight));
            }

            return items;
        }
    }

    // Solves the unbounded knapsack problem
    public class KnapsackSolver
    {
        private readonly List<Item> _items;
        private readonly int _capacity;
        private readonly int[] _dp;
        private readonly int[] _selectedItem;

        public KnapsackSolver(List<Item> items, int capacity)
        {
            _items = items;
            _capacity = capacity;
            _dp = new int[capacity + 1];
            _selectedItem = new int[capacity + 1];
        }

        public void Solve()
        {
            // Dynamic programming: unbounded knapsack
            for (int j = 1; j <= _capacity; j++)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    Item item = _items[i];
                    if (j >= item.Weight && _dp[j] < _dp[j - item.Weight] + item.Value)
                    {
                        _dp[j] = _dp[j - item.Weight] + item.Value;
                        _selectedItem[j] = i + 1;
                    }
                }
            }
        }

        public void DisplayResult()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n\n  ---Result---");
            Console.ResetColor();
            Console.Write("  Maximum value in the knapsack: ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(_dp[_capacity]);
            Console.ResetColor();

            Console.WriteLine("\n  Items in the knapsack:");
            int weightRemaining = _capacity;
            int totalWeight = 0;

            while (weightRemaining > 0 && _selectedItem[weightRemaining] != 0)
            {
                int itemIndex = _selectedItem[weightRemaining] - 1;
                Item item = _items[itemIndex];
                int count = 0;

                while (weightRemaining >= item.Weight && _selectedItem[weightRemaining] - 1 == itemIndex)
                {
                    weightRemaining -= item.Weight;
                    count++;
                }

                int totalItemWeight = count * item.Weight;
                int totalItemValue = count * item.Value;
                totalWeight += totalItemWeight;

                Console.WriteLine($"  Item {itemIndex + 1} | Weight: {item.Weight} | Value: {item.Value} | Quantity: {count} | Total Weight: {totalItemWeight} | Total Value: {totalItemValue}");
            }

            Console.Write("\n  Total weight of items in knapsack: ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(totalWeight);
            Console.ResetColor();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
                Console.ResetColor();
                Console.WriteLine("  Program to solve the unbounded knapsack problem.");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/");
                Console.ResetColor();

                int itemCount = InputHandler.ReadPositiveInt("\n  Enter number of items: ");
                List<Item> items = InputHandler.ReadItems(itemCount);
                int capacity = InputHandler.ReadPositiveInt("\n  Enter knapsack capacity: ");

                var solver = new KnapsackSolver(items, capacity);
                solver.Solve();
                solver.DisplayResult();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  An error occurred: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
