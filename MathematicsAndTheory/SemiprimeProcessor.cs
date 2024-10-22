using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace SemiprimeApp
{
    // Handles all user input/output
    public class UserInterface
    {
        public void PrintWelcome()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Welcome to the Semiprime Number Finder!");
            Console.ResetColor();
        }

        public string AskFilePath()
        {
            Console.WriteLine("\nChoose file input option:\n1 - Default file (liczby.txt)\n2 - Custom path");
            Console.Write("Enter your choice: ");
            string input = Console.ReadLine();
            return input == "1" ? "liczby.txt" : GetCustomPath();
        }

        private string GetCustomPath()
        {
            Console.Write("\nEnter full path to your .txt file: ");
            return Console.ReadLine();
        }

        public int AskOutputOption()
        {
            Console.WriteLine("\nChoose output option:\n1 - Save to program folder\n2 - Save to custom path\n3 - Display on screen");
            Console.Write("Enter your choice: ");
            int.TryParse(Console.ReadLine(), out int choice);
            return choice;
        }

        public string GetOutputPath()
        {
            Console.Write("Enter output file path: ");
            return Console.ReadLine();
        }

        public void DisplayResults(List<int> results)
        {
            Console.WriteLine("\nSemiprimes:");
            for (int i = 0; i < results.Count; i++)
            {
                Console.Write($"{results[i]}, ");
                if ((i + 1) % 10 == 0) Console.WriteLine();
            }
            Console.WriteLine("\n\nPress any key to exit...");
            Console.ReadKey();
        }

        public void DisplayMessage(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    // Handles file read/write
    public class FileManager
    {
        public List<int> ReadNumbers(string path)
        {
            try
            {
                var lines = File.ReadAllLines(path);
                return lines.Select(int.Parse).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"File read error: {ex.Message}");
            }
        }

        public void WriteNumbers(string path, List<int> numbers)
        {
            File.WriteAllLines(path, numbers.Select(n => n.ToString()));
        }
    }

    // Checks if numbers are valid
    public class NumberValidator
    {
        public static List<int> ValidateNumbers(List<int> numbers)
        {
            return numbers.Where(n => n > 0).ToList();
        }
    }

    // Generates prime numbers
    public class PrimeCalculator
    {
        public List<int> GetPrimes(int limit)
        {
            var primes = new List<int>();
            for (int i = 2; i <= limit; i++)
            {
                if (IsPrime(i)) primes.Add(i);
            }
            return primes;
        }

        private bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            int sqrt = (int)Math.Sqrt(number);
            for (int i = 3; i <= sqrt; i += 2)
            {
                if (number % i == 0) return false;
            }
            return true;
        }
    }

    // Finds semiprime numbers
    public class SemiprimeFinder
    {
        public List<int> FindSemiprimes(List<int> input, List<int> primes)
        {
            var semiprimes = new HashSet<int>();
            int max = input.Max();

            // Try all pairs of primes and check if their product is in the input list
            foreach (var a in primes)
            {
                foreach (var b in primes)
                {
                    int product = a * b;
                    if (product > max) break;
                    if (input.Contains(product))
                    {
                        semiprimes.Add(product);
                    }
                }
            }

            return semiprimes.OrderBy(x => x).ToList();
        }
    }

    // Application entry point
    internal class Program
    {
        static void Main()
        {
            var ui = new UserInterface();
            var fileManager = new FileManager();
            var primeCalc = new PrimeCalculator();
            var semiprimeFinder = new SemiprimeFinder();

            ui.PrintWelcome();

            string filePath = ui.AskFilePath();
            List<int> numbers;
            try
            {
                numbers = fileManager.ReadNumbers(filePath);
                numbers = NumberValidator.ValidateNumbers(numbers);
            }
            catch (Exception ex)
            {
                ui.DisplayMessage(ex.Message, ConsoleColor.Red);
                return;
            }

            int max = numbers.Max() / 2;
            var primes = primeCalc.GetPrimes(max);
            var semiprimes = semiprimeFinder.FindSemiprimes(numbers, primes);

            int outputChoice = ui.AskOutputOption();
            switch (outputChoice)
            {
                case 1:
                    fileManager.WriteNumbers("semiprimes_output.txt", semiprimes);
                    ui.DisplayMessage("Results saved to semiprimes_output.txt", ConsoleColor.Green);
                    break;
                case 2:
                    string outPath = ui.GetOutputPath();
                    fileManager.WriteNumbers(outPath, semiprimes);
                    ui.DisplayMessage("Results saved successfully.", ConsoleColor.Green);
                    break;
                case 3:
                    ui.DisplayResults(semiprimes);
                    break;
                default:
                    ui.DisplayMessage("Invalid option selected.", ConsoleColor.Red);
                    break;
            }
        }
    }
}
