using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABA6
{
    class Program
    {
        public static string FIO = "NN"; //PLUTOERIKVALERIEVICH
        public static string alphabet =         "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string rotorLBeta =       "LEYJVCNIXWPBQMDRTAKZGFUHOS";
        public static string rotorMVIII =       "FKQHTLXOCBJSPDZRAMEWNIUYGV";
        public static string rotorRI =          "EKMFLGDQVZNTOWYHXUSPAIBRCJ";
        public static string reflectorBDunn =   "ENKQAUYWJICOPBLMDXZVFTHRGS";

        // Шаг ротора
        static string Step(string message, string alphabet, string rotor, int shift)
        {
            StringBuilder sb = new StringBuilder(message);
            for (int i = 0; i < message.Length; i++) {
                if (alphabet.IndexOf(message[i]) + (i * shift) >= alphabet.Length)
                {
                    sb[i] = rotor[(alphabet.IndexOf(message[i]) + (i * shift)) % (alphabet.Length - 1)];
                }
                else
                    sb[i] = rotor[alphabet.IndexOf(message[i]) + (i * shift)];
            }
            return sb.ToString();
        }

        // Рефлектор
        static string Reflector(string message, string alphabet, string rotor)
        {
            StringBuilder sb = new StringBuilder(message);
            for (int i = 0; i < message.Length; i++)
            {
                sb[i] = rotor[alphabet.IndexOf(message[i])];
            }
            return sb.ToString();
        }

        static void Main(string[] args)
        {
            Console.WriteLine(">> 11 ВАРИАНТ");

            Console.WriteLine("\nЗадание 1:");
            string firstStep = Step(FIO, alphabet, rotorRI, 3); 
            string secondStep = Step(firstStep, alphabet, rotorMVIII, 1);
            string thirdStep = Step(secondStep, alphabet, rotorLBeta, 3);

            string reflector = Reflector(thirdStep, alphabet, reflectorBDunn);

            string firstBStep = Step(reflector, rotorLBeta, alphabet, 3);
            string secondBStep = Step(firstBStep, rotorMVIII, alphabet, 1);
            string thirdBStep = Step(secondBStep, rotorRI, alphabet, 3);

            Console.WriteLine("Исходное сообщение: " + FIO);
            Console.WriteLine("Зашифрованное сообщение " + thirdBStep);
        }
    }
}