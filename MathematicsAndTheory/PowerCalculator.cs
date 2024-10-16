using System;

namespace PowerApp
{
    // Service for power calculation
    public class PowerService
    {
        // Calculates x raised to power k recursively
        public double Calculate(double x, int k)
        {
            if (k == 0)
            {
                return 1;
            }

            if (k < 0)
            {
                return 1 / Calculate(x, -k);
            }

            if (k % 2 == 0)
            {
                double half = Calculate(x, k / 2);
                return half * half;
            }

            return x * Calculate(x, k - 1);
        }
    }

    // Handles input validation
    public static class InputValidator
    {
        // Validates if input is a valid double
        public static bool TryReadDouble(string prompt, out double value)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            return double.TryParse(input, out value);
        }

        // Validates if input is a valid int
        public static bool TryReadInt(string prompt, out int value)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            return int.TryParse(input, out value);
        }
    }

    // Entry point
    internal class ProgramEntry
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Power Calculator ===");

            if (!InputValidator.TryReadDouble("Enter base x: ", out double x))
            {
                Console.WriteLine("Error: Invalid input for x. Please enter a numeric value.");
                return;
            }

            if (!InputValidator.TryReadInt("Enter exponent k: ", out int k))
            {
                Console.WriteLine("Error: Invalid input for k. Please enter an integer.");
                return;
            }

            try
            {
                PowerService powerService = new PowerService();
                double result = powerService.Calculate(x, k);
                Console.WriteLine($"\nResult: {x}^{k} = {result}");
            }
            catch (Exception ex)
            {
                // Handles any unexpected error
                Console.WriteLine($"An error occurred during calculation: {ex.Message}");
            }
        }
    }
}
