using System;

namespace TapeSplitterApp
{
    /// <summary>
    /// Entry point for the tape splitter program.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Display description
                UI.PrintDescription();

                // Initialize tape
                Tape tape = new Tape(1000);
                tape.GenerateRandom(-100, 100);

                // Process optimal cut
                TapeAnalyzer analyzer = new TapeAnalyzer(tape);
                CutResult result = analyzer.FindOptimalCut();

                // Display result
                UI.PrintResult(result);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                Console.ResetColor();
            }
        }
    }

    /// <summary>
    /// Responsible for displaying UI content
    /// </summary>
    public static class UI
    {
        public static void PrintDescription()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(" \"");
            Console.ResetColor();
            Console.Write(" Mamy do dyspozycji długą papierową taśmę, na której może zmieścić się nawet\n  1000 liczb całkowitych z przedziału (-100; 100). Chcielibyśmy wiedzieć,\n  w którym miejscu (po której z kolei liczbie) możną przeciąć ją tak,\n  by różnica między sumą liczb na jednym kawałku a sumą liczb na drugim\n  kawałku była jak najmniejsza oraz chcielibyśmy poznać wartość\n  bezwzględną tej różnicy. Napisz program, który pozwala to ustalić.");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(" \"\n");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/");
            Console.ResetColor();
        }

        public static void PrintResult(CutResult result)
        {
            Console.Write("\n  Miejsce przecięcia taśmy: ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(result.CutIndex);
            Console.ResetColor();
            Console.Write("\n  Wartość bezwzględna różnicy sum: ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(result.AbsoluteDifference);
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Represents the tape (sequence of integers)
    /// </summary>
    public class Tape
    {
        private int[] _numbers;

        public int[] Numbers => _numbers;

        public Tape(int length)
        {
            if (length <= 0 || length > 1000)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be between 1 and 1000.");
            }
            _numbers = new int[length];
        }

        /// <summary>
        /// Fill the tape with random integers in the specified range
        /// </summary>
        public void GenerateRandom(int min, int max)
        {
            if (min >= max || min < -100 || max > 100)
            {
                throw new ArgumentException("Invalid range. Must be within -100 and 100, and min < max.");
            }

            Random rand = new Random();
            for (int i = 0; i < _numbers.Length; i++)
            {
                _numbers[i] = rand.Next(min, max + 1);
            }
        }
    }

    /// <summary>
    /// Responsible for analyzing the tape
    /// </summary>
    public class TapeAnalyzer
    {
        private readonly Tape _tape;

        public TapeAnalyzer(Tape tape)
        {
            _tape = tape ?? throw new ArgumentNullException(nameof(tape));
        }

        /// <summary>
        /// Find the best cut to minimize absolute difference between sums
        /// </summary>
        public CutResult FindOptimalCut()
        {
            int[] nums = _tape.Numbers;
            int totalSum = 0;

            foreach (int n in nums)
            {
                totalSum += n;
            }

            int leftSum = 0;
            int rightSum = totalSum;
            int minDiff = int.MaxValue;
            int cutIndex = 0;

            // Iterate to find optimal cut point
            for (int i = 0; i < nums.Length - 1; i++)
            {
                leftSum += nums[i];
                rightSum -= nums[i];
                int diff = Math.Abs(leftSum - rightSum);

                if (diff < minDiff)
                {
                    minDiff = diff;
                    cutIndex = i + 1;
                }
            }

            return new CutResult(cutIndex, minDiff);
        }
    }

    /// <summary>
    /// Represents the result of the cut operation
    /// </summary>
    public class CutResult
    {
        public int CutIndex { get; }
        public int AbsoluteDifference { get; }

        public CutResult(int cutIndex, int absDiff)
        {
            CutIndex = cutIndex;
            AbsoluteDifference = absDiff;
        }
    }
}
