using System;

namespace IntegrationApp
{
    // Handles user input and validation
    class IntegralInput
    {
        public double LowerBound { get; private set; }
        public double UpperBound { get; private set; }
        public int PrecisionLevel { get; private set; }

        public void ReadInput()
        {
            Console.Write("  Enter lower bound: ");
            LowerBound = Validator.ReadDouble();

            Console.Write("  Enter upper bound: ");
            UpperBound = Validator.ReadDouble();

            Console.WriteLine("\n  Choose precision level:");
            Console.WriteLine("  1. Low\n  2. Medium\n  3. High\n  4. Very High\n  5. Maximum (may take long)");
            Console.Write("  Your choice (1-5): ");
            PrecisionLevel = Validator.ReadIntInRange(1, 5);
        }

        public int GetPointsCount()
        {
            int[] points = { 10, 10000, 1000000, 100000000, int.MaxValue };
            return points[PrecisionLevel - 1];
        }
    }

    // Performs integration using different methods
    class Integrator
    {
        private readonly Func<double, double> _function;
        private readonly ProgressTracker _tracker;

        public Integrator(Func<double, double> function, ProgressTracker tracker)
        {
            _function = function;
            _tracker = tracker;
        }

        public double RectangleMethod(double low, double up, int n)
        {
            double dx = (up - low) / n;
            double sum = 0.0;

            for (int i = 0; i < n; i++)
            {
                double x = low + i * dx;
                sum += _function(x);
                _tracker.Update(i);
            }

            return sum * dx;
        }

        public double TrapezoidalMethod(double low, double up, int n)
        {
            double dx = (up - low) / n;
            double sum = (_function(low) + _function(up)) / 2.0;

            for (int i = 1; i < n; i++)
            {
                double x = low + i * dx;
                sum += _function(x);
                _tracker.Update(i);
            }

            return sum * dx;
        }

        public double MonteCarloMethod(double low, double up, int n)
        {
            Random rand = new Random();
            double sum = 0.0;

            for (int i = 0; i < n; i++)
            {
                double x = low + (up - low) * rand.NextDouble();
                sum += _function(x);
                _tracker.Update(i);
            }

            return (up - low) * sum / n;
        }
    }

    // Tracks and displays progress
    class ProgressTracker
    {
        private int _interval;
        private double _progress = 0.0;
        private readonly double _increment = 1.0 / 3.0;

        public void Init(int totalSteps)
        {
            _interval = Math.Max(totalSteps / 100, 1);
        }

        public void Update(int step)
        {
            if (step % _interval == 0)
            {
                Console.Clear();
                Console.WriteLine($"  Progress: {Math.Round(_progress, 2)}%");
                _progress += _increment;
            }
        }
    }

    // Utility for input validation
    static class Validator
    {
        public static double ReadDouble()
        {
            while (true)
            {
                try
                {
                    return Convert.ToDouble(Console.ReadLine());
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("  Invalid input. Please enter a number: ");
                    Console.ResetColor();
                }
            }
        }

        public static int ReadIntInRange(int min, int max)
        {
            while (true)
            {
                try
                {
                    int val = Convert.ToInt32(Console.ReadLine());
                    if (val >= min && val <= max)
                        return val;

                    throw new ArgumentOutOfRangeException();
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"  Enter a number between {min} and {max}: ");
                    Console.ResetColor();
                }
            }
        }
    }

    internal class Program
    {
        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n  Numerical Integration of sin(x)");
            Console.WriteLine("  Rectangle, Trapezoid, and Monte Carlo methods\n");
            Console.ResetColor();

            var input = new IntegralInput();
            input.ReadInput();
            int n = input.GetPointsCount();

            var tracker = new ProgressTracker();
            tracker.Init(n);

            var integrator = new Integrator(Math.Sin, tracker);

            double rect = integrator.RectangleMethod(input.LowerBound, input.UpperBound, n);
            double trap = integrator.TrapezoidalMethod(input.LowerBound, input.UpperBound, n);
            double monte = integrator.MonteCarloMethod(input.LowerBound, input.UpperBound, n);

            Console.Clear();
            Console.WriteLine("\n  Integration Results:");
            Console.WriteLine($"  Lower Bound: {input.LowerBound}");
            Console.WriteLine($"  Upper Bound: {input.UpperBound}");
            Console.WriteLine($"  Rectangle Method: {rect}");
            Console.WriteLine($"  Trapezoidal Method: {trap}");
            Console.WriteLine($"  Monte Carlo Method: {monte}\n");
        }
    }
}
