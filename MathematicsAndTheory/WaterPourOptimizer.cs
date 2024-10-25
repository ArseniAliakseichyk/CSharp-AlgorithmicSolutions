using System;

namespace WaterOptimizerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            UI.PrintIntro();

            int targetLiters = InputHandler.GetTargetLiters();

            var optimizer = new WaterPourOptimizer();
            var result = optimizer.CalculateOptimalUsage(targetLiters);

            UI.PrintResult(result);
        }
    }

    public class WaterPourResult
    {
        public int Count2L { get; set; }
        public int Count3L { get; set; }
        public int Count11L { get; set; }

        public int TotalUses => Count2L + Count3L + Count11L;
    }

    public class WaterPourOptimizer
    {
        private readonly int[] measures = { 2, 3, 11 };

        public WaterPourResult CalculateOptimalUsage(int target)
        {
            var result = new WaterPourResult();

            // Use as many 11L as possible
            result.Count11L = target / 11;
            target -= result.Count11L * 11;

            // Handle remainder using combinations of 2L and 3L
            while (target > 0)
            {
                if (TryHandleRemainder(target, result))
                    break;

                // If not divisible, reduce one 11L and try again
                result.Count11L--;
                target += 11;

                if (result.Count11L < 0)
                    break;
            }

            return result;
        }

        private bool TryHandleRemainder(int target, WaterPourResult result)
        {
            // Hardcoded logic for small remainders
            switch (target)
            {
                case 1:
                    result.Count2L++;
                    result.Count3L++;
                    return true;
                case 2:
                    result.Count2L++;
                    return true;
                case 3:
                    result.Count3L++;
                    return true;
                case 4:
                    result.Count2L += 2;
                    return true;
                case 5:
                    result.Count2L++;
                    result.Count3L++;
                    return true;
                case 6:
                    result.Count3L += 2;
                    return true;
                case 7:
                    result.Count2L += 2;
                    result.Count3L++;
                    return true;
                case 8:
                    result.Count3L++;
                    result.Count11L--;
                    return true;
                case 9:
                    result.Count2L++;
                    result.Count11L--;
                    return true;
                case 10:
                    result.Count2L++;
                    result.Count3L++;
                    result.Count11L--;
                    return true;
                default:
                    return false;
            }
        }
    }

    public static class InputHandler
    {
        public static int GetTargetLiters()
        {
            int liters = 0;
            bool valid = false;

            while (!valid)
            {
                try
                {
                    Console.Write("\n  Zapisz, ile wody chcesz nalać(w litrach): ");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    string input = Console.ReadLine();
                    Console.ResetColor();

                    liters = int.Parse(input);
                    if (liters <= 0 || liters > 1000)
                        throw new ArgumentOutOfRangeException();

                    valid = true;
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("  Wpisz liczbę z zakresu 1-1000.");
                    Console.ResetColor();
                }
            }

            return liters;
        }
    }

    public static class UI
    {
        public static void PrintIntro()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  Witamy w programie do znajdowania minimalnej liczby\n  czynności podczas przenoszenia wody.");
            Console.ResetColor();
            Console.WriteLine("    \\       /          \\       /");
            Console.WriteLine("    |       |          |       |");
            Console.Write("    | 1000L |   ===>   |   ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("0L");
            Console.ResetColor();
            Console.WriteLine("  |");
            Console.WriteLine("    |_______|          |_______|");
            Console.WriteLine("    Zbiornik 1         Zbiornik 2");
            Console.WriteLine("\n  Mając tylko ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("2-litr");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(" 3-litr");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(" 11-litr");
            Console.ResetColor();
            Console.WriteLine(" miarki, otrzymasz najbardziej optymalną opcję.\n");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/\n");
            Console.ResetColor();
        }

        public static void PrintResult(WaterPourResult result)
        {
            Console.WriteLine("\n  Do nalewania użyj: ");

            if (result.Count2L > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("  2-litrowej");
                Console.ResetColor();
                Console.WriteLine($" miarki {result.Count2L} razy");
            }

            if (result.Count3L > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("  3-litrowej");
                Console.ResetColor();
                Console.WriteLine($" miarki {result.Count3L} razy");
            }

            if (result.Count11L > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("  11-litrowej");
                Console.ResetColor();
                Console.WriteLine($" miarki {result.Count11L} razy");
            }

            if (result.TotalUses == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  żadnej miarki");
                Console.ResetColor();
            }
            else
            {
                Console.Write("\n  Liczba zastosowań: ");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"{result.TotalUses}");
                Console.ResetColor();
            }
        }
    }
}
