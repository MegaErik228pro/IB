using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

class LABA12
{
    public static string text = "Pluto Erik Valerievich";

    // MD5
    public static byte[] GetMD5Hash()
    {
        byte[] textBytes = Encoding.ASCII.GetBytes(text);

        using (MD5 md5 = MD5.Create())
        {
            return md5.ComputeHash(textBytes);
        }
    }

    // Создание RSA подписи
    public static byte[] CreateRSASignature(RSAParameters privateKey)
    {
        using (var rsa = RSA.Create())
        {
            rsa.ImportParameters(privateKey);
            byte[] hash = GetMD5Hash();
            return rsa.SignHash(hash, HashAlgorithmName.MD5, RSASignaturePadding.Pkcs1);
        }
    }

    // Проверка RSA подписи
    public static bool VerifyRSASignature(byte[] signature, RSAParameters publicKey)
    {
        using (var rsa = RSA.Create())
        {
            rsa.ImportParameters(publicKey);
            byte[] hash = GetMD5Hash();
            return rsa.VerifyHash(hash, signature, HashAlgorithmName.MD5, RSASignaturePadding.Pkcs1);
        }
    }

    // Создание Эль-Гамаль подписи
    public static byte[] GenerateElGamalSignature(int p, int g, int x, int k, int h)
    {
        int m = p - 1;
        int k_inverse = 1;
        while (k_inverse * k % m != 1)
        {
            k_inverse++;
        }
        int a = (int)BigInteger.ModPow(g, k, p);
        var b = new BigInteger((k_inverse * (h - (x * a) % m) % m) % m);
        byte[] signature = new byte[2 * sizeof(int)];
        Buffer.BlockCopy(BitConverter.GetBytes(a), 0, signature, 0, sizeof(int));
        Buffer.BlockCopy(BitConverter.GetBytes((int)b), 0, signature, sizeof(int), sizeof(int));
        return signature;
    }

    // Проверка Эль-Гамаль подписи
    public static bool VerifyElGamalSignature(int p, int g, int y, int h, byte[] signature)
    {
        int a = BitConverter.ToInt32(signature, 0);
        int b = BitConverter.ToInt32(signature, sizeof(int));

        var hash1 = BigInteger.ModPow(BigInteger.Pow(y, a) * BigInteger.Pow(a, b), 1, p);
        var hash2 = BigInteger.ModPow(g, h, p);

        return hash1 == hash2;
    }

    static void Main()
    {
        Console.WriteLine("Задание 1:");
        Console.WriteLine("RSA:");
        bool isValid;
        byte[] signature;
        Stopwatch sw = new Stopwatch();
        using (var rsa = new RSACryptoServiceProvider(4096))
        {
            RSAParameters privateKey = rsa.ExportParameters(true);
            RSAParameters publicKey = rsa.ExportParameters(false);

            sw.Start();
            signature = CreateRSASignature(privateKey);
            sw.Stop();
            Console.WriteLine("Подпись создана за " + sw.ElapsedMilliseconds + "мс");
            sw.Restart();
            isValid = VerifyRSASignature(signature, publicKey);
            sw.Stop();
            Console.WriteLine("Подпись проверена за " + sw.ElapsedMilliseconds + "мс");

            if (isValid)
                Console.WriteLine("Подпись верифицированна");
            else
                Console.WriteLine("Подпись не верифицированна");
        }

        Console.WriteLine("Эль-Гамаль:");
        int p = 23, g = 5, x = 6, k = 15, h = 11;

        sw.Start();
        signature = GenerateElGamalSignature(p, g, x, k, h);
        sw.Stop();
        Console.WriteLine("Подпись создана за " + sw.Elapsed.TotalMilliseconds + "мс");
        int y = (int)BigInteger.ModPow(g, x, p);
        sw.Restart();
        isValid = VerifyElGamalSignature(p, g, y, h, signature);
        sw.Stop();
        Console.WriteLine("Подпись проверена за " + sw.Elapsed.TotalMilliseconds + "мс");

        if (isValid)
            Console.WriteLine("Подпись верифицированна");
        else
            Console.WriteLine("Подпись не верифицированна");

        Console.WriteLine("Шнорр:");
        using (var schnorr = new ECDsaCng())
        {
            sw.Restart();
            signature = schnorr.SignData(GetMD5Hash());
            sw.Stop();
            Console.WriteLine("Подпись создана за " + sw.ElapsedMilliseconds + "мс");
            sw.Restart();
            isValid = schnorr.VerifyData(GetMD5Hash(), signature);
            sw.Stop();
            Console.WriteLine("Подпись проверена за " + sw.ElapsedMilliseconds + "мс");
            if (isValid)
                Console.WriteLine("Подпись верифицированна");
            else
                Console.WriteLine("Подпись не верифицированна");
        }
    }
}