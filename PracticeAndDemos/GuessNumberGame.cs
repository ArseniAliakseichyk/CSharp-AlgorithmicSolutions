using System;

namespace GuessNumberGame
{
    // Class responsible for generating the secret number and range
    public class NumberRange
    {
        private readonly int _min;
        private readonly int _max;
        private readonly int _secretNumber;

        public int Min => _min;
        public int Max => _max;
        public int SecretNumber => _secretNumber;

        private static readonly Random rnd = new Random();

        public NumberRange()
        {
            _min = rnd.Next(0, 71);
            _max = _min + 30;
            _secretNumber = rnd.Next(_min, _max + 1);
        }
    }

    // Class responsible for user interaction and input validation
    public class UserInputHandler
    {
        public int GetValidatedNumberInput(int min, int max)
        {
            while (true)
            {
                Console.Write($"Enter a number between {min} and {max}: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int number))
                {
                    Console.WriteLine("Invalid input! Please enter a valid integer.");
                    continue;
                }
                if (number < min || number > max)
                {
                    Console.WriteLine($"Number out of range! Please enter a number between {min} and {max}.");
                    continue;
                }
                return number;
            }
        }
    }

    // Main game logic encapsulated here
    public class GuessingGame
    {
        private readonly NumberRange _numberRange;
        private readonly UserInputHandler _inputHandler;

        public GuessingGame()
        {
            _numberRange = new NumberRange();
            _inputHandler = new UserInputHandler();
        }

        public void Play()
        {
            Console.WriteLine("Welcome to the Guess Number Game!");
            Console.WriteLine($"Try to guess the secret number between {_numberRange.Min} and {_numberRange.Max}.");
            Console.WriteLine("After each guess, you'll get a hint if your guess is too high or too low.");
            Console.WriteLine("Press any key to start...");
            Console.ReadKey();
            Console.Clear();

            int attempts = 0;
            int guess;

            do
            {
                guess = _inputHandler.GetValidatedNumberInput(_numberRange.Min, _numberRange.Max);
                attempts++;

                if (guess != _numberRange.SecretNumber)
                {
                    PrintHint(guess);
                }

            } while (guess != _numberRange.SecretNumber);

            Console.WriteLine("*********************************************************************");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"Congratulations! You found the answer in {attempts} attempts.");
            Console.ResetColor();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private void PrintHint(int guess)
        {
            Console.Write("Wrong number. Try a ");
            if (guess > _numberRange.SecretNumber)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("smaller");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("bigger");
            }
            Console.ResetColor();
            Console.WriteLine(" number.");
        }
    }

    internal class Program
    {
        static void Main()
        {
            try
            {
                var game = new GuessingGame();
                game.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }
    }
}
