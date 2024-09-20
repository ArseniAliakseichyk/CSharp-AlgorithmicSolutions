using System;
using System.Collections.Generic;

namespace C1a
{
    // Main application class: manages UI, input, and runs the search demo
    internal class SearchApp
    {
        private const int MaxArraySize = 20000;
        private const int MinArraySize = 1;

        private int[] _array;
        private int _searchSteps;

        public void Run()
        {
            PrintWelcome();

            int size = PromptArraySize();

            var generator = new UniqueRandomArrayGenerator();
            _array = generator.Generate(size);

            var sorter = new MergeSorter();
            sorter.Sort(_array);

            PrintArray(_array);

            int target = PromptSearchNumber();

            var interpSearch = new InterpolationSearch();
            var binarySearch = new BinarySearch();

            interpSearch.ResetSteps();
            int interpIndex = interpSearch.Search(_array, target);

            binarySearch.ResetSteps();
            int binaryIndex = binarySearch.Search(_array, target);

            PrintResults(target, interpIndex, interpSearch.Steps, binarySearch.Steps);
        }

        private void PrintWelcome()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  Witamy w programie do znajdowania liczby za pomocą \n        metody interpolacji binarnej i binarnej");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("  W tym programie możesz podać rozmiar tablicy ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"od {MinArraySize} do {MaxArraySize} ");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("  Na końcu programu znajdziesz wyniki liczby kroków \n  potrzebnych dwóm metodom do znalezienia liczb w tablicy");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/");
            Console.ResetColor();
            Console.WriteLine();
        }

        private int PromptArraySize()
        {
            int size;
            while (true)
            {
                Console.Write($"Podaj rozmiar tablicy ({MinArraySize}-{MaxArraySize}): ");
                if (int.TryParse(Console.ReadLine(), out size) && size >= MinArraySize && size <= MaxArraySize)
                {
                    break;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid input! Please enter an integer between {MinArraySize} and {MaxArraySize}.");
                Console.ResetColor();
            }
            return size;
        }

        private int PromptSearchNumber()
        {
            int num;
            while (true)
            {
                Console.Write("Podaj szukaną liczbę w tablicy: ");
                if (int.TryParse(Console.ReadLine(), out num))
                {
                    return num;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input! Please enter a valid integer.");
                Console.ResetColor();
            }
        }

        private void PrintArray(int[] arr)
        {
            const int columns = 18;
            const int lineLength = 108;

            for (int i = 0; i < arr.Length; i++)
            {
                if (i % columns == 0)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine(new string('-', lineLength));
                    Console.ResetColor();
                }

                string formatted = arr[i] switch
                {
                    < 10 => $"{arr[i]}    ",
                    < 100 => $"{arr[i]}   ",
                    < 1000 => $"{arr[i]}  ",
                    < 10000 => $"{arr[i]} ",
                    _ => $"{arr[i]}"
                };

                Console.Write(formatted);

                if (i < arr.Length - 1)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("|");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine(new string('-', lineLength));
                    Console.ResetColor();
                }
            }
            Console.WriteLine();
        }

        private void PrintResults(int target, int interpIndex, int interpSteps, int binarySteps)
        {
            Console.WriteLine();
            if (interpIndex != -1)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.ResetColor();
                Console.WriteLine();

                Console.Write("Miejsce dla liczby ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(target);
                Console.ResetColor();
                Console.Write(" w tablicy to ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(interpIndex + 1); // +1 for user-friendly indexing
                Console.WriteLine();
                Console.ResetColor();

                Console.Write("Metoda ");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("interpolacji binarna");
                Console.ResetColor();
                Console.Write(" wymagała ");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(interpSteps);
                Console.ResetColor();
                Console.Write(" kroków, a metoda ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("binarna");
                Console.ResetColor();
                Console.Write(" wymagała ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(binarySteps);
                Console.ResetColor();
                Console.Write(" kroków.");
                Console.WriteLine();
            }
            else
            {
                Console.Write("W tabeli brakuje liczby");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($" {target}");
                Console.ResetColor();
                Console.WriteLine();
            }
        }
    }

    // Generates an array with unique random numbers
    internal class UniqueRandomArrayGenerator
    {
        private readonly Random _rand = new Random();

        public int[] Generate(int size)
        {
            var uniqueNumbers = new HashSet<int>();
            int[] result = new int[size];

            int maxRandomValue = size * 4;

            int count = 0;
            while (count < size)
            {
                int num = _rand.Next(0, maxRandomValue + 1);
                if (uniqueNumbers.Add(num))
                {
                    result[count++] = num;
                }
            }
            return result;
        }
    }

    // Classic MergeSort implementation for sorting arrays
    internal class MergeSorter
    {
        public void Sort(int[] arr)
        {
            if (arr == null || arr.Length <= 1)
                return;

            MergeSort(arr, 0, arr.Length - 1);
        }

        private void MergeSort(int[] arr, int left, int right)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;
                MergeSort(arr, left, mid);
                MergeSort(arr, mid + 1, right);
                Merge(arr, left, mid, right);
            }
        }

        private void Merge(int[] arr, int left, int mid, int right)
        {
            int leftSize = mid - left + 1;
            int rightSize = right - mid;

            int[] leftArr = new int[leftSize];
            int[] rightArr = new int[rightSize];

            Array.Copy(arr, left, leftArr, 0, leftSize);
            Array.Copy(arr, mid + 1, rightArr, 0, rightSize);

            int i = 0, j = 0, k = left;

            while (i < leftSize && j < rightSize)
            {
                if (leftArr[i] <= rightArr[j])
                {
                    arr[k++] = leftArr[i++];
                }
                else
                {
                    arr[k++] = rightArr[j++];
                }
            }

            while (i < leftSize)
                arr[k++] = leftArr[i++];

            while (j < rightSize)
                arr[k++] = rightArr[j++];
        }
    }

    // Abstract base class for search algorithms
    internal abstract class SearchAlgorithm
    {
        protected int Steps { get; set; }

        public int StepsCount => Steps;

        public void ResetSteps() => Steps = 0;

        public abstract int Search(int[] arr, int target);
    }

    // Binary search implementation with step counting
    internal class BinarySearch : SearchAlgorithm
    {
        public override int Search(int[] arr, int target)
        {
            return SearchRecursive(arr, 0, arr.Length - 1, target);
        }

        private int SearchRecursive(int[] arr, int left, int right, int target)
        {
            if (left > right)
                return -1;

            Steps++;
            int mid = (left + right) / 2;

            if (arr[mid] == target)
                return mid;

            if (arr[mid] < target)
                return SearchRecursive(arr, mid + 1, right, target);

            return SearchRecursive(arr, left, mid - 1, target);
        }
    }

    // Interpolation search implementation with step counting
    internal class InterpolationSearch : SearchAlgorithm
    {
        public override int Search(int[] arr, int target)
        {
            return SearchRecursive(arr, 0, arr.Length - 1, target);
        }

        private int SearchRecursive(int[] arr, int left, int right, int target)
        {
            if (left <= right && arr[left] <= target && arr[right] >= target)
            {
                Steps++;
                int pos = left + ((target - arr[left]) * (right - left)) / (arr[right] - arr[left]);

                if (pos < left || pos > right)
                    return -1;

                if (arr[pos] == target)
                    return pos;
                else if (arr[pos] < target)
                    return SearchRecursive(arr, pos + 1, right, target);
                else
                    return SearchRecursive(arr, left, pos - 1, target);
            }
            else if (left < arr.Length && arr[left] == target)
            {
                Steps++;
                return left;
            }
            else
            {
                return -1;
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var app = new SearchApp();
                app.Run();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unexpected error: " + ex.Message);
                Console.ResetColor();
            }
        }
    }
}
