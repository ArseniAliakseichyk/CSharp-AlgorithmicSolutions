using System;
using System.Text.RegularExpressions;

namespace CardPinDecoderApp
{
    // Responsible for user input and output
    internal class Program
    {
        static void Main(string[] args)
        {
            UI.ShowBanner();

            Console.Write("\n  Enter last 4 characters on card (uppercase letters or digits): ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            string input = Console.ReadLine()?.Trim().ToUpper();
            Console.ResetColor();

            try
            {
                Validator.ValidateCardCode(input);
                CardCode cardCode = new CardCode(input);
                PinDecoder decoder = new PinDecoder();
                int[] pin = decoder.Decode(cardCode);

                Console.Write("\n  PIN code for this card: ");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                foreach (int digit in pin)
                {
                    Console.Write(digit);
                }
                Console.ResetColor();
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  Error: {ex.Message}");
                Console.ResetColor();
            }
        }
    }

    // Represents a card code entity
    public class CardCode
    {
        public string Value { get; }

        public CardCode(string value)
        {
            Value = value;
        }
    }

    // Handles PIN decoding logic
    public class PinDecoder
    {
        private readonly int[] bankMap = { 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }; // Map A-J to 0-9

        public int[] Decode(CardCode cardCode)
        {
            int[] pin = new int[cardCode.Value.Length];

            for (int i = 0; i < cardCode.Value.Length; i++)
            {
                char c = cardCode.Value[i];
                if (char.IsDigit(c))
                {
                    int digit = int.Parse(c.ToString());
                    pin[i] = digit <= 0 ? 0 : bankMap[digit];
                }
                else if (char.IsUpper(c) && c >= 'A' && c <= 'J')
                {
                    pin[i] = bankMap[c - 'A'];
                }
                else
                {
                    throw new ArgumentException($"Unsupported character: '{c}'");
                }
            }

            return pin;
        }
    }

    // Validates user input
    public static class Validator
    {
        public static void ValidateCardCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Code cannot be empty.");

            if (code.Length != 4)
                throw new ArgumentException("Code must be exactly 4 characters.");

            if (!Regex.IsMatch(code, @"^[A-J0-9]{4}$"))
                throw new ArgumentException("Code must contain only uppercase letters A-J or digits 0-9.");
        }
    }

    // Handles UI rendering
    public static class UI
    {
        public static void ShowBanner()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  Welcome to the PIN Code Decoder.");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  (All actions are for educational purposes only)\n");
            Console.ResetColor();
            Console.WriteLine("  Enter the last four characters from your card.");
            Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/");
        }
    }
}
