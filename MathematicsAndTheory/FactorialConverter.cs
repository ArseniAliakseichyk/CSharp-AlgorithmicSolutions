using System;
using System.Numerics;
using System.Text.RegularExpressions;

namespace FactorialConverterApp
{
    // Represents conversion modes
    enum InputMode
    {
        Decimal = 1,
        FactorialBase = 2
    }

    // Class responsible for factorial calculations
    public static class Factorial
    {
        // Calculates factorial of n recursively
        public static BigInteger Calculate(int n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException(nameof(n), "Factorial input cannot be negative.");

            if (n == 0)
                return 1;

            return n * Calculate(n - 1);
        }
    }

    // Handles validation logic
    public static class Validator
    {
        // Validates factorial base number string (only digits allowed)
        public static bool IsValidFactorialBaseNumber(string input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }

        // Validates if input string is a valid integer in decimal format
        public static bool IsValidDecimalNumber(string input)
        {
            return BigInteger.TryParse(input, out BigInteger result) && result >= 0;
        }

        // Validates input mode choice
        public static bool IsValidModeChoice(string input, out InputMode mode)
        {
            mode = 0;
            if (int.TryParse(input, out int choice) && (choice == 1 || choice == 2))
            {
                mode = (InputMode)choice;
                return true;
            }
            return false;
        }
    }

    // Converts factorial base strings to decimal numbers
    public class FactorialToDecimalConverter
    {
        private readonly string factorialNumber;

        public FactorialToDecimalConverter(string factorialNumber)
        {
            if (string.IsNullOrWhiteSpace(factorialNumber))
                throw new ArgumentException("Input cannot be empty.");

            if (!Validator.IsValidFactorialBaseNumber(factorialNumber))
                throw new ArgumentException("Input contains invalid characters. Only digits are allowed.");

            this.factorialNumber = factorialNumber;
        }

        public BigInteger Convert()
        {
            BigInteger sum = 0;
            int length = factorialNumber.Length;

            // Sum each digit multiplied by factorial of position (1-based)
            for (int i = 0; i < length; i++)
            {
                int digit = factorialNumber[i] - '0'; // convert char to int
                int position = length - i; // factorial index starts from right
                sum += digit * Factorial.Calculate(position);
            }

            return sum;
        }
    }

    // Converts decimal numbers to factorial base strings
    public class DecimalToFactorialConverter
    {
        private readonly BigInteger decimalNumber;

        public DecimalToFactorialConverter(BigInteger decimalNumber)
        {
            if (decimalNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(decimalNumber), "Number must be non-negative.");

            this.decimalNumber = decimalNumber;
        }

        public string Convert()
        {
            if (decimalNumber == 0)
                return "0";

            BigInteger remaining = decimalNumber;
            int factorialIndex = 1;

            // Find the largest factorial index where factorial is <= decimalNumber
            while (Factorial.Calculate(factorialIndex) <= remaining)
                factorialIndex++;

            factorialIndex--;

            var digits = new BigInteger[factorialIndex + 1];
            int idx = 0;

            // Convert decimal to factorial base representation
            while (factorialIndex >= 0)
            {
                BigInteger factorialValue = Factorial.Calculate(factorialIndex);
                digits[idx] = remaining / factorialValue;
                remaining %= factorialValue;
                factorialIndex--;
                idx++;
            }

            // Convert digits array to string
            return string.Concat(Array.ConvertAll(digits, d => d.ToString()));
        }
    }

    // Main application class handling user interaction
    internal class Program
    {
        static void Main()
        {
            try
            {
                ShowWelcomeMessage();

                InputMode mode = GetUserInputMode();

                if (mode == InputMode.FactorialBase)
                {
                    string factorialInput = Prompt("Enter factorial base number: ");

                    var factorialConverter = new FactorialToDecimalConverter(factorialInput);
                    BigInteger decimalResult = factorialConverter.Convert();

                    Console.WriteLine($"Factorial base number {factorialInput} is {decimalResult} in decimal.");
                }
                else // Decimal input mode
                {
                    string decimalInput = Prompt("Enter decimal number: ");

                    if (!Validator.IsValidDecimalNumber(decimalInput))
                    {
                        Console.WriteLine("Invalid decimal number input.");
                        return;
                    }

                    BigInteger decimalValue = BigInteger.Parse(decimalInput);

                    var decimalConverter = new DecimalToFactorialConverter(decimalValue);
                    string factorialResult = decimalConverter.Convert();

                    Console.WriteLine($"Decimal number {decimalValue} is {factorialResult} in factorial base.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ShowWelcomeMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(" Welcome to the factorial base <-> decimal converter program.");
            Console.WriteLine(" Choose input number system:");
            Console.WriteLine(" 1. Decimal system");
            Console.WriteLine(" 2. Factorial base system");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/");
            Console.ResetColor();
            Console.WriteLine();
        }

        private static InputMode GetUserInputMode()
        {
            Console.Write("Choose input system (1 or 2): ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            string input = Console.ReadLine();
            Console.ResetColor();

            if (!Validator.IsValidModeChoice(input, out InputMode mode))
            {
                throw new ArgumentException("Invalid selection. Please enter 1 or 2.");
            }

            return mode;
        }

        private static string Prompt(string message)
        {
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            string input = Console.ReadLine();
            Console.ResetColor();
            return input;
        }
    }
}
