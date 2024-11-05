using System;
using System.Collections.Generic;

namespace OptimalBusAllocator
{
    // Represents a single bus type
    public class Bus
    {
        public string Name { get; }
        public int Capacity { get; }
        public int Cost { get; }

        public Bus(string name, int capacity, int cost)
        {
            Name = name;
            Capacity = capacity;
            Cost = cost;
        }
    }

    // Validates user input
    public static class Validator
    {
        public static bool TryParseStudentCount(string input, out int count)
        {
            return int.TryParse(input, out count) && count > 0;
        }
    }

    // Allocates buses to minimize cost
    public class BusAllocator
    {
        private readonly List<Bus> _busTypes;

        public BusAllocator()
        {
            // Sorted by descending capacity to prioritize larger buses
            _busTypes = new List<Bus>
            {
                new Bus("C", 32, 900),
                new Bus("B", 18, 550),
                new Bus("A", 9, 300)
            };
        }

        public Dictionary<string, int> Allocate(int students)
        {
            Dictionary<string, int> allocation = new();
            int remaining = students;

            foreach (var bus in _busTypes)
            {
                allocation[bus.Name] = remaining / bus.Capacity;
                remaining %= bus.Capacity;
            }

            // Adjust last two A buses to one B if it’s more optimal
            if (allocation["A"] >= 2)
            {
                allocation["A"] -= 2;
                allocation["B"] += 1;
            }

            return allocation;
        }

        public int CalculateTotalCost(Dictionary<string, int> allocation)
        {
            int total = 0;
            foreach (var bus in _busTypes)
            {
                total += allocation[bus.Name] * bus.Cost;
            }
            return total;
        }
    }

    // Main user interface
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  Welcome to the optimal bus allocation program.");
            Console.ResetColor();
            Console.WriteLine("\n  Enter the number of students\n");
            Console.WriteLine("  C – 32 students, 900 zł");
            Console.WriteLine("  B – 18 students, 550 zł");
            Console.WriteLine("  A – 9 students, 300 zł");
            Console.WriteLine("\n  The result will show the most cost-efficient solution.");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/");
            Console.ResetColor();

            int studentCount = 0;
            while (true)
            {
                Console.Write("\n  Enter number of students: ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                string input = Console.ReadLine();
                Console.ResetColor();

                if (Validator.TryParseStudentCount(input, out studentCount))
                {
                    break;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Invalid input. Please enter a positive integer.");
                Console.ResetColor();
            }

            BusAllocator allocator = new();
            Dictionary<string, int> result = allocator.Allocate(studentCount);
            int totalCost = allocator.CalculateTotalCost(result);

            Console.WriteLine("\n\n  Allocation:");
            foreach (var pair in result)
            {
                Console.WriteLine($"  {pair.Key}: x{pair.Value}");
            }

            Console.WriteLine($"  Total Cost: {totalCost} zł");
        }
    }
}
