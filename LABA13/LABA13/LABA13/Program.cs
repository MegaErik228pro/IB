using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LABA13
{
    public static string text = "Pluto Erik Valerievich";
    public static string lab11 = "Разработать авторское приложение, которое должно реализовывать один из алгоритмов хеширования семейства MD или SHA.Реализация алгоритма хеширования MD5 представлена на листинге 1.Результат выполнения кода представлен на рисунке 1.Рисунок 1 – Результат хешированияВремя выполнения хеширования сообщения представлено ниже.Рисунок 2 – Время выполнения хешированияВывод:Были приобретены практические навыки разработки и использования приложений для реализации хеширования семейства MD5. Также было замерено время выполнения хеширования, а также построен соответствующий график.";
    public static Bitmap image = new Bitmap("C:\\Users\\Erik\\Desktop\\3course\\IB\\LABA13\\LABA13\\LABA13\\photo.bmp");

    // LSB
    public static string LSB(string texts)
    {
        byte[] text = Encoding.UTF8.GetBytes(texts);
        int charIndex = 0;
        int bitIndex = 0;
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                if (charIndex < text.Length)
                {
                    Color pixel = image.GetPixel(x, y);
                    byte Byte = text[charIndex];
                    int Bit = (Byte >> (7 - bitIndex)) & 0x01;
                    pixel = Color.FromArgb((pixel.R & 0xFE) | Bit, 
                                            pixel.G, 
                                            pixel.B);
                    image.SetPixel(x, y, pixel);
                    bitIndex++;
                    if (bitIndex >= 8)
                    {
                        bitIndex = 0;
                        charIndex++;
                    }
                }
            }
        }
        image.Save("C:\\Users\\Erik\\Desktop\\3course\\IB\\LABA13\\LABA13\\LABA13\\" + texts.Substring(0, 5) + ".bmp");
        return "C:\\Users\\Erik\\Desktop\\3course\\IB\\LABA13\\LABA13\\LABA13\\" + texts.Substring(0, 5) + ".bmp";
    }

    // Получить сообщение LSB
    public static string GetMessageLSB(Bitmap image, int length)
    {
        byte[] hiddenMessage = new byte[length];
        int bitIndex = 0;
        byte Byte = 0;
        int charIndex = 0;
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                Color pixel = image.GetPixel(x, y);
                Byte = (byte)((Byte << 1) | pixel.R & 0x01);
                bitIndex++;
                if (bitIndex >= 8)
                {
                    hiddenMessage[charIndex] = Byte;
                    Byte = 0;
                    bitIndex = 0;
                    charIndex++;
                    if (charIndex >= length)
                        break;
                }
            }
            if (charIndex >= length)
                break;
        }
        return Encoding.UTF8.GetString(hiddenMessage);
    }

    // Цветовая матрица
    public static void ColorMatrix(Bitmap image, string fileName)
    {
        Bitmap colorMatrix = new Bitmap(image.Width, image.Height);
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                Color pixel = image.GetPixel(x, y);
                Color matrixPixel = BitColor(GetBit(pixel.R, pixel.G, pixel.B, 5));
                colorMatrix.SetPixel(x, y, matrixPixel);
            }
        }
        colorMatrix.Save("C:\\Users\\Erik\\Desktop\\3course\\IB\\LABA13\\LABA13\\LABA13\\" + fileName + ".bmp");
    }

    public static int GetBit(int red, int green, int blue, int bitLevel)
    {
        int bitMask = 1 << bitLevel;
        int redBit = (red & bitMask) >> bitLevel;
        int greenBit = (green & bitMask) >> bitLevel;
        int blueBit = (blue & bitMask) >> bitLevel;
        return (redBit << 2) | (greenBit << 1) | blueBit;
    }

    public static Color BitColor(int bitValue)
    {
        int red = (bitValue & 0x04) != 0 ? 255 : 0;
        int green = (bitValue & 0x02) != 0 ? 255 : 0;
        int blue = (bitValue & 0x01) != 0 ? 255 : 0;

        Color color = Color.FromArgb(red, green, blue);
        return color;
    }

    // LSB3
    public static string LSB3(string texts)
    {
        byte[] text = Encoding.UTF8.GetBytes(texts);
        int charIndex = 0;
        int bitIndex = 0;
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                if (charIndex < text.Length)
                {
                    Color pixel = image.GetPixel(x, y);
                    byte Byte = text[charIndex];
                    int redBit = (Byte >> (7 - bitIndex)) & 1;
                    int greenBit = (Byte >> (6 - bitIndex)) & 1;
                    int blueBit = (Byte >> (5 - bitIndex)) & 1;
                    //Console.WriteLine(pixel.R + " - " + ((pixel.R & 0xFE) | redBit));

                    pixel = Color.FromArgb(
                            (pixel.R & 0xFE) | redBit,
                            (pixel.G & 0xFE) | greenBit,
                            (pixel.B & 0xFE) | blueBit
                        );
                    image.SetPixel(x, y, pixel);
                    bitIndex++;
                    if (bitIndex >= 8)
                    {
                        bitIndex = 0;
                        charIndex++;
                    }
                }
            }
        }
        image.Save("C:\\Users\\Erik\\Desktop\\3course\\IB\\LABA13\\LABA13\\LABA13\\" + texts.Substring(0, 5) + "3.bmp");
        return "C:\\Users\\Erik\\Desktop\\3course\\IB\\LABA13\\LABA13\\LABA13\\" + texts.Substring(0, 5) + "3.bmp";
    }

    // Получить сообщение LSB3
    public static string GetMessageLSB3(Bitmap image, int length)
    {
        byte[] hiddenMessage = new byte[length];
        int bitIndex = 0;
        byte Byte = 0;
        int charIndex = 0;
        int i = 0;
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                Color pixel = image.GetPixel(x, y);
                if (i == 0)
                    Byte = (byte)((Byte << 1) | pixel.R & 0x01);
                if (i == 1)
                    Byte = (byte)((Byte << 2) | pixel.G & 0x01);
                if (i == 2)
                    Byte = (byte)((Byte << 3) | pixel.B & 0x01);
                else
                    i = 0;
                bitIndex++;
                if (bitIndex >= 8)
                {
                    hiddenMessage[charIndex] = Byte;
                    Byte = 0;
                    bitIndex = 0;
                    charIndex++;
                    if (charIndex >= length)
                        break;
                }
            }
            if (charIndex >= length)
                break;
        }
        return Encoding.UTF8.GetString(hiddenMessage);
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Задание 1:");
        string textPath = LSB(text);
        string lab11Path = LSB(lab11);
        Bitmap textBitmap = new Bitmap(textPath);
        Bitmap lab11Bitmap = new Bitmap(lab11Path);
        Console.WriteLine("Зашифрованное сообщение(ФИО): " + GetMessageLSB(textBitmap, 22));
        Console.WriteLine("Зашифрованное сообщение(ЛАБА11): " + GetMessageLSB(lab11Bitmap, 1061));
        ColorMatrix(textBitmap, "textColorMatrix");
        ColorMatrix(lab11Bitmap, "lab11ColorMatrix");

        string textPath3 = LSB3(text);
        string lab11Path3 = LSB3(lab11);
        Bitmap textBitmap3 = new Bitmap(textPath3);
        Bitmap lab11Bitmap3 = new Bitmap(lab11Path3);
        Console.WriteLine("Зашифрованное сообщение(ФИО): " + GetMessageLSB3(textBitmap3, 22));
        Console.WriteLine("Зашифрованное сообщение(ЛАБА11): " + GetMessageLSB3(lab11Bitmap3, 1061));
        ColorMatrix(textBitmap3, "textColorMatrix3");
        ColorMatrix(lab11Bitmap, "lab11ColorMatrix3");
    }
}