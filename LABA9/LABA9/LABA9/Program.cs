using NUnit.Framework.Internal;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

class LABA9
{
    public static string text = "Pluto Erik Valerievich";

    // Генерация тайного ключа
    public static int[] GenerateSecretKey(int size)
    {
        Console.WriteLine("Тайный ключ: ");
        int[] key = new int[size];
        int from = 0, to = 1000;
        Random random = new Random();

        for (int i = 0; i < size; i++)
        {
            key[i] = (random.Next(from, to));
            from = key[i];
            to += key[i] * 2;
            Console.Write(Math.Abs(key[i]) + " ");
        }

        return key;
    }

    // Функция для вычисления обратного числа по модулю
    public static int ModInverse(int a, int N)
    {
        N = 8;
        int m0 = N;
        int y = 0, x = 1;

        if (N == 1)
            return 0;

        while (a > 1)
        {
            if (N == 0)
                return 0;
            int q = a / N;
            int t = N;

            N = a % N;
            a = t;
            t = y;

            y = x - q * y;
            x = t;
        }

        if (x < 0)
            x += m0;

        return x;
    }

    // Генерация простого числа
    public static int GeneratePrimeNumber(int n)
    {
        var random = new Random();
        while (true)
        {
            int number = random.Next(1, n);
            if (NOD(number, n) == 1)
            {
                return number;
            }
        }
    }

    // Генерация открытого ключа
    public static int[] GenerateOpenKey(int[] d, int a, int n, int size)
    {
        Console.WriteLine("\nОткрытый ключ: ");
        int[] key = new int[size];
        for (int i = 0; i < size; i++)
        {
            key[i] = (d[i] * a) % n;
            Console.Write(Math.Abs(key[i]) + " ");
        }
        return key;
    }

    // Шифрование
    public static string Encrypt(int[] key, string message)
    {
        Console.WriteLine("\nЗашифрованный текст: ");
        int[] result = new int[message.Length];
        int j = 0;
        StringBuilder encryptedMessage = new StringBuilder();
        foreach (char ch in message)
        {
            int total = 0;
            string binary = "0" + Convert.ToString(ch, 2);
            if (binary.Length == 7)
                binary = "0" + binary;

            encryptedMessage.Append(binary + " ");
            for (int i = 0; i < binary.Length; i++)
            {
                if (binary[i] == '1')
                {
                    total += key[i];
                }
            }
            result[j] = total;
            Console.WriteLine(binary + " - " + Math.Abs(result[j]) + " ");
            j++;
        }
        return encryptedMessage.ToString();
    }

    // Расшифрование
    public static void Decrypt(string ciphertext, int[] secretKey, int n)
    {
        string[] binaryStrings = ciphertext.Split(' ');
        int k = 0;

        foreach (string ch in binaryStrings)
        {

            int total = 0;
            for (int i = 0; i < ch.Length; i++)
            {
                if (ch[i] == '1')
                {
                    total += secretKey[i];
                }
            }

            char decryptedChar = (char)total;

            if (k != 22)
                Console.WriteLine(ch + " - " + text[k] + " ");
            k++;
        }
    }

    // НОД
    public static int NOD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static void Main()
    {
        Console.WriteLine(">> 11 ВАРИАНТ");

        Console.WriteLine("\nЗадание 1:");
        int[] secretKey = GenerateSecretKey(8);
        int sum = 0;
        foreach(int i in secretKey)
        {
            sum += i;
        }
        int[] openKey = GenerateOpenKey(secretKey, GeneratePrimeNumber(sum + 1), sum + 1, 8);
        string encrypted = Encrypt(openKey, text);
        Console.WriteLine("\nРасшифрованный текст: ");
        Decrypt(encrypted, secretKey, 8);
    }
}