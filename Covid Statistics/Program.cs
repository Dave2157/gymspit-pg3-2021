using System;

namespace Covid_Statistics
{
    class Program
    {
        static int[] cases = new int[0];
        static int min(int[] array, out int index)
        {
            int min = int.MaxValue;
            index = 0;
            for (int i = 0; i < array.Length; i++)
                if (array[i] < min)
                {
                    min = array[i];
                    index = i;
                }
                    
            return min;
        }
        static int max(int[] array, out int index)
        {
            int max = 0;
            index = 0;
            for (int i = 0; i < array.Length; i++)
                if (array[i] > max)
                {
                    max = array[i];
                    index = i;
                }

            return max;
        }
        static int max(int[] array)
        {
            int max = 0;
            for (int i = 0; i < array.Length; i++)
                if (array[i] > max)
                    max = array[i];

            return max;
        }
        static int menu()
        {
            Console.Clear();
            Console.WriteLine("1 - Enter values");
            Console.WriteLine("2 - Edit values");
            Console.WriteLine("3 - Delete values");
            Console.WriteLine("4 - Show report");
            Console.WriteLine("5 - Graph");
            Console.WriteLine("0 - Quit");
            int choice;

            while (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out choice) && choice < 6 && choice >= 0) ;
            return choice;
        }
        static void enter()
        {
            Console.Clear();
            Console.WriteLine("Enter a series of positive numbers separated by spaces");
            string input = Console.ReadLine();
            string[] substrings = input.Split(' ');

            System.Collections.Generic.List<int> faultyInputs = new System.Collections.Generic.List<int>();

            int prevCSize = cases.Length;
            Array.Resize<int>(ref cases, cases.Length + substrings.Length);

            for (int i = 0; i < substrings.Length; i++)
            {
                
                if (!int.TryParse(substrings[i], out cases[prevCSize + i]) || !(cases[prevCSize + i] >= 0))
                {
                    cases[prevCSize + i] = 0;
                    faultyInputs.Add(prevCSize + i);
                }
            }
            if (faultyInputs.Count == 0)
                Console.WriteLine("Input successful!");
            else
            {
                Console.Write("Faulty inputs on indexes: ");
                foreach (int fi in faultyInputs)
                    Console.Write(fi.ToString() + ' '); ;
                Console.Write('\n');
            }
            Console.ReadKey();
        }
        static void report()
        {
            report(0, cases.Length);
        }
        static void report(int b, int e)
        {
            Console.Clear();
            int[] work = new int[e - b];
            for (int i = b, j = 0; i < e; i++, j++)
                work[j] = cases[i];

            int maxIndex;
            int minIndex;
            int max = Program.max(work, out maxIndex);
            int min = Program.min(work, out minIndex);
            for (int i = -10; i < 10; i++)
            {
                Console.Write('=');
                if (i == 0)
                    Console.Write(" REPORT =");
            }
            Console.Write('\n');
            int biggestIndexLength = work.Length > 1 ? (int) Math.Ceiling(Math.Log10(work.Length - 1)) : 1;
            for (int i = 0; i < work.Length; i++)
            {
                float average = (float)work[i] / 100.0f;
                double R = 0;
                if (i >= 14)
                     R = Math.Round((double)(work[i] + work[i - 1] + work[i - 2] + work[i - 3] + work[i - 4] + work[i - 5] + work[i - 6]) * 100.0 / (double)(work[i - 7] + work[i - 8] + work[i - 9] + work[i - 10] + work[i - 11] + work[i - 12] + work[i - 13])) / 100.0;
                Console.Write("Day " + (b + i));
                int currentIndexLength = i > 1 ? i % 10 == 0 ? (int)Math.Log(i) : (int)Math.Ceiling(Math.Log10(i)) : 1; //velmi přehledný statement
                for (int j = -1; j < biggestIndexLength - currentIndexLength; j++)
                    Console.Write(' ');
                Console.WriteLine("- Infected: " + work[i] + "; Average 100 000 group: " + average + (i >= 14 ? ("; R: " + R) : ""));
            }
            Console.Write('\n');
            Console.WriteLine("The highest number of infections: " + max + ", day " + maxIndex);
            Console.WriteLine("The lowest number of infections: " + min + ", day " + minIndex);
            for (int i = -14; i < 14; i++)
                Console.Write('=');
            Console.Write('\n');
            Console.ReadKey();
        }
        static void reportRange()
        {
            Console.Clear();
            Console.WriteLine("1 digit - display a day | 2 digits - display a range | 'A' - display all days");
            int a, b;
            string[] substrings;
            do
            {
                string input = Console.ReadLine();
                substrings = input.Split(' ');
                if (input == "a")
                    goto tohleFaktAsiNeniUplneKoserReseniTohohleProblemu;
                if (substrings.Length == 1 && int.TryParse(substrings[0], out a))
                    goto gotoJePrejHrichAleSnadToNevadi;
            } while (!(int.TryParse(substrings[0], out a) && int.TryParse(substrings[1], out b) && a <= b));
            report(a, b + 1);
            return;
        gotoJePrejHrichAleSnadToNevadi:
            report(a, a + 1);
            return;
        tohleFaktAsiNeniUplneKoserReseniTohohleProblemu:
            report();
        }
        static void edit()
        {
            edit(0, cases.Length);
        }
        static void edit(int b, int e)
        {
            Console.Clear();
            Console.WriteLine("Enter a series of positive numbers separated by spaces");
            string input = Console.ReadLine();
            string[] substrings = input.Split(' ');

            System.Collections.Generic.List<int> faultyInputs = new System.Collections.Generic.List<int>();

            for (int i = 0; i < substrings.Length && i < cases.Length; i++)
            {
                if (!int.TryParse(substrings[i], out cases[b + i]) || !(cases[b + i] >= 0))
                {
                    cases[b + i] = 0;
                    faultyInputs.Add(b + i);
                }
            }
            if (faultyInputs.Count == 0)
                Console.WriteLine("Edit successful!");
            else
            {
                Console.Write("Faulty inputs on indexes: ");
                foreach (int fi in faultyInputs)
                    Console.Write(fi.ToString() + ' '); ;
                Console.Write('\n');
            }
            Console.ReadKey();
        }
        static void editRange()
        {
            Console.Clear();
            Console.WriteLine("1 digit - edit a day | 2 digits - edit a range | 'A' - edit all days");
            int a, b;
            string[] substrings;
            do
            {
                string input = Console.ReadLine();
                substrings = input.Split(' ');
                if (input == "a")
                    goto tohleFaktAsiNeniUplneKoserReseniTohohleProblemu;
                if (substrings.Length == 1 && int.TryParse(substrings[0], out a))
                    goto gotoJePrejHrichAleSnadToNevadi;
            } while (!(int.TryParse(substrings[0], out a) && int.TryParse(substrings[1], out b) && a <= b));
            edit(a, b + 1);
            return;
        gotoJePrejHrichAleSnadToNevadi:
            edit(a, a + 1);
            return;
        tohleFaktAsiNeniUplneKoserReseniTohohleProblemu:
            edit();
        }
        static void delete()
        {
            cases = new int[0];
        }
        static void delete(int b, int e)
        {
            int[] temp = new int[cases.Length - e - 1 + b];
            for (int i = 0, j = 0; i < cases.Length; i++)
                if (i < b || i > e)
                {
                    temp[j] = cases[i];
                    j++;
                }
            cases = temp;
        }
        static void deleteRange()
        {
            Console.Clear();
            Console.WriteLine("1 digit - delete a day | 2 digits - delete a range | 'A' - delete all days");
            int a, b;
            string[] substrings;
            do
            {
                string input = Console.ReadLine();
                substrings = input.Split(' ');
                if (input == "a")
                    goto tohleFaktAsiNeniUplneKoserReseniTohohleProblemu;
                if (substrings.Length == 1 && int.TryParse(substrings[0], out a))
                    goto gotoJePrejHrichAleSnadToNevadi;
            } while (!(int.TryParse(substrings[0], out a) && int.TryParse(substrings[1], out b) && a <= b));
            delete(a, b + 1);
            return;
        gotoJePrejHrichAleSnadToNevadi:
            delete(a, a);
            return;
        tohleFaktAsiNeniUplneKoserReseniTohohleProblemu:
            delete();
        }
        static void graph()
        {
            Console.Clear();
            if (cases.Length == 0)
                return;
            int maxVal = max(cases);
            int step = (int) (Math.Pow(10.0, Math.Ceiling(Math.Log10(maxVal))) / 5.0);
            char[][] output = new char[112][];
            int yAxis = (int)Math.Floor(Math.Log10(step * 5)) + 2;
            Console.SetWindowSize(115 + yAxis, 32);
            for (int i = 0; i < 112; i++)
                output[i] = new char[25];

            for (int y = 0; y < 26; y++)
            {
                for (int x = 0; x < 113 + yAxis; x++)
                {
                    if (y == 25)
                        Console.Write(x < yAxis ? ' ': '_');
                    else if (x < yAxis)
                    {
                        if (y == 0 || y == 5 || y == 10 || y == 15 || y == 20)
                        {
                            for (int i = 0; i < Math.Log10(step * 5) - Math.Log10(step * (5 - (double)y / 5.0)); i++)
                            {
                                Console.Write(' ');
                            }
                            Console.Write(step * (5 - (double)y / 5.0));
                            Console.Write(' ');
                            x = yAxis - 1;
                        }
                        else
                            Console.Write(' ');
                    }
                    else if (x == yAxis)
                    {
                        Console.Write('|');
                    }
                }
                Console.Write('\n');
            }

            Console.ReadKey();
        }
        static void Main(string[] args)
        {
            while (true)
            { 
                switch (menu())
                {
                    case 0:
                        goto end;
                    case 1:
                        enter();
                        break;
                    case 2:
                        editRange();
                        break;
                    case 3:
                        deleteRange();
                        break;
                    case 4:
                        reportRange();
                        break;
                    case 5:
                        graph();
                        break;
                }
            }
        end:;
        }
    }
}
