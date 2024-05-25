using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

class LABA8
{
    public static byte[] key = { 13, 19, 90, 92, 240 };
    public static string text = "Pluto Erik Valerievich";

    // Линейный конгруэнтный генератор
    public static void LinearCongruentialGenerator(int x, int x1)
    {
        int a = 421, c = 1663, n = 7875;
        x1 = (a * x1 + c) % n;
        if (x1 == x)
            return;
        else
        {
            Console.WriteLine("Сгенерированное число: " + x1);
            LinearCongruentialGenerator(x, x1);
        }
    }

    // Буфер
    public static void Swap(byte[] buff, int i, int j)
    {
        byte temp = buff[i];
        buff[i] = buff[j];
        buff[j] = temp;
    }

    // RC4
    public static byte[] RC4(byte[] input)
    {
        byte[] S = new byte[256];
        for (int k = 0; k < 256; k++)
            S[k] = (byte)k;

        int j = 0;
        for (int k = 0; k < 256; k++)
        {
            j = (j + S[k] + key[k % key.Length]) % 256;
            Swap(S, k, j);
        }

        int i = 0;
        j = 0;
        byte[] output = new byte[input.Length];
        for (int k = 0; k < input.Length; k++)
        {
            i = (i + 1) % 256;
            j = (j + S[i]) % 256;
            Swap(S, i, j);
            output[k] = (byte)(input[k] ^ S[(S[i] + S[j]) % 256]);
        }

        return output;
    }

    static void Main()
    {
        Console.WriteLine(">> 11 ВАРИАНТ");

        Console.WriteLine("\nЗадание 1:");
        LinearCongruentialGenerator(1, 1);

        Console.WriteLine("\nЗадание 2:");
        Stopwatch st = new Stopwatch();
        st.Start();
        byte[] encrypted = RC4(Encoding.ASCII.GetBytes(text));
        st.Stop();
        Console.WriteLine("Зашифрованное сообщение: " + Encoding.ASCII.GetString(encrypted));
        Console.WriteLine("Шифрование заняло: " + st.Elapsed.TotalMilliseconds + "мс");

        st.Restart();
        byte[] decrypted = RC4(encrypted);
        st.Stop();
        Console.WriteLine("Расшифрованное сообщение: " + Encoding.ASCII.GetString(decrypted));
        Console.WriteLine("Расшифрование заняло: " + st.Elapsed.TotalMilliseconds + "мс");
    }
}