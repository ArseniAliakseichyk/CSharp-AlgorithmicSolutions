using System;

namespace RectangleAreaCalculatorApp
{
    // Handles user input and validation for perimeter
    public class PerimeterInputHandler
    {
        public int GetValidPerimeter()
        {
            int perimeter;

            while (true)
            {
                Console.Write("\n  Enter perimeter (integer): ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                string input = Console.ReadLine();
                Console.ResetColor();

                // Try to parse and validate input
                if (int.TryParse(input, out perimeter) && perimeter > 0)
                {
                    return perimeter;
                }

                // Display error if invalid
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\n  Invalid input. Please enter a positive whole number.");
                Console.ResetColor();
            }
        }
    }

    // Responsible for calculating the max area of a rectangle based on perimeter
    public class RectangleCalculator
    {
        private int _perimeter;

        public RectangleCalculator(int perimeter)
        {
            _perimeter = perimeter;
        }

        // Calculates max area using formula: (P/4)^2 for square
        public double CalculateMaxArea()
        {
            double side = _perimeter / 4.0;
            return side * side;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            ShowHeader();

            // Input perimeter from user
            var inputHandler = new PerimeterInputHandler();
            int perimeter = inputHandler.GetValidPerimeter();

            // Calculate max area
            var calculator = new RectangleCalculator(perimeter);
            double maxArea = calculator.CalculateMaxArea();

            ShowResult(perimeter, maxArea);
        }

        // Shows initial welcome message
        static void ShowHeader()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  Welcome to the rectangle max area calculator\n  based on the given perimeter.");
            Console.ResetColor();
            Console.WriteLine("\n  Please input the perimeter as an integer.");
            Console.WriteLine("\n  The result will be shown at the end.");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/");
            Console.ResetColor();
        }

        // Displays the result of the calculation
        static void ShowResult(int perimeter, double area)
        {
            Console.Write("\n  The maximum area for a rectangle with perimeter ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(perimeter);
            Console.ResetColor();
            Console.Write(" is ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(area);
            Console.ResetColor();

            Console.Write("\n\n\n  Press any key to exit...");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.ReadKey();
            Console.ResetColor();
        }
    }
}
