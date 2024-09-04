using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace HuffmanApp
{
    internal class Program
    {
        static void Main()
        {
            var app = new HuffmanApp();
            app.Run();
        }
    }

    public class HuffmanApp
    {
        public void Run()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~ Huffman Compression ~~~~~~~~~~~~~~~~~~~~~~~\\");
            Console.ResetColor();

            try
            {
                Console.WriteLine("Choose operation:\n  1 - Encode text\n  2 - Decode text");
                Console.Write("Your choice: ");
                int mode = ReadIntInRange(1, 2);

                string filePath = FileManager.SelectFile();

                if (!File.Exists(filePath))
                    throw new FileNotFoundException("File not found at the specified path.");

                HuffmanTree tree = new HuffmanTree();
                tree.BuildFromFile(filePath);

                if (mode == 1)
                {
                    string encoded = tree.Encode(filePath);
                    Console.WriteLine("\nEncoded text:\n" + encoded);
                    tree.DisplayCompressionRatio(filePath);
                }
                else
                {
                    Console.Write("\nEnter encoded text: ");
                    string encodedText = Console.ReadLine();
                    string decoded = tree.Decode(encodedText);
                    Console.WriteLine("\nDecoded text:\n" + decoded);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + ex.Message);
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private int ReadIntInRange(int min, int max)
        {
            if (!int.TryParse(Console.ReadLine(), out int value) || value < min || value > max)
                throw new ArgumentException($"Please enter a number between {min} and {max}.");
            return value;
        }
    }

    public static class FileManager
    {
        public static string SelectFile()
        {
            Console.WriteLine("\nSelect file input mode:\n  1 - Use 'text.txt' in app folder\n  2 - Enter custom path");
            Console.Write("Your choice: ");
            int choice = int.Parse(Console.ReadLine());

            return choice switch
            {
                1 => "text.txt",
                2 => PromptCustomPath(),
                _ => throw new ArgumentException("Invalid option selected.")
            };
        }

        private static string PromptCustomPath()
        {
            Console.Write("Enter full path to your .txt file: ");
            return Console.ReadLine();
        }
    }

    public class HuffmanTree
    {
        private HuffmanNode root;
        private Dictionary<char, string> codes = new();

        public void BuildFromFile(string filePath)
        {
            int[] frequencies = new int[128];

            using StreamReader reader = new(filePath);
            while (!reader.EndOfStream)
            {
                char ch = (char)reader.Read();
                if (ch < 128) frequencies[ch]++;
            }

            List<HuffmanNode> nodes = CreateLeafNodes(frequencies);
            root = BuildTree(nodes);
            GenerateCodes(root, "");
        }

        public string Encode(string filePath)
        {
            using StreamReader reader = new(filePath);
            string result = "";

            while (!reader.EndOfStream)
            {
                char ch = (char)reader.Read();
                if (codes.TryGetValue(ch, out string code))
                {
                    result += code;
                }
                else
                {
                    throw new Exception($"Character '{ch}' is not encoded.");
                }
            }

            return result;
        }

        public string Decode(string encoded)
        {
            string result = "";
            HuffmanNode node = root;

            foreach (char bit in encoded)
            {
                node = bit == '0' ? node.Left : node.Right;

                if (node.IsLeaf)
                {
                    result += node.Symbol;
                    node = root;
                }
            }

            return result;
        }

        public void DisplayCompressionRatio(string filePath)
        {
            int originalBits = 0;
            int encodedBits = 0;

            using StreamReader reader = new(filePath);
            while (!reader.EndOfStream)
            {
                char ch = (char)reader.Read();
                originalBits += 8;
                encodedBits += codes[ch].Length;
            }

            double ratio = ((double)(originalBits - encodedBits) / originalBits) * 100;

            Console.WriteLine($"\nOriginal size: {originalBits} bits");
            Console.WriteLine($"Encoded size: {encodedBits} bits");
            Console.WriteLine($"Compression: {ratio:F2}%");
        }

        private List<HuffmanNode> CreateLeafNodes(int[] frequencies)
        {
            List<HuffmanNode> nodes = new();
            for (int i = 0; i < frequencies.Length; i++)
            {
                if (frequencies[i] > 0)
                    nodes.Add(new HuffmanNode((char)i, frequencies[i]));
            }
            return nodes;
        }

        private HuffmanNode BuildTree(List<HuffmanNode> nodes)
        {
            while (nodes.Count > 1)
            {
                nodes.Sort((a, b) => a.Frequency.CompareTo(b.Frequency));

                HuffmanNode left = nodes[0];
                HuffmanNode right = nodes[1];

                HuffmanNode parent = new(null, left.Frequency + right.Frequency)
                {
                    Left = left,
                    Right = right
                };

                nodes.RemoveRange(0, 2);
                nodes.Add(parent);
            }

            return nodes[0];
        }

        private void GenerateCodes(HuffmanNode node, string code)
        {
            if (node == null) return;

            if (node.IsLeaf)
            {
                codes[node.Symbol.Value] = code;
                return;
            }

            GenerateCodes(node.Left, code + "0");
            GenerateCodes(node.Right, code + "1");
        }
    }

    public class HuffmanNode
    {
        public char? Symbol { get; }
        public int Frequency { get; }
        public HuffmanNode Left { get; set; }
        public HuffmanNode Right { get; set; }

        public HuffmanNode(char? symbol, int frequency)
        {
            Symbol = symbol;
            Frequency = frequency;
        }

        public bool IsLeaf => Left == null && Right == null;
    }
}
