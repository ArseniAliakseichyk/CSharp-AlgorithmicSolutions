using System;

namespace Z_3
{
    // Validates user input
    public static class Validator
    {
        public static int ReadIntWithMin(string prompt, int min)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                string input = Console.ReadLine();
                Console.ResetColor();

                if (int.TryParse(input, out value) && value >= min)
                    return value;

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"  Value must be an integer ≥ {min}.");
                Console.ResetColor();
            }
        }
    }

    // Handles user input for the array
    public class ArrayInput
    {
        private readonly int[] _values;

        public ArrayInput(int size)
        {
            _values = new int[size];
        }

        public int[] GetArray()
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = Validator.ReadIntWithMin($"Enter integer #{i + 1}: ", int.MinValue);
            }
            return _values;
        }
    }

    // Sort algorithm interface (extensible for multiple algorithms)
    public interface ISorter
    {
        int[] Sort(int[] array);
    }

    // Bubble sort implementation
    public class BubbleSorter : ISorter
    {
        public int[] Sort(int[] array)
        {
            int[] sorted = (int[])array.Clone();
            int n = sorted.Length - 1;

            // Simple bubble sort
            for (int i = 0; i < sorted.Length; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (sorted[j] > sorted[j + 1])
                    {
                        int temp = sorted[j];
                        sorted[j] = sorted[j + 1];
                        sorted[j + 1] = temp;
                    }
                }
                n--;
            }
            return sorted;
        }
    }

    // Calculates sum skipping first 3 elements
    public class SumCalculator
    {
        public int CalculateSum(int[] array)
        {
            int sum = 0;

            // Sum elements starting from index 3
            for (int i = 3; i < array.Length; i++)
            {
                sum += array[i];
            }

            return sum;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/");
            Console.ResetColor();

            int size = Validator.ReadIntWithMin("Enter array size (min 3): ", 3);

            var input = new ArrayInput(size);
            int[] array = input.GetArray();

            ISorter sorter = new BubbleSorter();
            int[] sortedArray = sorter.Sort(array);

            var calculator = new SumCalculator();
            int result = calculator.CalculateSum(sortedArray);

            Console.WriteLine("........");
            Console.WriteLine($"Max sum (excluding first 3 smallest): {result}");
        }
    }
}
