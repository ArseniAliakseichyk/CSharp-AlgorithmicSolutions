using System;

namespace ArmstrongCheckerApp
{
    // Handles Armstrong number logic
    public class ArmstrongNumberChecker
    {
        public bool IsArmstrong(ulong number)
        {
            ulong original = number;
            int digits = number.ToString().Length;
            ulong sum = 0;

            while (number > 0)
            {
                ulong digit = number % 10;
                sum += (ulong)Math.Pow(digit, digits);
                number /= 10;
            }

            return sum == original;
        }
    }

    // Handles array generation and filtering
    public class ArmstrongProcessor
    {
        private readonly ArmstrongNumberChecker _checker;

        public ArmstrongProcessor()
        {
            _checker = new ArmstrongNumberChecker();
        }

        public ulong[] GetArmstrongNumbers(ulong size)
        {
            if (size == 0)
                return Array.Empty<ulong>();

            ulong[] results = new ulong[size];
            int index = 0;

            for (ulong i = 1; i < size; i++)
            {
                ulong current = i + 152;
                if (_checker.IsArmstrong(current))
                {
                    results[index++] = current;
                }
            }

            // Return only valid entries
            Array.Resize(ref results, index);
            return results;
        }
    }

    // Handles all user interaction and validation
    public static class UserInterface
    {
        public static ulong GetArraySize()
        {
            while (true)
            {
                Console.Write("  Podaj rozmiar tablicy: ");
                string? input = Console.ReadLine();

                if (ulong.TryParse(input, out ulong result) && result >= 1)
                {
                    return result;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  [Error] Wprowadź poprawną liczbę całkowitą większą lub równą 1.");
                Console.ResetColor();
            }
        }

        public static void ShowWelcomeMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  Witamy w programie do liczenia liczb Armstronga");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("  W tym programie możesz podać rozmiar tablicy ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("od 1 i więcej ");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("            Warning! Warning! Warning! Warning!");
            Console.ResetColor();
            Console.WriteLine("  If tab > 1000000 to Obliczenie programu zajmie dużo czasu");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/");
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void ShowResults(ulong[] numbers, ulong size)
        {
            Console.WriteLine("\n");
            Console.WriteLine($"  Liczby Armstronga w przejściu od 153 do {size + 152} : ");
            Console.Write("  ");

            for (int i = 0; i < numbers.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(numbers[i]);
                Console.ResetColor();

                Console.Write(i < numbers.Length - 1 ? ", " : ".");
            }

            Console.WriteLine();
        }
    }

    internal class Program
    {
        static void Main()
        {
            UserInterface.ShowWelcomeMessage();

            ulong size = UserInterface.GetArraySize();

            try
            {
                var processor = new ArmstrongProcessor();
                var armstrongNumbers = processor.GetArmstrongNumbers(size);
                UserInterface.ShowResults(armstrongNumbers, size);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  [Unhandled Error] {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
