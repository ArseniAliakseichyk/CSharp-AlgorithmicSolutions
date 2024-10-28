using System;
using System.Collections.Generic;

namespace CashierAssistantApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UI.ShowWelcomeMessage();

            try
            {
                decimal amountDue = UI.ReadDecimal("  Kwotę do zapłaty: ");
                decimal amountReceived = UI.ReadDecimal("  Kwotę otrzymanych: ");

                Transaction transaction = new Transaction(amountDue, amountReceived);
                ChangeCalculator calculator = new ChangeCalculator();
                Dictionary<int, int> change = calculator.CalculateChange(transaction);

                UI.DisplayChange(transaction, change);
            }
            catch (FormatException)
            {
                UI.ShowError("  Błędny format danych. Wprowadź prawidłową liczbę.");
            }
            catch (ArgumentException ex)
            {
                UI.ShowError($"  Błąd: {ex.Message}");
            }
            catch (Exception)
            {
                UI.ShowError("  Wystąpił nieoczekiwany błąd.");
            }

            UI.ExitPrompt();
        }
    }

    // Entity representing a transaction
    public class Transaction
    {
        public decimal AmountDue { get; }
        public decimal AmountReceived { get; }

        public Transaction(decimal amountDue, decimal amountReceived)
        {
            if (amountDue < 0 || amountReceived < 0)
                throw new ArgumentException("Kwoty nie mogą być ujemne.");

            AmountDue = amountDue;
            AmountReceived = amountReceived;
        }

        public decimal Change => AmountReceived - AmountDue;
        public bool IsExact => Change == 0;
        public bool IsShort => Change < 0;
    }

    // Handles change logic
    public class ChangeCalculator
    {
        private readonly int[] _zlote = { 500, 200, 100, 50, 20, 10, 5, 2, 1 };
        private readonly int[] _grosze = { 50, 20, 10, 5, 2, 1 };

        public Dictionary<int, int> CalculateChange(Transaction transaction)
        {
            if (transaction.IsShort)
                throw new ArgumentException("Za mało pieniędzy do zapłacenia.");

            Dictionary<int, int> result = new();

            int zl = (int)transaction.Change;
            foreach (int denom in _zlote)
            {
                int count = zl / denom;
                if (count > 0)
                {
                    result[denom] = count;
                    zl %= denom;
                }
            }

            int gr = Convert.ToInt32((transaction.Change - (int)transaction.Change) * 100);
            foreach (int denom in _grosze)
            {
                int count = gr / denom;
                if (count > 0)
                {
                    result[denom] = count;
                    gr %= denom;
                }
            }

            return result;
        }
    }

    // Handles console UI
    public static class UI
    {
        public static void ShowWelcomeMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("          Witamy w programie Asystent kasjera");
            Console.ResetColor();
            Console.WriteLine("\n  Ten program pomoże wydać resztę używając jak najmniej monet\n");
            Console.WriteLine("  W pierwszym wierszu należy wpisać kwotę do zapłaty ");
            Console.WriteLine("  W drugim wierszu wpisz kwotę otrzymanych pieniędzy ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/\n");
            Console.ResetColor();
        }

        public static decimal ReadDecimal(string prompt)
        {
            Console.Write(prompt);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string? input = Console.ReadLine();
            Console.ResetColor();

            if (!decimal.TryParse(input, out decimal result))
                throw new FormatException();

            return result;
        }

        public static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void DisplayChange(Transaction transaction, Dictionary<int, int> change)
        {
            if (transaction.IsShort)
            {
                ShowError("Za mało pieniędzy.");
                return;
            }
            if (transaction.IsExact)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("  Nie trzeba nic oddawac\n");
                Console.ResetColor();
                return;
            }

            Console.Write("  Muszę oddać: ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(transaction.Change);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("  -------Banknoty i Monety------");
            Console.ResetColor();

            foreach (var item in change)
            {
                string unit = item.Key >= 1 ? "Zl" : "Gr";
                int denom = item.Key >= 1 ? item.Key : item.Key;
                Console.Write("  ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"{denom}");
                Console.ResetColor();
                Console.Write($" {unit} razy ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(item.Value);
                Console.ResetColor();
            }
        }

        public static void ExitPrompt()
        {
            Console.Write("\n\n  Aby zakończyć program, naciśnij dowolny przycisk...");
            Console.ReadKey();
        }
    }
}
