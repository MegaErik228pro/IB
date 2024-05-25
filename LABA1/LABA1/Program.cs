using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABA1
{
    class Program
    {
        public static string czech = "bcčdďfghchjklmnňpqrřsštťvwxzžaáeéĕiíoóuúůyý";
        public static string belarus = "абвгдеёжзiйклмнопрстуўфхцчшыьэюя";
        public static string binary = "01";

        public static string text3 = GetStringFromFile(@"C:\Users\Erik\Desktop\3course\IB\LABA4\EncryptedTrysemus.txt");

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
                    .Replace("?", "")
                    .ToLower();
            }
            //Console.WriteLine(text);
            return text;
        }

        // Вычисление энтропии
        public static double GetEntropy(string text = "XPZDWEMHZGMNTCADIOET", string lang = "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
        {
            double entropy = 0;
            double[] probability = new double[lang.Length];
            for(int i = 0; i < lang.Length; i++)
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

        //Эффективная энтропия
        public static double GetEffectiveEntropy(double p, string text)
        {
            if (!IsBinary(text)) return 0;
            if (p == 1) return 1;
            double q = 1 - p;
            return 1 - (-1) * (p * Math.Log2(p) + q * Math.Log2(q));
        }

        // Проверка на бинарный алфавит
        static bool IsBinary(string text)
        {
            foreach (char c in text)
            {
                if (c != '0' && c != '1')
                {
                    return false;
                }
            }
            return true;
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Энтропия белорусского алфавита: " + GetEntropy());

            Console.WriteLine("Задание А:");


        }
    }
}