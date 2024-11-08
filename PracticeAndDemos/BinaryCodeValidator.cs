using System;

namespace BinaryCodeValidation
{
    // Entry point
    internal class Program
    {
        static void Main(string[] args)
        {
            DisplayIntro();

            Console.Write("  Podaj kod: ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            string input = Console.ReadLine();
            Console.ResetColor();

            try
            {
                if (!InputValidator.IsValidInteger(input, out int code))
                {
                    ConsoleHelper.ShowError("Nieprawidłowy format liczby. Podaj liczbę całkowitą.");
                    return;
                }

                BinaryCode binaryCode = new BinaryCode(code);
                if (binaryCode.IsValid())
                {
                    ConsoleHelper.ShowSuccess("Poprawny kod!");
                }
                else
                {
                    ConsoleHelper.ShowError("Niepoprawny kod.");
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.ShowError($"Wystąpił błąd: {ex.Message}");
            }
        }

        static void DisplayIntro()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  Witamy w programie, aby sprawdzić, czy Twój kod jest odpowiedni, czy nie: ");
            Console.WriteLine();
            Console.ResetColor();
            Console.WriteLine("  1. Wartość bitu poprzedzającego najbardziej znaczący bit (pierwszy od lewej,\n  zawsze równy 1) musi być równa 0.");
            Console.WriteLine("  2. Suma wszystkich bitów musi być liczbą parzystą.");
            Console.WriteLine("  3. Liczba bitów nie może być mniejsza niż dwa i większa niż dziesięć.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/");
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    // Encapsulates and validates the binary code
    class BinaryCode
    {
        private readonly string _binaryString;

        public BinaryCode(int code)
        {
            _binaryString = Convert.ToString(code, 2);
        }

        public bool IsValid()
        {
            return IsLengthValid() && IsLeadingBitValid() && IsEvenBitSum();
        }

        private bool IsLengthValid()
        {
            return _binaryString.Length >= 2 && _binaryString.Length <= 10;
        }

        private bool IsLeadingBitValid()
        {
            // Ensure the bit before the most significant '1' is '0'
            // MSB is always '1', so check the next bit
            return _binaryString[_binaryString.Length - 2] == '0';
        }

        private bool IsEvenBitSum()
        {
            int sum = 0;
            foreach (char bit in _binaryString)
            {
                sum += bit - '0';
            }
            return sum % 2 == 0;
        }
    }

    // Handles common input validations
    static class InputValidator
    {
        public static bool IsValidInteger(string input, out int number)
        {
            return int.TryParse(input, out number) && number >= 0;
        }
    }

    // Provides reusable console UI methods
    static class ConsoleHelper
    {
        public static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"  {message}");
            Console.ResetColor();
        }

        public static void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"  {message}");
            Console.ResetColor();
        }
    }
}
