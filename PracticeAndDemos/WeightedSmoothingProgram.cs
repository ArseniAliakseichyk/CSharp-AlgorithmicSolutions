using System;

namespace WeightedSmoothing
{
    // Represents a numeric sequence with a smoothing operation
    public class DataProcessor
    {
        private double[] _numbers;
        private double[] _weights;
        private readonly Random _rand = new();

        public double[] Numbers => _numbers;
        public double[] Weights => _weights;

        public DataProcessor()
        {
            _numbers = GenerateRandomArray(10, -10, 10);
            _weights = GenerateRandomArray(3, 0.01, 0.99); // (0,1) exclusive
        }

        // Generate an array of doubles with specific range and 2 decimal rounding
        private double[] GenerateRandomArray(int length, double min, double max)
        {
            double[] result = new double[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = Math.Round(_rand.NextDouble() * (max - min) + min, 2);
            }

            return result;
        }

        // Apply weighted smoothing using neighbor elements
        public double[] ApplySmoothing()
        {
            double[] smoothed = new double[_numbers.Length];

            for (int i = 0; i < _numbers.Length; i++)
            {
                int prev = (i == 0) ? _numbers.Length - 1 : i - 1;
                int next = (i == _numbers.Length - 1) ? 0 : i + 1;

                smoothed[i] = Math.Round(
                    _numbers[prev] * _weights[0] +
                    _numbers[i] * _weights[1] +
                    _numbers[next] * _weights[2], 2);
            }

            return smoothed;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(" \"");
                Console.ResetColor();
                Console.Write("This program generates two arrays: one with 10 random real numbers in [-10,10] and one with 3 weights in (0,1). It performs weighted smoothing on the number array using the weights.");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(" \"\n");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/");
                Console.ResetColor();

                DataProcessor processor = new DataProcessor();

                double[] numbers = processor.Numbers;
                double[] weights = processor.Weights;
                double[] result = processor.ApplySmoothing();

                Console.WriteLine("\n  Numbers array: " + string.Join(" ", numbers));
                Console.WriteLine("\n  Weights array: " + string.Join(" ", weights));
                Console.WriteLine("\n  Smoothed result: " + string.Join(" ", result));
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
