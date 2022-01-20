using System;
using System.IO;
using System.Runtime;

namespace File_stuff
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\david\source\repos\File stuff\stuff.txt";
            string pathOut = @"C:\Users\david\source\repos\File stuff\out.txt";

            Random rnd = new Random(888);
            using TextWriter tw = new StreamWriter(path);
            
            using TextWriter twOut = new StreamWriter(pathOut);

            for (int i = 0; i < 52; i++)
                tw.WriteLine((char)rnd.Next(65, 91));
            tw.Dispose();

            int n, m;
            bool rep;
            Console.WriteLine("Enter the number of selected values");
            n = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the number of iterations");
            m = int.Parse(Console.ReadLine());
            Console.WriteLine("Repetition? (press y to confirm otherwise press any key to continue)");
            rep = Console.ReadKey().KeyChar == 'y' ? true : false;

            using TextReader tr = new StreamReader(path);
            int[] occurences = new int[26];
            int[] ot = new int[26];
            char[] inArr = new char[52];
            for (int i = 0; i < 52; i++)
                inArr[i] = tr.ReadLine()[0];
            
            for (int i = 0; i < m; i++)
            {
                if (!rep)
                {
                    for (int fds = 0; fds < 52; fds++)
                    {
                        int index = rnd.Next(52);
                        char temp = inArr[fds];
                        inArr[fds] = inArr[index];
                        inArr[index] = temp;
                    }
                }
                for (int j = 0; j < n; j++)
                {
                    if (rep)
                    {
                        int rand = rnd.Next(52);
                        occurences[inArr[rand] - 65]++;
                        ot[inArr[rand] - 65]++;
                        twOut.WriteLine(inArr[rand]);
                    }
                    else
                    {
                        occurences[inArr[j] - 65]++;
                        ot[inArr[j] - 65]++;
                        twOut.WriteLine(inArr[j]);
                    }
                }
                for (int x = 0; x < 5; x++)
                    twOut.Write('-');
                twOut.WriteLine();
                for (int lol = 0; lol < 26; lol++)
                {
                    if (ot[lol] != 0)
                    {
                        twOut.WriteLine((char)(lol + 65) + ": " + ot[lol]);
                    }
                }
                ot = new int[26];
                for (int x = 0; x < 5; x++)
                    twOut.Write('=');
                twOut.WriteLine();
            }
            for (int x = 0; x < 10; x++)
                twOut.Write('=');
            twOut.WriteLine();
            for (int x = 0; x < 10; x++)
                twOut.Write('=');
            twOut.WriteLine();
            for (int lol = 0; lol < 26; lol++)
            {
                twOut.WriteLine((char)(lol + 65) + ": " + (float)occurences[lol] / m);
            }
        }
    }
}
