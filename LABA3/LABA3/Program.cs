using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABA3
{
    class Program
    {
        // НОД
        public static int NOD(int a, int b)
        {
            int temp = int.MaxValue, max = Math.Max(a, b), min = Math.Min(a, b);
            while (temp > 0)
            {
                temp = max % min;
                max = min;
                min = temp;
            }
            return max;
        }

        // Проверка на простое число
        public static bool IsPrime(int a)
        {
            for (int i = 2; i < a; i++)
            {
                if (a % i == 0)
                    return false;
            }
            return true;
        }

        // Поиск простых чисел
        public static void PrimeNumbers(int from, int to)
        {
            int counter = 0;
            for (int i = from; i <= to; i++)
            {
                if (IsPrime(i))
                {
                    Console.Write(i + " ");
                    counter++;
                }
            }
            Console.WriteLine("\nКоличество простых чисел: " + counter);
            Console.Write("n/ln(n): " + Math.Round((to - from) / Math.Log(to - from, 2.72),0));
        }

        // Каноническое разложение
        public static void Canon(int n)
        {
            Console.Write("Каноническое разложение " + n + ": ");
            Dictionary<int, int> factors = new Dictionary<int, int>();
            for (int i = 2; i <= n; i++)
            {
                while (n % i == 0)
                {
                    if (factors.ContainsKey(i))
                    {
                        factors[i]++;
                    }
                    else
                    {
                        factors[i] = 1;
                    }
                    n /= i;
                }
            }
            Console.WriteLine();
            foreach (var factor in factors)
            {
                Console.WriteLine($"{factor.Key}^{factor.Value}");
            }
        }

        // Конкантинация чисел
        public static int Concat(int a, int b)
        {
            string astr = a.ToString();
            string bstr = b.ToString();
            return int.Parse(astr + bstr);
        }

        static void Main(string[] args)
        {
            Console.WriteLine(">> 11 ВАРИАНТ");
            Console.WriteLine("\nЗадание 3:");
            Canon(555);
            Canon(591);

            Console.WriteLine("\nЗадание 4:");
            Console.WriteLine("m || n является простым: " + IsPrime(Concat(555, 591)));

            Console.WriteLine("\nЗадание 7[2-591]:");
            PrimeNumbers(2, 591);

            Console.WriteLine("\n\nЗадание 7[555-591]:");
            PrimeNumbers(555, 591);
        }

    }
}