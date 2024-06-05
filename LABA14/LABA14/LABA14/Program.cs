using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Program
{
    public static string text = "Erik";
    public static string txt = GetStringFromFile(@"C:\Users\Erik\Desktop\3course\IB\LABA14\Text.txt");

    // Чтение файла
    public static string GetStringFromFile(string path)
    {
        string text;
        using (StreamReader streamReader = new StreamReader(path))
        {
            text = streamReader.ReadToEnd();
        }
        Console.WriteLine(text);
        return text;
    }

    // Сокрытие
    public static string Hide()
    {
        byte[] byteText = Encoding.ASCII.GetBytes(text);
        BitArray bitArray = new BitArray(byteText);
        /*foreach (bool b in bitArray) 
        {
            Console.WriteLine(b ? 1 : 0);
        }*/
        string encrypted = txt;
        int length = txt.Length;
        for (int i = 0, j = 0; i < length && j < bitArray.Length; i++) 
        {
            if (encrypted[i] == '.')
            {
                if (bitArray[j] == true)
                {
                    encrypted = encrypted.Insert(i + 1, " ");
                    i++; j++; length++;
                }
                else
                {
                    j++;
                }
            }
        }
        Console.WriteLine("Текст со скрытым сообщением:\n" + encrypted);
        return encrypted;
    }

    // Извлечение
    public static void GetHiddenMessage(string txt, int messageLength)
    {
        BitArray bitArray = new BitArray(messageLength);

        for (int i = 0, j = 0; i < txt.Length && j < messageLength; i++)
        {
            if (txt[i] == '.')
            {
                if (txt[i + 1] == ' ' && txt[i + 2] == ' ')
                {
                    bitArray.Set(j, true);
                    j++;
                }
                else
                {
                    bitArray.Set(j, false);
                    j++;
                }
            }
        }

        byte[] byteText = new byte[bitArray.Length / 8];

        for (int i = 0; i < bitArray.Length; i++)
        {
            if (bitArray[i])
            {
                int byteIndex = i / 8;
                int bitOffset = i % 8;
                byteText[byteIndex] |= (byte)(1 << bitOffset);
            }
        }

        Console.WriteLine("Извлеченное сообщение: " + Encoding.ASCII.GetString(byteText));
    }

    static void Main(string[] args)
    {
        Console.WriteLine(">>11 ВАРИАНТ");

        Console.WriteLine("\nЗадание 1:");
        string txt2 = Hide();
        GetHiddenMessage(txt2, 4 * 8);
    }
}
