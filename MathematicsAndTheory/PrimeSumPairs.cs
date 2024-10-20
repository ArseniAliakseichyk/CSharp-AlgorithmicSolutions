using System;
using System.Collections.Generic;

namespace PrimeSumPairsApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter a positive number: ");
            string? input = Console.ReadLine();

            try
            {
                // Validate user input
                int number = InputValidator.ValidatePositiveInteger(input);

                // Find prime pairs
                var finder = new PrimePairFinder(new PrimeNumberService());
                List<(int, int)> pairs = finder.FindPrimeSumPairs(number);

                // Output result
                if (pairs.Count == 0)
                {
                    Console.WriteLine($"No prime pairs found that sum to {number}.");
                }
                else
                {
                    Console.WriteLine($"Prime pairs that sum to {number}:");
                    foreach (var pair in pairs)
                    {
                        Console.WriteLine($"{number} = {pair.Item1} + {pair.Item2}");
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }
    }

    // Validates input data
    public static class InputValidator
    {
        public static int ValidatePositiveInteger(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be empty.");
            }

            if (!int.TryParse(input, out int result))
            {
                throw new ArgumentException("Input must be a valid integer.");
            }

            if (result <= 1)
            {
                throw new ArgumentException("Number must be greater than 1.");
            }

            return result;
        }
    }

    // Service to calculate primes
    public class PrimeNumberService
    {
        // Returns all primes up to a given limit using Sieve of Eratosthenes
        public List<int> GetPrimesUpTo(int max)
        {
            var isPrime = new bool[max + 1];
            for (int i = 2; i <= max; i++) { isPrime[i] = true; }

            for (int i = 2; i * i <= max; i++)
            {
                if (isPrime[i])
                {
                    for (int j = i * i; j <= max; j += i)
                    {
                        isPrime[j] = false;
                    }
                }
            }

            var primes = new List<int>();
            for (int i = 2; i <= max; i++)
            {
                if (isPrime[i])
                {
                    primes.Add(i);
                }
            }

            return primes;
        }
    }

    // Responsible for finding pairs of primes that sum to a target
    public class PrimePairFinder
    {
        private readonly PrimeNumberService _primeService;

        public PrimePairFinder(PrimeNumberService primeService)
        {
            _primeService = primeService;
        }

        public List<(int, int)> FindPrimeSumPairs(int target)
        {
            List<int> primes = _primeService.GetPrimesUpTo(target);
            var pairs = new List<(int, int)>();

            // Check combinations (i, j) where i <= j and i + j == target
            for (int i = 0; i < primes.Count; i++)
            {
                for (int j = i; j < primes.Count; j++)
                {
                    if (primes[i] + primes[j] == target)
                    {
                        pairs.Add((primes[i], primes[j]));
                    }
                }
            }

            return pairs;
        }
    }
}
