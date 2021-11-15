using System;

namespace Covid_Statistics
{
    class Program
    {
        static int[] cases = new int[0];
        static bool debug = false;
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
            Console.WriteLine("5 - Predict");
            Console.WriteLine("6 - Graph");
            Console.WriteLine("0 - Quit");
            int choice;

            while (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out choice) && choice < 6 && choice >= 0) ;
            return choice;
        }
        static void enter()
        {
            if (debug)
            {
                cases = new int[21] { 1000, 1500, 2000, 5000, 10000, 6000, 4000, 1000, 1000, 3000, 2000, 1000, 1000, 1500, 1500, 2000, 3000, 5000, 7500, 8500, 9000 };
                return;
            }
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
            if (cases.Length == 0)
                return;

            if (b < 0)
                b = 0;
            if (b > cases.Length - 1)
                b = cases.Length - 1;
            if (e < b)
                e = b + 1;
            if (e > cases.Length - 1)
                e = cases.Length - 1;

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
                double A7 = 0;
                double A14 = 0;
                if (i >= 11)
                     R = Math.Round((double)(work[i] + work[i - 1] + work[i - 2] + work[i - 3] + work[i - 4] + work[i - 5] + work[i - 6]) * 100.0 / (double)(work[i - 7] + work[i - 8] + work[i - 9] + work[i - 10] + work[i - 11] + work[i - 5] + work[i - 6])) / 100.0;
                if (i >= 13)
                     A14 = (double)(work[i] + work[i - 1] + work[i - 2] + work[i - 3] + work[i - 4] + work[i - 5] + work[i - 6] + work[i - 7] + work[i - 8] + work[i - 9] + work[i - 10] + work[i - 11] + work[i - 12] + work[i - 13] / 100);
                if (i >= 6)
                    A7 = (double)(work[i] + work[i - 1] + work[i - 2] + work[i - 3] + work[i - 4] + work[i - 5] + work[i - 6] / 100);
                
                Console.Write("Day " + (b + i));
                int currentIndexLength = i > 1 ? i % 10 == 0 ? (int)Math.Log(i) : (int)Math.Ceiling(Math.Log10(i)) : 1; //velmi přehledný statement
                for (int j = -1; j < biggestIndexLength - currentIndexLength; j++)
                    Console.Write(' ');
                Console.WriteLine("- Infected: " + work[i] + "; Average 100 000 group: " + average + (i >= 11 ? ("; R: " + R) : "") + (i >= 6 ? "; A7: " + A7 : "") + (i >= 13 ? "; A14: " + A14 : ""));
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
            Console.WriteLine("1 digit - display a day | 2 digits separated by spaces - display a range | 'A' - display all days");
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
            if (cases.Length == 0)
                return;

            if (b < 0)
                b = 0;
            if (b > cases.Length - 1)
                b = cases.Length - 1;
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
            Console.WriteLine("1 digit - edit a day | 2 digits separated by spaces - edit a range | 'A' - edit all days");
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
            if (cases.Length == 0)
                return;
            if (b < 0)
                b = 0;
            if (e < b)
                e = b + 1;
            if (b > cases.Length - 1)
                b = cases.Length - 1;
            if (e > cases.Length - 1)
                e = cases.Length - 1;
            if (e > cases.Length - 1)
                e = cases.Length - 1;
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
            Console.WriteLine("1 digit - delete a day | 2 digits separated by spaces - delete a range | 'A' - delete all days");
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
        static void graph(int offset)
        {
            Console.Clear();
            if (cases.Length < 14)
                return;

            int maxVal = max(cases);

            int step = (int)(Math.Pow(10.0, Math.Ceiling(Math.Log10(maxVal))) / 5.0);
            int yAxis = (int)Math.Floor(Math.Log10(step * 5)) + 2;

            int pointsPerDay = 8;
            int overlayWidth = yAxis + 1;
            int graphWidth = pointsPerDay * 14;

            Console.SetWindowSize(overlayWidth + graphWidth + 2, 29);

            char[][] output = new char[112][];
            for (int i = 0; i < 112; i++)
                output[i] = new char[25];

            for (int y = 0; y < 28; y++)
            {
                if (y == 28)
                    Console.Write("Use arrows to navigate");
                for (int x = 0; x < overlayWidth + graphWidth; x++)
                {
                    if (y == 25)
                    {
                        Console.Write(x < yAxis ? ' ' : '_');
                    }
                    else if (y == 26)
                    {
                        if (x < yAxis)
                            Console.Write(' ');
                        else if (x == yAxis)
                            Console.Write('|');
                        else
                        {
                            if ((x - overlayWidth) % pointsPerDay == 0)
                                Console.Write(offset + (x - overlayWidth) / pointsPerDay);
                            else
                            {
                                bool lil = (x - overlayWidth - 1) % pointsPerDay == 0;
                                int lul = offset + (x - overlayWidth - 1) / pointsPerDay;

                                bool lal = (x - overlayWidth - 2) % pointsPerDay == 0;
                                int lol = offset + (x - overlayWidth - 2) / pointsPerDay;
                                if (!(lil && Math.Log10(lul) >= 1) && !(lal && Math.Log10(lol) >= 2))
                                    Console.Write(' ');
                            }
                                
                        }
                    }
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
                    else
                    {
                        char ch;
                        int closestIntDolu = cases[offset + (int)Math.Floor((double)(x - overlayWidth) / pointsPerDay)];
                        int closestIntNahoru;
                        if (offset + (int)Math.Floor((double)(x - overlayWidth) / pointsPerDay) + 1 == cases.Length)
                            closestIntNahoru = closestIntDolu;
                        else
                            closestIntNahoru = cases[offset + (int)Math.Floor((double)(x - overlayWidth) / pointsPerDay) + 1];

                        double blend = (double)((x - overlayWidth) % pointsPerDay) / pointsPerDay;
                        if (blend == 0)
                            ch = 'X';
                        else
                            ch = '.';
                        int valueAtX = (int)Math.Floor((1 - blend) * closestIntDolu + blend * closestIntNahoru);

                        int rowStep = step / 5;
                        for (int i = 0; i < 25; i++)
                        {
                            if (i * rowStep > valueAtX)
                            {
                                valueAtX = i * rowStep;
                                break;
                            }
                        }

                        if (y == 0 || y == 5 || y == 10 || y == 15 || y == 20)
                        {
                            if ((25 - y) * rowStep == valueAtX)
                                Console.Write(ch);
                            else
                                Console.Write('-');
                        }
                        else
                        {
                            if ((25 - y) * rowStep == valueAtX)
                                Console.Write(ch);
                            else
                                Console.Write(' ');
                        }
                    }
                }
                Console.Write('\n');
            }
        }
        static void predict()
        {
            uint n;
            char choice;
            if (cases.Length < 15)
                return;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter the number of days, on which you want to have the number of cases predicted");
            } while (!uint.TryParse(Console.ReadLine(), out n));
            do
            {
                Console.Clear();
                Console.WriteLine("Choose a prediction method: A - using R, B - using the case ratio between weeks");
                choice = Console.ReadKey().KeyChar;
            } while (choice != 'a' && choice != 'b');

            Array.Resize<int>(ref cases, cases.Length + (int)n);

            if (choice == 'a')
            {
                for (int i = cases.Length - (int)n; i < cases.Length; i++)
                {
                    double R = Math.Round((double)(cases[i - 7] + cases[i - 1] + cases[i - 2] + cases[i - 3] + cases[i - 4] + cases[i - 5] + cases[i - 6]) * 100.0 / (double)(cases[i - 7] + cases[i - 8] + cases[i - 9] + cases[i - 10] + cases[i - 11] + cases[i - 12] + cases[i - 6])) / 100.0;
                    int value = (int)Math.Round(R * (double)(cases[i - 5] + cases[i - 6] + cases[i - 7] + cases[i - 8] + cases[i - 9] + cases[i - 10] + cases[i - 11] - (double)(cases[i - 1] + cases[i - 2] + cases[i - 3] + cases[i - 4] + cases[i - 5] + cases[i - 6])));
                    cases[i] = value;
                }
                report(cases.Length - (int)n - 1, cases.Length - 1);
                delete(cases.Length - (int)n, cases.Length - 1);
            }
            else
            {
                for (int i = cases.Length - (int)n; i < cases.Length; i++)
                {
                    int value = (int)((double)(cases[i - 7] * cases[i - 7]) / cases[i - 14]);
                    cases[i] = value;
                }
                report(cases.Length - (int)n - 1, cases.Length - 1);
                delete(cases.Length - (int)n, cases.Length - 1);
            }
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
                        predict();
                        break;
                    case 6:
                        int o = 0;
                        ConsoleKey k;

                        do
                        {
                            graph(o);
                            k = Console.ReadKey().Key;

                            if (k == ConsoleKey.LeftArrow)
                            {
                                if (o > 0)
                                    o--;
                            }
                            else if (k == ConsoleKey.RightArrow)
                            {
                                if (o < cases.Length - 15)
                                    o++;
                            }
                        } while (k == ConsoleKey.RightArrow || k == ConsoleKey.LeftArrow);

                        break;
                }
            }
        end:;
        }
    }
}
