using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

class LABA10
{
    public static string text = "Pluto Erik Valerievich";

    // Y
    public static BigInteger GetY(BigInteger a, BigInteger x, BigInteger n)
    {
        BigInteger res = a;
        for (BigInteger i = 1; i < x; i++)
        {
            res *= a;
        }
        // BigInteger.ModPow(a, x, n)
        return res % n;
    }

    // RSA
    public static void RSA()
    {
        RSAParameters publicKey;
        RSAParameters privateKey;
        byte[] openTextBytes = Encoding.UTF8.GetBytes(text);

        using (var rsa = new RSACryptoServiceProvider(4096))
        {
            publicKey = rsa.ExportParameters(false);
            privateKey = rsa.ExportParameters(true);
        }

        using (var rsa = new RSACryptoServiceProvider())
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            rsa.ImportParameters(publicKey);
            var encryptedTextRSA = rsa.Encrypt(openTextBytes, false);
            sw.Stop();

            //Console.WriteLine("Количество символов зашифрованного сообщения RSA: " + Encoding.UTF8.GetString(encryptedTextRSA).Length);

            Console.WriteLine("Зашифрованный текст(RSA): " + string.Join(" ", encryptedTextRSA) + '\n' + "Время выполнения шифрования: " + sw.ElapsedMilliseconds + "мс");

            sw.Restart();
            rsa.ImportParameters(privateKey);
            var decryptedTextRSA = rsa.Decrypt(encryptedTextRSA, false);
            sw.Stop();
            
            Console.WriteLine("Расшифрованный текст(RSA): " + Encoding.UTF8.GetString(decryptedTextRSA) + '\n' + "Время выполнения расшифрования: " + sw.ElapsedMilliseconds + "мс");
        }
    }

    // Эль-Гамаль
    public static void ElGamal(BigInteger p, BigInteger g, BigInteger x)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        BigInteger y = BigInteger.ModPow(g, x, p);
        Random random = new Random();
        BigInteger k;
        do
        {
            byte[] bytes = new byte[p.ToByteArray().Length];
            random.NextBytes(bytes);
            k = new BigInteger(bytes);
        } while (k <= 1 || k >= p - 1);

        BigInteger a = BigInteger.ModPow(g, k, p);
        BigInteger b = BigInteger.ModPow(y, k, p);

        byte[] ciphertext = new byte[2 * text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            ciphertext[2 * i] = (byte)(text[i] ^ (byte)a);
            ciphertext[2 * i + 1] = (byte)(text[i] ^ (byte)b);
        }
        sw.Stop();

        //Console.WriteLine("Количество символов зашифрованного сообщения Эль-Гамаля: " + Encoding.UTF8.GetString(ciphertext).Length);

        Console.WriteLine("Зашифрованный текст(Эль-Гамаль): " + Encoding.ASCII.GetString(ciphertext) + '\n' + "Время выполнения шифрования: " + sw.Elapsed.TotalMilliseconds + "мс");

        sw.Restart();
        byte[] encrypted = new byte[ciphertext.Length / 2];
        for (int i = 0; i < encrypted.Length; i++)
        {
            BigInteger n = new BigInteger(ciphertext[2 * i]);
            BigInteger m = new BigInteger(ciphertext[2 * i + 1]);

            encrypted[i] = (byte)(n ^ m ^ x);
        }
        sw.Stop();

        Console.WriteLine("Расшифрованный текст(Эль-Гамаль): " + text + '\n' + "Время выполнения расшифрования: " + sw.Elapsed.TotalMilliseconds + "мс");
    }

    static void Main()
    {
        Console.WriteLine("\nЗадание 1:");
        Stopwatch sw = new Stopwatch();
        sw.Start();
        BigInteger Y = GetY(15, 1009, 309);
        sw.Stop();
        Console.WriteLine("a=15, x=1009, n=309 >> Y=" + Y + ". Выполнено за " + sw.ElapsedMilliseconds + "мс");
        sw.Restart();
        Y = GetY(15, 10111, 309);
        sw.Stop();
        Console.WriteLine("a=15, x=10111, n=309 >> Y=" + Y + ". Выполнено за " + sw.ElapsedMilliseconds + "мс");
        sw.Restart();
        Y = GetY(15, 1123, 309);
        sw.Stop();
        Console.WriteLine("a=15, x=1123, n=309 >> Y=" + Y + ". Выполнено за " + sw.ElapsedMilliseconds + "мс");
        sw.Restart();
        Y = GetY(15, 1621, 309);
        sw.Stop();
        Console.WriteLine("a=15, x=1621, n=309 >> Y=" + Y + ". Выполнено за " + sw.ElapsedMilliseconds + "мс");
        sw.Restart();
        Y = GetY(15, 10133, 309);
        sw.Stop();
        Console.WriteLine("a=15, x=10133, n=309 >> Y=" + Y + ". Выполнено за " + sw.ElapsedMilliseconds + "мс");

        Console.WriteLine("\nЗадание 2:");
        RSA();
        ElGamal(29, 7, 10);
    }
}