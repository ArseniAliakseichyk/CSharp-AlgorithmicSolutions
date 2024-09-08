using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace MaxSubarrayAnalyzer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Place all text files into:\nZ1\\bin\\Debug\\net8.0");
            Console.WriteLine("Required files:\nnajlepsza_1.txt, najlepsza_2.txt, najlepsza_3.txt");
            Console.WriteLine("Press any key to start...");
            Console.ReadKey();

            var fileNames = new[] { "najlepsza_1.txt", "najlepsza_2.txt", "najlepsza_3.txt" };

            var processor = new FileProcessor(fileNames);
            var results = processor.ProcessFiles();

            ResultPrinter.PrintResults(results);
        }
    }

    class FileProcessor
    {
        private readonly string[] _fileNames;

        public FileProcessor(string[] fileNames)
        {
            _fileNames = fileNames;
        }

        public List<FileAnalysisResult> ProcessFiles()
        {
            var results = new List<FileAnalysisResult>();

            foreach (var file in _fileNames)
            {
                try
                {
                    var lines = File.ReadAllLines(file);
                    if (!Validator.ValidateFileContent(lines, out var numbers))
                    {
                        Console.WriteLine($"Invalid data in file: {file}");
                        continue;
                    }

                    var runner = new AlgorithmRunner();
                    var result = runner.RunAlgorithms(numbers, file);

                    results.Add(result);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine($"File not found: {file}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error processing {file}: {ex.Message}");
                }
            }

            return results;
        }
    }

    class AlgorithmRunner
    {
        // Brute-force algorithm with progress display
        public FileAnalysisResult RunAlgorithms(int[] data, string fileName)
        {
            var stopwatch = new Stopwatch();
            var result = new FileAnalysisResult { FileName = fileName };

            stopwatch.Start();
            result.Algorithm1Result = MaxSubarrayBruteForce(data, fileName);
            stopwatch.Stop();
            result.Algorithm1Time = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            result.Algorithm2Result = MaxSubarrayKadane(data);
            stopwatch.Stop();
            result.Algorithm2Time = stopwatch.ElapsedMilliseconds;

            return result;
        }

        private int MaxSubarrayBruteForce(int[] data, string fileName)
        {
            int max = 0;
            double progress = 0;
            double step = 100.0 / data.Length;

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = i; j < data.Length; j++)
                {
                    int sum = 0;
                    for (int k = i; k <= j; k++)
                    {
                        sum += data[k];
                    }
                    if (sum > max)
                        max = sum;
                }

                // Display progress for long operations
                progress += step;
                if (i > 8 && i % (i / 8) == 0)
                {
                    Console.Clear();
                    Console.WriteLine($"File: {fileName}\nProgress: {Math.Round(progress, 2)}%");
                }
            }

            return max;
        }

        private int MaxSubarrayKadane(int[] data)
        {
            int max = 0, current = 0;
            foreach (var num in data)
            {
                current = Math.Max(0, current + num);
                max = Math.Max(max, current);
            }
            return max;
        }
    }

    class FileAnalysisResult
    {
        public string FileName { get; set; }
        public int Algorithm1Result { get; set; }
        public long Algorithm1Time { get; set; }
        public int Algorithm2Result { get; set; }
        public long Algorithm2Time { get; set; }
    }

    static class Validator
    {
        // Validates that all lines are valid integers
        public static bool ValidateFileContent(string[] lines, out int[] numbers)
        {
            numbers = null;
            try
            {
                numbers = lines.Select(int.Parse).ToArray();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    static class ResultPrinter
    {
        // Displays results for each file
        public static void PrintResults(List<FileAnalysisResult> results)
        {
            Console.Clear();
            foreach (var result in results)
            {
                Console.WriteLine($"File: {result.FileName}");
                Console.WriteLine($"Algorithm 1: {result.Algorithm1Result} in {result.Algorithm1Time} ms");
                Console.WriteLine($"Algorithm 2: {result.Algorithm2Result} in {result.Algorithm2Time} ms\n");
            }
        }
    }
}
