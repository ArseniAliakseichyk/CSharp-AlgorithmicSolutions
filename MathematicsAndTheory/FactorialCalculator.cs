using System;
using System.Numerics;

namespace MultiFactorialApp
{
    // Represents the input parameters for factorial calculation
    public class FactorialInput
    {
        private int _n;
        private int _k;

        public int N
        {
            get => _n;
            set
            {
                if (value < 1 || value > 1000)
                    throw new ArgumentOutOfRangeException(nameof(N), "Value must be between 1 and 1000.");
                _n = value;
            }
        }

        public int K
        {
            get => _k;
            set
            {
                if (value < 1 || value > 4)
                    throw new ArgumentOutOfRangeException(nameof(K), "Value must be between 1 and 4.");
                _k = value;
            }
        }

        public FactorialInput(int n, int k)
        {
            N = n;
            K = k;
        }
    }

    // Handles factorial calculations, supports multi-factorial (!, !!, !!!, !!!!)
    public class FactorialCalculator
    {
        // Calculates multi-factorial of n with step k using recursion
        public BigInteger Calculate(int n, int k)
        {
            if (n == 0)
                return 1;
            if (n <= k)
                return n;
            return n * Calculate(n - k, k);
        }
    }

    // Manages user interaction and input/output logic
    public class ConsoleInterface
    {
        private readonly FactorialCalculator _calculator = new();

        public void Run()
        {
            ShowWelcomeMessage();

            FactorialInput input;
            try
            {
                input = ReadInput();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
                return;
            }

            Console.Clear();
            BigInteger result = _calculator.Calculate(input.N, input.K);

            ShowResult(input, result);
        }

        private void ShowWelcomeMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  Witamy w programie obliczeń silniowych");
            Console.ResetColor();
            Console.Write("  W tym programie możesz podać liczby od ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("0 do 1000 ");
            Console.ResetColor();
            Console.WriteLine("  nz których musisz obliczyć silnię");
            Console.Write("  I podaj wartość silni na przykładzie ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("1=!, 2=!!, 3=!!!, 4=!!!!");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/");
            Console.ResetColor();
        }

        private FactorialInput ReadInput()
        {
            int n = ReadInteger("Podaj wartość której musisz obliczyć silnię (1-1000): ", 1, 1000);
            int k = ReadInteger("Podaj wartość silni (1-4): ", 1, 4);

            return new FactorialInput(n, k);
        }

        // Read integer from console with validation and prompt retry on invalid input
        private int ReadInteger(string prompt, int min, int max)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out value))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                    Console.ResetColor();
                    continue;
                }

                if (value < min || value > max)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Value must be between {min} and {max}.");
                    Console.ResetColor();
                    continue;
                }

                break;
            }
            return value;
        }

        private void ShowResult(FactorialInput input, BigInteger result)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"{input.N}");
            for (int i = 0; i < input.K; i++)
            {
                Console.Write("!");
            }
            Console.ResetColor();
            Console.Write($" = {result}");
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main()
        {
            var consoleInterface = new ConsoleInterface();
            consoleInterface.Run();
        }
    }
}
