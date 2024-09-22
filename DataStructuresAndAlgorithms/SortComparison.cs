using System;
using System.Collections.Generic;

namespace SortingComparison
{
    // Abstract base class for sort algorithms to implement
    internal abstract class SortAlgorithm
    {
        protected int Comparisons { get; private set; }
        protected int Swaps { get; private set; }

        // Resets counters before each sort
        protected void ResetCounters()
        {
            Comparisons = 0;
            Swaps = 0;
        }

        // Increments comparison counter
        protected void IncrementComparisons(int count = 1) => Comparisons += count;

        // Increments swap counter
        protected void IncrementSwaps(int count = 1) => Swaps += count;

        // Abstract method to be implemented by each sorting algorithm
        public int[] Sort(int[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            ResetCounters();
            int[] copy = (int[])array.Clone();
            SortCore(copy);
            return copy;
        }

        protected abstract void SortCore(int[] array);

        public int GetComparisons() => Comparisons;
        public int GetSwaps() => Swaps;
    }

    internal class QuickSort : SortAlgorithm
    {
        protected override void SortCore(int[] arr)
        {
            QuickSortRec(arr, 0, arr.Length - 1);
        }

        private void QuickSortRec(int[] arr, int low, int high)
        {
            IncrementComparisons(); // for base case check
            if (low < high)
            {
                int pi = Partition(arr, low, high);
                QuickSortRec(arr, low, pi);
                QuickSortRec(arr, pi + 1, high);
            }
        }

        private int Partition(int[] arr, int low, int high)
        {
            int pivot = arr[(low + high) / 2];
            int i = low - 1;
            int j = high + 1;

            while (true)
            {
                do { i++; IncrementComparisons(); } while (arr[i] < pivot);
                do { j--; IncrementComparisons(); } while (arr[j] > pivot);

                IncrementComparisons();
                if (i >= j)
                    return j;

                Swap(arr, i, j);
            }
        }

        private void Swap(int[] arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
            IncrementSwaps();
        }
    }

    internal class BubbleSort : SortAlgorithm
    {
        protected override void SortCore(int[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    IncrementComparisons();
                    if (arr[j] > arr[j + 1])
                    {
                        Swap(arr, j, j + 1);
                    }
                }
            }
        }

        private void Swap(int[] arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
            IncrementSwaps();
        }
    }

    internal class MergeSort : SortAlgorithm
    {
        protected override void SortCore(int[] arr)
        {
            MergeSortRec(arr, 0, arr.Length - 1);
        }

        private void MergeSortRec(int[] arr, int left, int right)
        {
            IncrementComparisons(); // base case check
            if (left < right)
            {
                int mid = (left + right) / 2;
                MergeSortRec(arr, left, mid);
                MergeSortRec(arr, mid + 1, right);
                Merge(arr, left, mid, right);
            }
        }

        private void Merge(int[] arr, int left, int mid, int right)
        {
            int leftLen = mid - left + 1;
            int rightLen = right - mid;

            int[] leftArr = new int[leftLen];
            int[] rightArr = new int[rightLen];

            Array.Copy(arr, left, leftArr, 0, leftLen);
            IncrementSwaps(leftLen);

            Array.Copy(arr, mid + 1, rightArr, 0, rightLen);
            IncrementSwaps(rightLen);

            int i = 0, j = 0, k = left;

            while (i < leftLen && j < rightLen)
            {
                IncrementComparisons(2);
                if (leftArr[i] <= rightArr[j])
                {
                    arr[k++] = leftArr[i++];
                    IncrementSwaps();
                }
                else
                {
                    arr[k++] = rightArr[j++];
                    IncrementSwaps();
                }
            }

            while (i < leftLen)
            {
                arr[k++] = leftArr[i++];
                IncrementSwaps();
            }

            while (j < rightLen)
            {
                arr[k++] = rightArr[j++];
                IncrementSwaps();
            }
        }
    }

