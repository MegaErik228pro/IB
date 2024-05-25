using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABA2
{
    class Program
    {
        public static string czech = "bcčdďfghchjklmnňpqrřsštťvwxzžaáeéĕiíoóuúůyý";
        public static string base64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        public static string binary = "01";
        public static byte[] name = Encoding.UTF8.GetBytes("Erik");
        public static byte[] surname = Encoding.UTF8.GetBytes("Pluto");
        public static byte[] base64Name = Convert.FromBase64String("RXJpaw==");
        public static byte[] base64Surname = Convert.FromBase64String("UGx1dG8=");
        public static string a = GetStringFromFile(@"C:\Users\Erik\Desktop\3course\IB\LABA2\A.txt");

        // Чтение файла
        public static string GetStringFromFile(string path)
        {
            string text;
            using (StreamReader streamReader = new StreamReader(path))
            {
                text = streamReader.ReadToEnd()
                    .Replace(" ", "")
                    .Replace(".", "")
                    .Replace(",", "")
                    .Replace("\n", "")
                    .Replace("\"", "")
                    .Replace("-", "")
                    .Replace("!", "")
                    .Replace("?", "");
            }
            //Console.WriteLine(text);
            return text;
        }

        // Вычисление энтропии Шеннона
        public static double GetShannonEntropy(string text, string lang)
        {
            double entropy = 0;
            double[] probability = new double[lang.Length];
            for (int i = 0; i < lang.Length; i++)
            {
                probability[i] = (double)text.Count(x => x == lang[i]) / (double)text.Length;
                if (probability[i] != 0)
                {
                    //Console.WriteLine(lang[i] + " - " + probability[i]);
                    entropy -= probability[i] * Math.Log2(probability[i]);
                }
            }
            return entropy;
        }

        // Вычисление энтропии Хартли
        public static double GetHartleyEntropy(string lang)
        {
            return Math.Log2(lang.Length);
        }

        // Вычисление избыточности алфавита
        static double GetRedundancy(double ShannonEntropy, double HartleyEntropy)
        {
            return ((HartleyEntropy - ShannonEntropy) / HartleyEntropy) * 100;
        }

        // Запись файла в base64
        static void WriteFileInBase64(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            string encodeText = Convert.ToBase64String(bytes);
            File.WriteAllText(@"C:\Users\Erik\Desktop\3course\IB\LABA2\B.txt", encodeText);
        }

        // XOR
        static byte[] XOR(byte[] buf1, byte[] buf2)
        {
            int maxSize = Math.Max(buf1.Length, buf2.Length);
            Array.Resize(ref buf1, maxSize);
            Array.Resize(ref buf2, maxSize);
            byte[] result = new byte[maxSize];
            for (int i = 0; i < maxSize; i++)
            {
                result[i] = (byte)(buf1[i] ^ buf2[i]);
            }
            return result;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Задание 1:");

            WriteFileInBase64(a);
            Console.WriteLine("Файл A.txt записан в файл B.txt в формате base64.");

            Console.WriteLine("\nЗадание 2:");

            Console.WriteLine("Файл А(латиница):\n\tЭнтропия Шеннона: " + GetShannonEntropy(a, czech) + 
                                               "\n\tЭнтропия Хартли:  " + GetHartleyEntropy(czech)    +
                                               "\n\tИзбыточность:     " + Math.Round(GetRedundancy(GetShannonEntropy(a, czech), GetHartleyEntropy(czech)), 2) + "%");
            string b = GetStringFromFile(@"C:\Users\Erik\Desktop\3course\IB\LABA2\B.txt");
            Console.WriteLine("Файл B(base64):  \n\tЭнтропия Шеннона: " + GetShannonEntropy(b, base64) +
                                               "\n\tЭнтропия Хартли:  " + GetHartleyEntropy(base64) +
                                               "\n\tИзбыточность:     " + Math.Round(GetRedundancy(GetShannonEntropy(b, base64), GetHartleyEntropy(base64)), 2) + "%");

            Console.WriteLine("\nЗадание 3:");
            Console.WriteLine("ASCII:" +
                                "\n\taXORb: "   + Encoding.ASCII.GetString(XOR(name, surname)) + 
                                "\n\taXORbXORb: " + Encoding.ASCII.GetString(XOR(XOR(name, surname), surname)));
            Console.WriteLine("base64:" +
                                "\n\taXORb: " + Convert.ToBase64String(XOR(base64Name, base64Surname)) +
                                "\n\taXORbXORb: " + Convert.ToBase64String(XOR(XOR(base64Name, base64Surname), base64Surname)));
        }
    }
}