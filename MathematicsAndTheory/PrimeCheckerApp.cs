using System;

namespace PrimeCheckerApp
{
    // Interface for prime-checking
    public interface IPrimeChecker
    {
        bool IsPrime(int number);
    }

    // Implements prime-checking logic
    public class PrimeChecker : IPrimeChecker
    {
        public bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            int limit = (int)Math.Floor(Math.Sqrt(number));
            for (int i = 3; i <= limit; i += 2)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }
    }

    // Encapsulates number input and validation
    public class NumberInput
    {
        private int _value;

        public int Value
        {
            get => _value;
            private set
            {
                if (value < 100 || value > 999)
                    throw new ArgumentOutOfRangeException("Number must be between 100 and 999.");
                _value = value;
            }
        }

        public void ReadFromConsole()
        {
            while (true)
            {
                try
                {
                    Console.Write("  Podaj liczbu (100-999): ");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    string? input = Console.ReadLine();
                    Console.ResetColor();

                    if (!int.TryParse(input, out int result))
                        throw new FormatException("Wprowadź poprawną liczbę.");

                    Value = result;
                    break;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"  Błąd: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }
    }

    // Handles presentation
    public static class UI
    {
        public static void ShowIntro()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  Witamy w programie sprawdzającym, czy liczba jest pierwsza i super pierwsza");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("  W tym programie możesz podać liczbu ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("od 100 do 999 ");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/");
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void ShowPrimeResult(int number, bool isPrime)
        {
            Console.Write("  Liczba ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(number);
            Console.ResetColor();

            if (isPrime)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(" jest liczbą pierwszą");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(" nie jest liczbą pierwszą");
            }

            Console.ResetColor();
        }

        public static void ShowSuperPrimeResult(bool isSuperPrime)
        {
            if (isSuperPrime)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("  I Super-pierwsza");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("  No ne super-pierwsza");
            }
            Console.ResetColor();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            UI.ShowIntro();

            var input = new NumberInput();
            input.ReadFromConsole();

            int number = input.Value;

            var checker = new PrimeChecker();
            bool isPrime = checker.IsPrime(number);

            UI.ShowPrimeResult(number, isPrime);
            if (!isPrime) return;

            // Digit sum check for "super-prime"
            int sum = SumOfDigits(number);
            bool isSuperPrime = checker.IsPrime(sum);
            UI.ShowSuperPrimeResult(isSuperPrime);
        }

        // Returns the sum of digits of the number
        private static int SumOfDigits(int number)
        {
            int sum = 0;
            while (number != 0)
            {
                sum += number % 10;
                number /= 10;
            }
            return sum;
        }
    }
}
