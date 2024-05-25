using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

class LABA7
{
    public static string FIO = "PLUTOERI";
    public static string text = "Pluto Erik Valerievich";

    // Шифрование
    public static byte[] EncryptTextToMemory(string text, byte[] key, byte[] iv)
    {
        using (MemoryStream mStream = new MemoryStream())
        {
            using (DES des = DES.Create())
            using (ICryptoTransform encryptor = des.CreateEncryptor(key, iv))
            using (var cStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write))
            {
                byte[] toEncrypt = Encoding.ASCII.GetBytes(text);

                cStream.Write(toEncrypt, 0, toEncrypt.Length);
            }
            byte[] ret = mStream.ToArray();
            return ret;
        }
    }

    // Расшифрование
    public static string DecryptTextFromMemory(byte[] encrypted, byte[] key, byte[] iv)
    {
        byte[] decrypted = new byte[encrypted.Length];
        int offset = 0;

        using (MemoryStream mStream = new MemoryStream(encrypted))
        {
            using (DES des = DES.Create())
            using (ICryptoTransform decryptor = des.CreateDecryptor(key, iv))
            using (var cStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read))
            {
                int read = 1;

                while (read > 0)
                {
                    read = cStream.Read(decrypted, offset, decrypted.Length - offset);
                    offset += read;
                }
            }
        }
        return Encoding.ASCII.GetString(decrypted, 0, offset);
    }

    // Разбитие на блоки
    public static List<string> MakeBlocks(string text, int blockSize)
    {
        List<string> blocks = new List<string>();
        for (int i = 0; i < text.Length; i += blockSize)
        {
            blockSize = Math.Min(blockSize, text.Length - i);
            string block = text.Substring(i, blockSize);
            block = block.PadRight(8, ' ');
            blocks.Add(block);
        }
        return blocks;
    }

    // Лавинный эффект
    public static int ChangedBites(byte[] originalBytes, byte[] modifiedBytes)
    {
        int changedBites = 0;

        for (int i = 0; i < originalBytes.Length; i++)
        {
            if (originalBytes[i] != modifiedBytes[i])
            {
                changedBites++;
            }
        }
        return changedBites;
    }

    // Сжатие
    public static byte[] Compress(byte[] data)
    {
        using (var compressedStream = new MemoryStream())
        using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
        {
            zipStream.Write(data, 0, data.Length);
            zipStream.Close();
            return compressedStream.ToArray();
        }
    }

    // Шифрование/расшифрование
    public static void EncDec(string myKey)
    {
        byte[] key;
        byte[] iv;

        //myKey = "01010101";

        using (DES des = DES.Create())
        {
            key = ASCIIEncoding.ASCII.GetBytes(myKey.ToArray<char>());
            iv = des.IV;
        }

        Stopwatch st = new Stopwatch();

        st.Start();
        byte[] encrypted = EncryptTextToMemory(text, key, iv);
        st.Stop();
        Console.WriteLine("Зашифрованная строка: " + Convert.ToBase64String(encrypted));
        Console.WriteLine("Шифрование заняло: " + st.ElapsedMilliseconds + "мс");

        //string newKey = "00001111";
        key = ASCIIEncoding.ASCII.GetBytes(myKey.ToArray<char>());

        st.Restart();
        string decrypted = DecryptTextFromMemory(encrypted, key, iv);
        st.Stop();
        Console.WriteLine("Расшифрованная строка: " + decrypted);
        Console.WriteLine("Расшифрование заняло: " + st.ElapsedMilliseconds + "мс");
        Console.WriteLine("Лавинный эффект: " + ChangedBites(Encoding.ASCII.GetBytes(text), encrypted));
    }

    // Возвращение зашифрованного сообщения
    public static byte[] EncRes(string myKey)
    {
        byte[] key;
        byte[] iv;

        using (DES des = DES.Create())
        {
            key = ASCIIEncoding.ASCII.GetBytes(myKey.ToArray<char>());
            iv = des.IV;
        }

        Stopwatch st = new Stopwatch();

        byte[] encrypted = EncryptTextToMemory(text, key, iv);

        return encrypted;
    }

    static void Main()
    {
        Console.WriteLine(">> 11 ВАРИАНТ");

        //EncDec("01010101");

        Console.WriteLine("\nЗадание 1:");
        EncDec(FIO);

        Console.WriteLine("\nЗадание 2:");
        Console.WriteLine("Слабый ключ: ");
        EncDec("01010101");
        Console.WriteLine("\nПолуслабый ключ: ");
        EncDec("F1FEF1FE");

        Console.WriteLine("\nЗадание 3:");
        Console.WriteLine("Степень сжатия исходного текста: " + ((double)Compress(ASCIIEncoding.ASCII.GetBytes(FIO)).Length / (ASCIIEncoding.ASCII.GetBytes(FIO)).Length));
        Console.WriteLine("Степень сжатия зашифрованного текста: " + ((double)Compress(EncRes(FIO)).Length / EncRes(FIO).Length));
    }
}