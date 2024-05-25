using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

class LABA11
{
    public static string text = "Pluto Erik Valerievich";

    // MD5
    public static string GetMD5Hash()
    {
        byte[] textBytes = Encoding.ASCII.GetBytes(text);

        using (MD5 md5 = MD5.Create())
        {
            byte[] hashBytes = md5.ComputeHash(textBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    static void Main()
    {
        Console.WriteLine("Задание 1:");
        Stopwatch sw = new Stopwatch();
        sw.Start();
        string MD5Hash = GetMD5Hash();
        sw.Stop();
        Console.WriteLine("MD5 хеш для " + text + ": " + MD5Hash + "\nВыполнено за " + sw.ElapsedMilliseconds + "мс");
    }
}