using System;

namespace RefactoredLoopDemo
{
    // Provides fixed values used in the logic
    public class ValueProvider
    {
        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 1;
        public int Z { get; private set; } = 2;
    }

    // Handles looping and conditional logic
    public class LoopProcessor
    {
        private readonly ValueProvider _values;
        private const int MaxOuterIterations = 5;
        private const int MaxInnerIterations = 5;

        public LoopProcessor(ValueProvider values)
        {
            _values = values ?? throw new ArgumentNullException(nameof(values));
        }

        public void Run()
        {
            int outer = 0;
            int count = 0;

            try
            {
                while (outer <= MaxOuterIterations)
                {
                    ProcessInnerLoop(ref count);
                    outer++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        private void ProcessInnerLoop(ref int count)
        {
            int inner = 0;

            while (inner < MaxInnerIterations)
            {
                // Conditional output based on inner index
                if (inner % 2 == 0)
                {
                    if (inner == 1 || inner == 3)
                    {
                        Console.WriteLine($"X = {_values.X}");
                    }
                    else
                    {
                        Console.WriteLine($"Z = {_values.Z}");
                    }
                }
                else
                {
                    if ((inner + 1) % 3 != 0)
                    {
                        Console.WriteLine($"Y = {_values.Y}");
                    }
                    else
                    {
                        Console.WriteLine($"X = {_values.X}");
                    }
                }

                Console.WriteLine($"Count: {count}");
                count++;
                inner++;
            }
        }
    }

    // Entry point
    internal class Program
    {
        static void Main(string[] args)
        {
            var values = new ValueProvider();
            var processor = new LoopProcessor(values);
            processor.Run();

            Console.WriteLine("Processing complete.");
        }
    }
}
