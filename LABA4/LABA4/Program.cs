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
        public static string text1 = GetStringFromFile(@"C:\Users\Erik\Desktop\3course\IB\LABA4\Text1.txt");

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

        // Создание нового альфавита используя шифр цезаря с ключевым словом
        public static string GetAlphabetCeasarWithKeyWord(string alphabet, string keyWord, int position)
        {
            position--;
            char[] encryptAlphabet = new char[alphabet.Length];
            bool skip = false;
            int symbol = 0;

            // Вставляем ключевое слово
            int insertPosition = position;
            for (int i = position; i < keyWord.Length + position; i++)
            {
                for (int j = position; j < i; j++)
                {
                    if (keyWord[i - position] == keyWord[j - position])
                    {
                        skip = true;
                        break;
                    }
                }
                if (!skip)
                {
                    //Console.WriteLine(insertPosition + " - " + keyWord[i - position]);
                    encryptAlphabet[insertPosition] = keyWord[i - position];
                    insertPosition++;
                }
                else
                    skip = false;
            }

            // Вставляем символы после ключевого слова
            for (int i = 0; i < alphabet.Length; i++)
            {
                for (int j = position; j < keyWord.Length + position; j++)
                {
                    if (alphabet[i] == encryptAlphabet[j])
                    {
                        skip = true;
                        break;
                    }
                }
                if (!skip)
                {
                    if (insertPosition == encryptAlphabet.Length)
                    {
                        symbol = i;
                        break;
                    }
                    //Console.WriteLine(insertPosition + " - " + alphabet[i]);
                    encryptAlphabet[insertPosition] = alphabet[i];
                    insertPosition++;
                }
                else
                    skip = false;
            }

            // Вставляем символы до ключевого слова
            insertPosition = 0;
            for (int i = symbol; i < alphabet.Length; i++)
            {
                for (int j = position; j < keyWord.Length + position; j++)
                {
                    if (alphabet[i] == encryptAlphabet[j])
                    {
                        skip = true;
                        break;
                    }
                }
                if (!skip)
                {
                    if (insertPosition == position)
                        break;
                    //Console.WriteLine(insertPosition + " - " + alphabet[i]);
                    encryptAlphabet[insertPosition] = alphabet[i];
                    insertPosition++;
                }
                else
                    skip = false;
            }

            return String.Concat<char>(encryptAlphabet);
        }

        // Шифрование файла с помощью шифра Цезаря
        public static void EncryptFileCeasar(string text, string alphabet)
        {
            string encryptedText = "";
            foreach (char symbol in text)
            {
                if (belarus.Contains(symbol))
                    encryptedText += alphabet[belarus.IndexOf(symbol)];
                else
                    encryptedText += symbol;
            }
            File.WriteAllText(@"C:\Users\Erik\Desktop\3course\IB\LABA4\EncryptedCaesar.txt", encryptedText);
        }

        // Расшифрование файла с помощью шифра Цезаря
        public static void DecryptFileCeasar(string path, string alphabet)
        {
            string text = GetStringFromFile(path);
            string decryptedText = "";
            foreach (char symbol in text)
            {
                if (alphabet.Contains(symbol))
                    decryptedText += belarus[alphabet.IndexOf(symbol)];
                else
                    decryptedText += symbol;
            }
            File.WriteAllText(@"C:\Users\Erik\Desktop\3course\IB\LABA4\DecryptedCaesar.txt", decryptedText);
        }

        // Создание таблицы Трисемуса
        public static char[,] CreateTable(string alphabet, int n)
        {
            char[,] table = new char[alphabet.Length/n, n];
            int position = 0;
            for (int i = 0; i < alphabet.Length/n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    table[i,j] = alphabet[position];
                    position++;
                }
            }
            return table;
        }

        // Шифрование файла с помощью таблицы Трисемуса
        public static void EncryptFileTrysemus(string text, char[,] table)
        {
            string encryptedText = "";
            foreach (char symbol in text)
            {
                if (belarus.Contains(symbol))
                {
                    for (int i = 0; i < table.GetUpperBound(0) + 1; i++)
                    {
                        for (int j = 0; j < table.Length / (table.GetUpperBound(0) + 1); j++)
                        {
                            if (table[i,j] == symbol)
                            {
                                if (i == table.GetUpperBound(0))
                                    encryptedText += table[0, j];
                                else
                                    encryptedText += table[i + 1,j];
                            }
                        }
                    }

                }
                else
                    encryptedText += symbol;
            }
            File.WriteAllText(@"C:\Users\Erik\Desktop\3course\IB\LABA4\EncryptedTrysemus.txt", encryptedText);
        }

        // Расшифрование файла с помощью таблицы Трисемуса
        public static void DecryptFileTrysemus(string path, char[,] table)
        {
            string text = GetStringFromFile(path);
            string decryptedText = "";
            foreach (char symbol in text)
            {
                if (belarus.Contains(symbol))
                {
                    for (int i = 0; i < table.GetUpperBound(0) + 1; i++)
                    {
                        for (int j = 0; j < table.Length / (table.GetUpperBound(0) + 1); j++)
                        {
                            if (table[i, j] == symbol)
                            {
                                if (i == 0)
                                    decryptedText += table[table.GetUpperBound(0), j];
                                else
                                    decryptedText += table[i - 1, j];
                            }
                        }
                    }

                }
                else
                    decryptedText += symbol;
            }
            File.WriteAllText(@"C:\Users\Erik\Desktop\3course\IB\LABA4\DecryptedTrysemus.txt", decryptedText);
        }

        static void Main(string[] args)
        {
            Console.WriteLine(">> 11 ВАРИАНТ");

            Console.WriteLine("\nЗадание 1:");

            Console.WriteLine("Алфавиты:\n" + belarus);
            Console.WriteLine(GetAlphabetCeasarWithKeyWord(belarus, "iнфарматыка", 2));
            var sw = new Stopwatch();
            sw.Start();
            EncryptFileCeasar(text1, GetAlphabetCeasarWithKeyWord(belarus, "iнфарматыка", 2));
            sw.Stop();
            Console.WriteLine("Файл Text1.txt зашифрован в файл EncryptedCaesar.txt. Выполнено за " + sw.ElapsedMilliseconds + "мс");
            sw.Restart();
            DecryptFileCeasar(@"C:\Users\Erik\Desktop\3course\IB\LABA4\EncryptedCaesar.txt", GetAlphabetCeasarWithKeyWord(belarus, "iнфарматыка", 2));
            sw.Stop();
            Console.WriteLine("Файл EncryptedCaesar.txt расшифрован в файл DecryptedCaesar.txt. Выполнено за " + sw.ElapsedMilliseconds + "мс");

            Console.WriteLine("\nЗадание 2:");

            Console.WriteLine("Алфавиты:\n" + belarus);
            Console.WriteLine(GetAlphabetCeasarWithKeyWord(belarus, "эрык", 1));
            Console.WriteLine("Таблица Трисемуса:");
            char[,] table = CreateTable(GetAlphabetCeasarWithKeyWord(belarus, "эрык", 1), 8);
            for (int i = 0; i < table.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < table.Length / (table.GetUpperBound(0) + 1); j++)
                {
                    Console.Write(table[i,j]);
                }
                Console.WriteLine();
            }
            sw.Restart();
            EncryptFileTrysemus(text1, table);
            sw.Stop();
            Console.WriteLine("Файл Text1.txt зашифрован в файл EncryptedTrysemus.txt. Выполнено за " + sw.ElapsedMilliseconds + "мс");
            sw.Restart();
            DecryptFileTrysemus(@"C:\Users\Erik\Desktop\3course\IB\LABA4\EncryptedTrysemus.txt", table);
            sw.Stop();
            Console.WriteLine("Файл EncryptedTrysemus.txt расшифрован в файл DecryptedTrysemus.txt. Выполнено за " + sw.ElapsedMilliseconds + "мс");
        }
    }
}