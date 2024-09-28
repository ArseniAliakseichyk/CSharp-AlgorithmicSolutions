using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ScholarshipBonus
{
    // Represents a student with name and average grade
    public class Student
    {
        private string firstName;
        private string lastName;
        private double averageGrade;

        public string FirstName
        {
            get => firstName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("First name cannot be empty");
                firstName = value;
            }
        }

        public string LastName
        {
            get => lastName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Last name cannot be empty");
                lastName = value;
            }
        }

        public double AverageGrade
        {
            get => averageGrade;
            private set
            {
                if (value < 0 || value > 5)
                    throw new ArgumentOutOfRangeException(nameof(AverageGrade), "Average grade must be between 0 and 5");
                averageGrade = value;
            }
        }

        public Student(string firstName, string lastName, double averageGrade)
        {
            FirstName = firstName;
            LastName = lastName;
            AverageGrade = averageGrade;
        }
    }

    // Handles loading students from a text file with validation
    public class StudentRepository
    {
        private readonly string filePath;

        public StudentRepository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty");
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            this.filePath = filePath;
        }

        public List<Student> LoadStudents()
        {
            var students = new List<Student>();
            try
            {
                var lines = File.ReadAllLines(filePath);
                if (lines.Length <= 1)
                    throw new InvalidDataException("File does not contain student data");

                // Skip header line
                for (int i = 1; i < lines.Length; i++)
                {
                    var line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line)) continue;

                    var parts = Regex.Split(line, @"\s+"); // split by spaces or tabs
                    if (parts.Length < 3)
                        throw new InvalidDataException($"Invalid line format at line {i + 1}");

                    string firstName = parts[0];
                    string lastName = parts[1];
                    string avgStr = parts[2].Replace(',', '.'); // support comma or dot decimal separator

                    if (!double.TryParse(avgStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double avgGrade))
                        throw new InvalidDataException($"Invalid average grade at line {i + 1}");

                    var student = new Student(firstName, lastName, avgGrade);
                    students.Add(student);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading students: {ex.Message}", ex);
            }

            return students;
        }
    }

    // Encapsulates logic for calculating bonuses according to grade brackets
    public class BonusCalculator
    {
        private readonly List<Student> students;
        private readonly int totalBonus;

        private Dictionary<string, double> gradeBracketsPercent = new Dictionary<string, double>
        {
            { "5-4.75", 0.6 },
            { "4.75-4.5", 0.4 },
            { "4.5-4.25", 0.2 },
            { "4.25-4", 0.0 }
        };

        private Dictionary<string, int> bracketCounts = new Dictionary<string, int>
        {
            { "5-4.75", 0 },
            { "4.75-4.5", 0 },
            { "4.5-4.25", 0 },
            { "4.25-4", 0 }
        };

        private Dictionary<string, int> bonuses = new Dictionary<string, int>();

        public BonusCalculator(List<Student> students, int totalBonus)
        {
            this.students = students ?? throw new ArgumentNullException(nameof(students));
            if (totalBonus <= 0)
                throw new ArgumentException("Total bonus must be positive", nameof(totalBonus));
            this.totalBonus = totalBonus;
        }

        // Classifies students into grade brackets and counts them
        private void CountStudentsByBracket()
        {
            foreach (var student in students)
            {
                if (student.AverageGrade >= 4.75)
                    bracketCounts["5-4.75"]++;
                else if (student.AverageGrade >= 4.5)
                    bracketCounts["4.75-4.5"]++;
                else if (student.AverageGrade >= 4.25)
                    bracketCounts["4.5-4.25"]++;
                else if (student.AverageGrade >= 4)
                    bracketCounts["4.25-4"]++;
            }
        }

        // Calculate bonuses per bracket proportionally
        public void CalculateBonuses()
        {
            CountStudentsByBracket();

            // Calculate weighted count sum based on bracket percentages
            double weightedSum = bracketCounts["4.25-4"] +
                                 bracketCounts["4.5-4.25"] * (1 + gradeBracketsPercent["4.5-4.25"]) +
                                 bracketCounts["4.75-4.5"] * (1 + gradeBracketsPercent["4.75-4.5"]) +
                                 bracketCounts["5-4.75"] * (1 + gradeBracketsPercent["5-4.75"]);

            if (weightedSum == 0)
                throw new InvalidOperationException("No students eligible for bonuses");

            // Start with base bonus for lowest bracket
            int baseBonus = (int)Math.Floor(totalBonus / weightedSum);

            // Calculate bonuses per bracket
            bonuses["4.25-4"] = baseBonus;
            bonuses["4.5-4.25"] = (int)(baseBonus * (1 + gradeBracketsPercent["4.5-4.25"]));
            bonuses["4.75-4.5"] = (int)(bonuses["4.5-4.25"] * (1 + 0.2)); // incremental 20%
            bonuses["5-4.75"] = (int)(bonuses["4.75-4.5"] * (1 + 0.2));

            // Adjust bonuses so total is as close as possible to totalBonus
            int sumBonuses = bonuses.Sum(b => b.Value * bracketCounts[b.Key]);

            while (sumBonuses < totalBonus)
            {
                bonuses["4.25-4"]++;
                bonuses["4.5-4.25"] = (int)(bonuses["4.25-4"] * (1 + gradeBracketsPercent["4.5-4.25"]));
                bonuses["4.75-4.5"] = (int)(bonuses["4.5-4.25"] * (1 + 0.2));
                bonuses["5-4.75"] = (int)(bonuses["4.75-4.5"] * (1 + 0.2));
                sumBonuses = bonuses.Sum(b => b.Value * bracketCounts[b.Key]);
            }
            while (sumBonuses > totalBonus)
            {
                bonuses["4.25-4"]--;
                bonuses["4.5-4.25"] = (int)(bonuses["4.25-4"] * (1 + gradeBracketsPercent["4.5-4.25"]));
                bonuses["4.75-4.5"] = (int)(bonuses["4.5-4.25"] * (1 + 0.2));
                bonuses["5-4.75"] = (int)(bonuses["4.75-4.5"] * (1 + 0.2));
                sumBonuses = bonuses.Sum(b => b.Value * bracketCounts[b.Key]);
            }
        }

        // Returns bonus amount for a student based on their grade
        public int GetBonusForStudent(Student student)
        {
            if (student.AverageGrade >= 4.75)
                return bonuses["5-4.75"];
            if (student.AverageGrade >= 4.5)
                return bonuses["4.75-4.5"];
            if (student.AverageGrade >= 4.25)
                return bonuses["4.5-4.25"];
            if (student.AverageGrade >= 4)
                return bonuses["4.25-4"];
            return 0;
        }

        public Dictionary<string, int> GetBracketCounts() => new Dictionary<string, int>(bracketCounts);

        public Dictionary<string, int> GetBonuses() => new Dictionary<string, int>(bonuses);

        public int GetTotalBonusDistributed() => bonuses.Sum(b => b.Value * bracketCounts[b.Key]);
    }

    internal class Program
    {
        private static void Main()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Welcome to the scholarship bonus calculator.");
                Console.WriteLine("You can provide your own txt file with student data.");
                Console.ResetColor();

                Console.Write("Enter the total bonus amount to distribute (e.g. 10000): ");
                if (!int.TryParse(Console.ReadLine(), out int totalBonus) || totalBonus <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid bonus amount. Program will exit.");
                    return;
                }

                Console.Write("Choose file input option (1 - default location, 2 - custom path): ");
                if (!int.TryParse(Console.ReadLine(), out int option) || (option != 1 && option != 2))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option selected. Program will exit.");
                    return;
                }

                string path = "stypendia.txt";
                if (option == 2)
                {
                    Console.Write("Enter full path to your .txt file: ");
                    path = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("File path cannot be empty. Program will exit.");
                        return;
                    }
                }

                var repo = new StudentRepository(path);
                var students = repo.LoadStudents();

                var calculator = new BonusCalculator(students, totalBonus);
                calculator.CalculateBonuses();

                Console.Clear();
                // Display student info with bonuses
                foreach (var student in students)
                {
                    int bonus = calculator.GetBonusForStudent(student);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write($"Name: {student.FirstName} {student.LastName}, ");
                    Console.ResetColor();
                    Console.Write($"Average Grade: {student.AverageGrade:#0.00}, ");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine($"Bonus: {bonus} Zł");
                    Console.ResetColor();
                }

                // Display summary
                Console.WriteLine("\nStudents eligible for bonuses:");
                var counts = calculator.GetBracketCounts();
                foreach (var bracket in counts.Keys)
                {
                    Console.WriteLine($" {bracket}: {counts[bracket]}");
                }

                Console.WriteLine("\nBonuses per bracket:");
                var bonuses = calculator.GetBonuses();
                foreach (var bracket in bonuses.Keys)
                {
                    Console.WriteLine($" {bracket}: {bonuses[bracket]} Zł");
                }

                int distributed = calculator.GetTotalBonusDistributed();
                Console.WriteLine($"\nTotal bonus distributed: {distributed} Zł");
                Console.WriteLine($"Bonus remaining undistributed: {totalBonus - distributed} Zł");

                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Fatal error: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
