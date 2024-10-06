using System;

namespace CirclePointCounterApp
{
    // Handles user input and application control flow
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter circle radius: ");
            string input = Console.ReadLine();

            try
            {
                // Parse and validate input
                if (!double.TryParse(input, out double radius))
                {
                    throw new ArgumentException("Input is not a valid number.");
                }

                if (radius <= 0)
                {
                    throw new ArgumentOutOfRangeException("Radius must be positive.");
                }

                // Calculate and display result
                var circle = new Circle(radius);
                var counter = new PointCounter(circle);
                int pointsInside = counter.CountIntegerPointsInside();

                Console.WriteLine($"\nRadius = {radius} => Integer Points Inside = {pointsInside}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    // Represents a geometric circle
    public class Circle
    {
        private double _radius;

        public double Radius
        {
            get => _radius;
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Radius), "Radius must be greater than zero.");
                }
                _radius = value;
            }
        }

        public Circle(double radius)
        {
            Radius = radius;
        }

        // Returns radius floored to nearest integer
        public int IntRadius => (int)Math.Floor(_radius);
    }

    // Responsible for counting integer coordinate points inside a given circle
    public class PointCounter
    {
        private readonly Circle _circle;

        public PointCounter(Circle circle)
        {
            _circle = circle;
        }

        public int CountIntegerPointsInside()
        {
            int count = 0;
            int r = _circle.IntRadius;

            // Iterate over grid and check if point is inside the circle
            for (int x = -r; x <= r; x++)
            {
                for (int y = -r; y <= r; y++)
                {
                    if (x * x + y * y < r * r)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
