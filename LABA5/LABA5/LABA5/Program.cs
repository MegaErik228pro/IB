using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABA4
{
    class Program
    {
        public static string belarus = "абвгдеёжзiйклмнопрстуўфхцчшыьэюя";
        public static string text1 = GetStringFromFile(@"C:\Users\Erik\Desktop\3course\IB\LABA5\Text1.txt");

        // Чтение файла
        public static string GetStringFromFile(string path)
        {
            string text;
            using (StreamReader streamReader = new StreamReader(path))
            {
                text = streamReader.ReadToEnd()
                    .ToLower();
            }
            return text;
        }

        // Создание таблицы
        public static char[,] CreateTable(string alphabet, int n)
        {
            if (n <= 5)
                throw new Exception("Минимальный размер таблицы 6x6.");
            char[,] table = new char[n, n];
            int position = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (position == alphabet.Length)
                    {
                        if (i == j)
                            table[i, j] = table[0, 0];
                        table[i, j] = table[j, i];
                    }
                    else
                    {
                        table[i, j] = alphabet[position];
                        position++;
                    }
                }
            }
            return table;
        }

        // Шифрование файла с помощью таблицы 
        public static void EncryptFile(string text, char[,] table)
        {
            string encryptedText = "";
            bool isFind = false;
            foreach (char symbol in text)
            {
                if (belarus.Contains(symbol))
                {
                    for (int i = 0; i < table.GetUpperBound(0) + 1; i++)
                    {
                        if (isFind)
                            break;
                        for (int j = 0; j < table.GetUpperBound(0) + 1; j++)
                        {
                            if (isFind)
                                break;
                            if (table[i, j] == symbol)
                            {
                                encryptedText += table[j, i];
                                isFind = true;
                            }
                        }
                    }
                    isFind = false;
                }
                else
                    encryptedText += symbol;
            }
            File.WriteAllText(@"C:\Users\Erik\Desktop\3course\IB\LABA5\Encrypted.txt", encryptedText);
        }

        // Расшифрование файла с помощью таблицы
        public static void DecryptFile(string path, char[,] table)
        {
            string text = GetStringFromFile(path);
            string decryptedText = "";
            bool isFind = false;
            foreach (char symbol in text)
            {
                if (belarus.Contains(symbol))
                {
                    for (int i = 0; i < table.GetUpperBound(0) + 1; i++)
                    {
                        if (isFind)
                            break;
                        for (int j = 0; j < table.GetUpperBound(0) + 1; j++)
                        {
                            if (isFind)
                                break;
                            if (table[i, j] == symbol)
                            {
                                decryptedText += table[j, i];
                                isFind = true;
                            }
                        }
                    }
                    isFind = false;
                }
                else
                    decryptedText += symbol;
            }
            File.WriteAllText(@"C:\Users\Erik\Desktop\3course\IB\LABA5\Decrypted.txt", decryptedText);
        }

        // Шифрование множественной перестановкой
        public static string MultiplePermutationEncrypt(string text, string key1, string key2)
        {
            int[] X = GetNumbers(key1, belarus);
            int[] Y = GetNumbers(key2, belarus);

            Console.Write("X: ");
            for (int i = 0; i < X.Length; i++)
            {
                Console.Write(X[i]);
            }
            Console.WriteLine();

            Console.Write("Y: ");
            for (int i = 0; i < Y.Length; i++)
            {
                Console.Write(Y[i]);
            }
            Console.WriteLine();

            char[,] textTable = new char[key1.Length, key2.Length];

            int position = 0;
            for (int i = 0; i < key1.Length; i++)
            {
                for (int j = 0; j < key2.Length; j++)
                {
                    textTable[i, j] = text[position];
                    Console.Write(textTable[i, j] + "\t");
                    position++;
                }
                Console.WriteLine();
            }

            char[] buff = new char[int.Max(key1.Length, key2.Length)];
            int buffInt = 0;

            for (int i = 0; i < X.Length; i++) 
            { 
                if (X[i] != i + 1)
                {
                    for (int j = 0; j < Y.Length; j++)
                    {
                        buff[j] = textTable[i, j];
                        textTable[i, j] = textTable[X[i] - 1, j];
                        textTable[X[i] - 1, j] = buff[j];
                    }
                    buffInt = X[i];
                    X[i] = i + 1;
                    X[buffInt - 1] = buffInt;
                }
            }

            Console.WriteLine("Строки: ");
            for (int i = 0; i < X.Length; i++)
            {
                for (int j = 0; j < Y.Length; j++)
                {
                    Console.Write(textTable[i,j] + "\t");
                }
                Console.WriteLine();
            }

            for (int i = 0; i < Y.Length; i++)
            {
                if (Y[i] != i + 1)
                {
                    for (int j = 0; j < X.Length; j++)
                    {
                        buff[j] = textTable[j, i];
                        textTable[j, i] = textTable[j, Y[i] - 1];
                        textTable[j, Y[i] - 1] = buff[j];
                    }
                    buffInt = Y[i];
                    Y[i] = i + 1;
                    Y[buffInt - 1] = buffInt;
                }
            }

            Console.WriteLine("Столбцы: ");
            for (int i = 0; i < X.Length; i++)
            {
                for (int j = 0; j < Y.Length; j++)
                {
                    Console.Write(textTable[i, j] + "\t");
                }
                Console.WriteLine();
            }

            string result = "";
            for (int i = 0; i < X.Length; i++)
            {
                for (int j = 0; j < Y.Length; j++)
                {
                    result += textTable[i, j];
                }
            }

            return result;
        }

        // Расшифрование множественной перестановкой
        public static string MultiplePermutationDecrypt(string text, string key1, string key2)
        {
            int[] X = GetNumbers(key1, belarus);
            int[] Y = GetNumbers(key2, belarus);

            Console.Write("X: ");
            for (int i = 0; i < X.Length; i++)
            {
                Console.Write(X[i]);
            }
            Console.WriteLine();

            Console.Write("Y: ");
            for (int i = 0; i < Y.Length; i++)
            {
                Console.Write(Y[i]);
            }
            Console.WriteLine();

            char[,] textTable = new char[key1.Length, key2.Length];

            int position = 0;
            for (int i = 0; i < key1.Length; i++)
            {
                for (int j = 0; j < key2.Length; j++)
                {
                    textTable[i, j] = text[position];
                    Console.Write(textTable[i, j] + "\t");
                    position++;
                }
                Console.WriteLine();
            }

            char[] buff = new char[int.Max(key1.Length, key2.Length)];
            int buffInt = 0;
            int[] ints = new int[] { 1, 2, 3, 4, 5};
            for (int i = 0; i < Y.Length; i++)
            {
                if (ints[i] != Y[i])
                {
                    for (int j = 0; j < X.Length; j++)
                    {
                        buff[j] = textTable[j, ints[i] - 1];                  // 1 2 3 4 5 - 1        3 1 5 4 2
                        textTable[j, ints[i] - 1] = textTable[j, Y[i] - 1];   // 3 2 3 4 5 - 1
                        textTable[j, Y[i] - 1] = buff[j];                     // 3 2 1 4 5
                    }
                    buffInt = ints[i];
                    ints[i] = Y[i];                                         // 1 2 3 4 5 - 1
                    ints[Y[i] - 1] = buffInt;                                   // 3 2 3 4 5 - 1
                    //Y[buffInt - 1] = buffInt;                               // 3 2 1 4 5

                }
            }
            for (int j = 0; j < X.Length; j++)
            {
                buff[j] = textTable[j, 1];      
                textTable[j, 1] = textTable[j, 2]; 
                textTable[j, 2] = buff[j];    
            }

            for (int i = 0; i < X.Length; i++)
            {
                if (X[i] != i + 1)
                {
                    for (int j = 0; j < Y.Length; j++)
                    {
                        buff[j] = textTable[i, j];
                        textTable[i, j] = textTable[X[i] - 1, j];
                        textTable[X[i] - 1, j] = buff[j];
                    }
                    buffInt = X[i];
                    X[i] = i + 1;
                    X[buffInt - 1] = buffInt;
                }
            }

            string result = "";
            for (int i = 0; i < X.Length; i++)
            {
                for (int j = 0; j < Y.Length; j++)
                {
                    result += textTable[i, j];
                }
            }

            char[,] resultTable = textTable;
            position = 0;
            for (int i = 0; i < key1.Length; i++)
            {
                for (int j = 0; j < key2.Length; j++)
                {
                    if (j == key2.Length - 1)
                        break;
                    resultTable[j, i] = result[position];
                    position++;
                }
            }

            Console.WriteLine("Расшифрованная таблица: ");
            for (int i = 0; i < X.Length; i++)
            {
                for (int j = 0; j < Y.Length; j++)
                {
                    Console.Write(resultTable[i, j] + "\t");
                }
                Console.WriteLine();
            }


            return result;
        }

        // Получение индексов букв
        public static int[] GetNumbers(string key, string alphabet)
        {
            int[] resNum = new int[key.Length];
            for (int i = 0; i < key.Length; i++)
            {
                for (int position = 0; position < alphabet.Length; position++)
                {
                    if (key[i] == alphabet[position])
                    {
                        resNum[i] = position;
                        break;
                    }
                }
            }
            int[] res = new int[key.Length];
            for (int num = 1; num <= key.Length; num++)
            {
                for (int i = 0; i < key.Length; i++)
                {
                    if (resNum[i] == resNum.Min())
                    {
                        res[i] = num;
                        resNum[i] = int.MaxValue;
                        break;
                    }
                }
                
            }
            return res;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(">> 11 ВАРИАНТ");

            Console.WriteLine("\nЗадание 1:");

            Console.WriteLine("Таблица:");
            char[,] table = CreateTable(belarus, 6);
            for (int i = 0; i < table.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < table.Length / (table.GetUpperBound(0) + 1); j++)
                {
                    Console.Write(table[i, j] + "\t");
                }
                Console.WriteLine();
            }
            var sw = new Stopwatch();
            sw.Start();
            EncryptFile(text1, table);
            sw.Stop();
            Console.WriteLine("Файл Text1.txt зашифрован в файл Encrypted.txt. Выполнено за " + sw.ElapsedMilliseconds + "мс");
            sw.Restart();
            DecryptFile(@"C:\Users\Erik\Desktop\3course\IB\LABA5\Encrypted.txt", table);
            sw.Stop();
            Console.WriteLine("Файл Encrypted.txt расшифрован в файл Decrypted.txt. Выполнено за " + sw.ElapsedMilliseconds + "мс");

            Console.WriteLine("\nЗадание 2:");
            sw.Restart();
            Console.WriteLine("Шифрование множественной перестановкой: " + MultiplePermutationEncrypt("жыўсабенасьвецебедны", "эрык", "плюто"));
            sw.Stop();
            Console.WriteLine("Шифрование выполнено за " + sw.ElapsedMilliseconds + "мс");
            sw.Restart();
            Console.WriteLine("Расшифрование множественной перестановкой: " + MultiplePermutationDecrypt("еыбндесбанвеьцеыажсў", "эрык", "плюто"));
            sw.Stop();
            Console.WriteLine("Расшифрование выполнено за " + sw.ElapsedMilliseconds + "мс");
        }
    }
}