    internal class InsertionSort : SortAlgorithm
    {
        protected override void SortCore(int[] arr)
        {
            int n = arr.Length;
            for (int i = 1; i < n; i++)
            {
                int key = arr[i];
                IncrementSwaps();
                int j = i - 1;

                IncrementComparisons();
                while (j >= 0 && arr[j] > key)
                {
                    IncrementComparisons();
                    arr[j + 1] = arr[j];
                    IncrementSwaps();
                    j--;
                }
                arr[j + 1] = key;
                IncrementSwaps();
            }
        }
    }

    internal class DataGenerator
    {
        private readonly int size;

        public DataGenerator(int size)
        {
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "Size must be positive");
            this.size = size;
        }

        public int[] GenerateAscending()
        {
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
            {
                arr[i] = i + 1;
            }
            return arr;
        }

        public int[] GenerateDescending()
        {
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
            {
                arr[i] = size - i;
            }
            return arr;
        }

        public int[] GenerateRandomUnique()
        {
            var set = new HashSet<int>();
            var rand = new Random();
            int[] arr = new int[size];

            int index = 0;
            while (index < size)
            {
                int num = rand.Next(1, size + 1);
                if (set.Add(num))
                {
                    arr[index++] = num;
                }
            }
            return arr;
        }
    }

    internal class Program
    {
        private static readonly int ArraySize = 10000;

        private static void Main()
        {
            try
            {
                DisplayWelcomeMessage();

                var generator = new DataGenerator(ArraySize);

                int[] ascending = generator.GenerateAscending();
                int[] descending = generator.GenerateDescending();
                int[] randomUnique = generator.GenerateRandomUnique();

                var sorters = new List<(SortAlgorithm sorter, ConsoleColor color, string name)>
                {
                    (new QuickSort(), ConsoleColor.DarkCyan, "Quick sort"),
                    (new BubbleSort(), ConsoleColor.DarkRed, "Bubble sort"),
                    (new MergeSort(), ConsoleColor.DarkYellow, "Merge sort"),
                    (new InsertionSort(), ConsoleColor.DarkGreen, "Insertion sort")
                };

                foreach (var (sorter, color, name) in sorters)
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine(name);
                    Console.ResetColor();

                    TestSorting(sorter, ascending, "ascending");
                    TestSorting(sorter, descending, "descending");
                    TestSorting(sorter, randomUnique, "random unique");
                    Console.WriteLine();
                }

                DisplayAverageResults(sorters, ascending, descending, randomUnique);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
            }
        }

        private static void DisplayWelcomeMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
            Console.ResetColor();
            Console.WriteLine(" Program to compare 4 sorting algorithms:");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("            1. Quick sort");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("            2. Bubble sort");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("            3. Merge sort");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("            4. Insertion sort");
            Console.ResetColor();
            Console.WriteLine(" Sorting will be compared by number of swaps and comparisons.");
            Console.WriteLine(" Arrays sorted: ascending, descending and random unique values.");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
        }

        private static void TestSorting(SortAlgorithm sorter, int[] data, string dataType)
        {
            int[] sorted = sorter.Sort(data);
            Console.WriteLine($"{dataType} array: swaps = {sorter.GetSwaps()}, comparisons = {sorter.GetComparisons()}");
        }

        private static void DisplayAverageResults(List<(SortAlgorithm sorter, ConsoleColor color, string name)> sorters, int[] ascending, int[] descending, int[] randomUnique)
        {
            Console.WriteLine("Average results for all types of data:");
            foreach (var (sorter, color, name) in sorters)
            {
                int[] resultAsc = sorter.Sort(ascending);
                int swapsAsc = sorter.GetSwaps();
                int compsAsc = sorter.GetComparisons();

                int[] resultDesc = sorter.Sort(descending);
                int swapsDesc = sorter.GetSwaps();
                int compsDesc = sorter.GetComparisons();

                int[] resultRnd = sorter.Sort(randomUnique);
                int swapsRnd = sorter.GetSwaps();
                int compsRnd = sorter.GetComparisons();

                double avgSwaps = (swapsAsc + swapsDesc + swapsRnd) / 3.0;
                double avgComps = (compsAsc + compsDesc + compsRnd) / 3.0;

                Console.ForegroundColor = color;
                Console.WriteLine($"{name}: average swaps = {avgSwaps:F2}, average comparisons = {avgComps:F2}");
                Console.ResetColor();
            }
        }
    }
}
