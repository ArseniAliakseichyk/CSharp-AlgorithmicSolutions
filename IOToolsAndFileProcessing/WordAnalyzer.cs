using System;
using System.Collections.Generic;
using System.IO;

namespace WordAnalysisApp
{
    // Handles reading words from file and validating file path
    public class FileHandler
    {
        public string FilePath { get; }

        public FileHandler(string path)
        {
            if (!Validator.IsValidPath(path))
            {
                throw new FileNotFoundException("Invalid file path.");
            }

            FilePath = path;
        }

        public List<string> ReadWords()
        {
            List<string> words = new List<string>();
            foreach (var line in File.ReadAllLines(FilePath))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    words.Add(line.Trim());
                }
            }
            return words;
        }
    }

    // Performs word-based analysis
    public class WordAnalyzer
    {
        public int GetPalindromePower(string word)
        {
            if (word.Length <= 1) return 1;
            int power = IsPalindrome(word) ? 1 : 0;
            return power + GetPalindromePower(word.Substring(0, word.Length - 1));
        }

        private bool IsPalindrome(string word)
        {
            int left = 0, right = word.Length - 1;
            while (left < right)
            {
                if (word[left++] != word[right--]) return false;
            }
            return true;
        }

        public bool HasPalindromeInRing(string word)
        {
            if (word.Length < 6) return false;

            string ring = word + word;
            for (int i = 0; i < word.Length; i++)
            {
                string sub = ring.Substring(i, 5);
                if (IsPalindrome(sub)) return true;
            }
            return false;
        }
    }

    // Provides static validation logic
    public static class Validator
    {
        public static bool IsValidPath(string path)
        {
            return File.Exists(path);
        }

        public static bool IsValidChoice(string input, out int choice)
        {
            return int.TryParse(input, out choice) && (choice == 1 || choice == 2);
        }
    }

    // Handles all user interaction
    public class ProgramUI
    {
        public void DisplayIntro()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Welcome to the Word Analysis Tool!");
            Console.ResetColor();
            Console.WriteLine("1. Find the word with the highest palindrome power.");
            Console.WriteLine("2. Count words without palindromic 5-char substrings in a ring.");
            Console.WriteLine("\nChoose file input method:\n1. Use local file (slowa.txt)\n2. Enter full file path.");
        }

        public int GetUserChoice()
        {
            Console.Write("Choice (1 or 2): ");
            string input = Console.ReadLine();
            if (!Validator.IsValidChoice(input, out int choice))
            {
                throw new ArgumentException("Invalid menu option.");
            }
            return choice;
        }

        public string GetFilePath(int option)
        {
            if (option == 1)
            {
                return "slowa.txt";
            }
            else
            {
                Console.Write("Enter full path to file (.txt): ");
                return Console.ReadLine();
            }
        }

        public void DisplayResult(string word, int power, int index)
        {
            Console.WriteLine($"\n[RESULT] Word: {word} | Power: {power} | Line Index: {index + 1}");
        }

        public void DisplayRingResult(int count)
        {
            Console.WriteLine($"\n[RESULT] Words with no palindromic ring substrings: {count}");
        }

        public void WaitForExit()
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

    internal class Program
    {
        static void Main()
        {
            var ui = new ProgramUI();
            var analyzer = new WordAnalyzer();

            try
            {
                ui.DisplayIntro();
                int choice = ui.GetUserChoice();
                string path = ui.GetFilePath(choice);

                var fileHandler = new FileHandler(path);
                List<string> words = fileHandler.ReadWords();

                int maxPower = -1;
                List<(string word, int power, int index)> bestWords = new List<(string, int, int)>();

                for (int i = 0; i < words.Count; i++)
                {
                    int power = analyzer.GetPalindromePower(words[i]);
                    if (power > maxPower)
                    {
                        maxPower = power;
                        bestWords.Clear();
                        bestWords.Add((words[i], power, i));
                    }
                    else if (power == maxPower)
                    {
                        bestWords.Add((words[i], power, i));
                    }
                }

                Console.Clear();
                foreach (var (word, power, index) in bestWords)
                {
                    ui.DisplayResult(word, power, index);
                }

                int count = 0;
                for (int i = 0; i < words.Count; i++)
                {
                    if (words[i].Length >= 6 && !analyzer.HasPalindromeInRing(words[i]))
                    {
                        count++;
                    }
                }

                ui.DisplayRingResult(count);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERROR] {ex.Message}");
                Console.ResetColor();
            }

            ui.WaitForExit();
        }
    }
}
