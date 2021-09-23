using System;

namespace Kalkulatschka
{
    class Program
    {
        static double calculate(double a, double b, char op)
        {
            switch (op)
            {
                case '+':
                    return a + b;
                case '-':
                    return a - b;
                case '*':
                    return a * b;
                case '/':
                    return a / b;
                default:
                    return 0;
            }
        }
        static void Main(string[] args)
        {
            char op;
            double a;
            double b;

            while (true)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Zadej operaci");
                    op = Console.ReadLine()[0];
                    if (op == '0')
                        return;
                } while (!(op == '+' || op == '-' || op == '*' || op == '/'));

                Console.WriteLine("Zadej Nummer 1");
                if (!double.TryParse(Console.ReadLine(), out a))
                {
                    Console.WriteLine("Keine Nummer!");
                    Console.ReadLine();
                    continue;
                }
                Console.WriteLine("Zadej Nummer 2");
                if (!double.TryParse(Console.ReadLine(), out b))
                {
                    Console.WriteLine("Keine Nummer!");
                    Console.ReadLine();
                    continue;
                }
                if (op == '/' && b == 0)
                {
                    Console.WriteLine("Nulou nelze dělit!");
                    Console.ReadLine();
                    continue;
                }

                Console.WriteLine("Výsledek je: " + calculate(a, b, op));
                Console.ReadLine();
            }
        }
    }
}
