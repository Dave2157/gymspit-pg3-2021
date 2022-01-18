using System;
using System.IO;
using System.Collections.Generic;


namespace Pokladni_denik
{
    class Stuff
    {
        public String name;
        public int price;
        public String category;
    }
    class Program
    {
        static int initialVal;
        static List<Stuff> stuff = new List<Stuff>();
        static string workingFile;
        static void Add()
        {
            Stuff sTemp = new Stuff();
            Console.Clear();
            Console.WriteLine("Enter the name of the item");
            string name = Console.ReadLine();
            sTemp.name = name;
            int value;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter the value of the item");
                if (int.TryParse(Console.ReadLine(), out value))
                    break;
            }
            sTemp.price = value;
            Console.Clear();
            Console.WriteLine("Set a category for the item");
            name = Console.ReadLine();
            sTemp.category = name;
            stuff.Add(sTemp);
        }
        static void Log()
        {
            string choice;
           

            int iChoice;
            while (true)
            {
                int possum = 0;
                int negsum = 0;
                int balance = initialVal;

                Console.Clear();
                Console.WriteLine("Enter an index to delete an item, enter 'q' to quit\n");
                for (int i = 0; i < stuff.Count; i++)
                {
                    if (stuff[i].price > 0)
                        possum += stuff[i].price;
                    else
                        negsum += stuff[i].price;

                    Console.WriteLine(i + " : " + balance + (stuff[i].price < 0 ? " - " : " + ") + Math.Abs(stuff[i].price) + ' ' + stuff[i].name + " = " + (balance += stuff[i].price));
                }
                Console.WriteLine("\nTotal income: " + possum);
                Console.WriteLine("Total expenses: " + -negsum);
                Console.WriteLine("Total: " + (possum + negsum));
                Console.WriteLine("Final balance: " + balance);

                choice = Console.ReadLine();
                if (choice == "q")
                    return;
                if (int.TryParse(choice, out iChoice) && iChoice < stuff.Count && iChoice >= 0)
                    break;
            }
            stuff.RemoveAt(iChoice);
        }
        static List<string> kjlsadjkl()
        {
            List<string> categories = new List<string>();

            foreach (Stuff x in stuff)
            {
                if (!categories.Contains(x.category))
                    categories.Add(x.category);
            }

            return categories;
        }
        static void logCategories()
        {
            List<string> categories = kjlsadjkl();


            string choice;
            int iChoice;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Select a category\n");
                for (int i = 0; i < categories.Count; i++)
                {
                    Console.WriteLine(i + " - " + categories[i]);
                }
                choice = Console.ReadLine();
                if (int.TryParse(choice, out iChoice) && iChoice < categories.Count && iChoice >= 0)
                    break;
            }
            string selectedCategory = categories[iChoice];

            List<Stuff> catStuff = new List<Stuff>();
            for (int i = 0; i < stuff.Count; i++)
            {
                if (stuff[i].category == selectedCategory)
                    catStuff.Add(stuff[i]);
            }

            while (true)
            {
                int possum = 0;
                int negsum = 0;
                int balance = initialVal;

                Console.Clear();
                Console.WriteLine("Enter an index to delete an item, enter 'q' to quit\n");
                int i = 0;
                for (; i < catStuff.Count; i++)
                {
                    if (catStuff[i].price > 0)
                        possum += catStuff[i].price;
                    else
                        negsum += catStuff[i].price;

                    Console.WriteLine(i + " : " + balance + (catStuff[i].price < 0 ? " - " : " + ") + Math.Abs(catStuff[i].price) + ' ' + catStuff[i].name + " = " + (balance += catStuff[i].price));
                }
                Console.WriteLine("\nTotal income: " + possum);
                Console.WriteLine("Total expenses: " + -negsum);
                Console.WriteLine("Total: " + (possum + negsum));
                Console.WriteLine("Final balance: " + balance);

                choice = Console.ReadLine();
                if (choice == "q")
                    return;
                if (int.TryParse(choice, out iChoice) && iChoice < catStuff.Count && iChoice > 0)
                    break;
            }
            stuff.Remove(catStuff[iChoice]);
        }
        static void Menu()
        {
            string choice;
            while (true)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("0 - Add a transaction");
                    Console.WriteLine("1 - Show log");
                    Console.WriteLine("2 - Show categories");
                    Console.WriteLine("3 - Reset file");
                    Console.WriteLine("4 - Quit");

                    choice = Console.ReadLine();
                } while (choice != "0" && choice != "1" && choice != "2" && choice != "3" && choice != "4");
                switch (choice)
                {
                    case "0":
                        Add();
                        break;
                    case "1":
                        Log();
                        break;
                    case "2":
                        logCategories();
                        break;
                    case "3":
                        stuff = new List<Stuff>();
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("Enter a new inital value");
                        }
                        while (!int.TryParse(Console.ReadLine(), out initialVal));
                        break;
                    case "4":
                        goto konec;
                }
            }
        konec:;
        }
        static void Main(string[] args)
        {    
            StreamReader filesSR = new StreamReader(@"C:\Users\david\source\repos\Pokladni denik\files.txt");

            List<string> fileNames = new List<string>();
            string temp;
            while ((temp = filesSR.ReadLine()) != null)
            {
                fileNames.Add(temp);
            }

            int iTemp = -1;
            do
            {
                Console.Clear();
                Console.WriteLine("Choose a working file || a - create a new working file");
                for (int i = 0; i < fileNames.Count; i++)
                {
                    Console.WriteLine(i + " - " + fileNames[i]);
                }
                temp = Console.ReadLine();
                if (temp == "a")
                    break;
                if (!int.TryParse(temp, out iTemp))
                    continue;
            } while (iTemp < 0 || iTemp >= fileNames.Count);

            filesSR.Close();

            StreamReader work;
            if (temp == "a")
            {
                Console.Clear();
                Console.WriteLine("Enter a name for the file");
                string fname = Console.ReadLine();
                fileNames.Add(fname);

                int iv;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Set an initial value");
                } while (!int.TryParse(Console.ReadLine(), out iv));


                File.Create(@"C:\Users\david\source\repos\Pokladni denik\" + fname + ".txt").Close();
                StreamWriter swTemp = new StreamWriter(@"C:\Users\david\source\repos\Pokladni denik\" + fname + ".txt");
                swTemp.WriteLine(iv);
                swTemp.Close();
                work = new StreamReader(@"C:\Users\david\source\repos\Pokladni denik\" + fname + ".txt");
                workingFile = fname;
            }
            else
            {
                workingFile = fileNames[iTemp];
                work = new StreamReader(@"C:\Users\david\source\repos\Pokladni denik\" + workingFile + ".txt");
            }
                

            
            temp = work.ReadLine();
            initialVal = int.Parse(temp);
            while ((temp = work.ReadLine()) != null)
            {
                string[] pair = temp.Split(' ');
                Stuff sTemp = new Stuff();
                sTemp.name = pair[0];
                sTemp.price = int.Parse(pair[1]);
                sTemp.category = pair[2];
                stuff.Add(sTemp);
            }
            work.Close();

            Menu();

            StreamWriter fout = new StreamWriter(@"C:\Users\david\source\repos\Pokladni denik\" + workingFile + ".txt");
            fout.WriteLine(initialVal);

            for (int i = 0; i < stuff.Count; i++)
            {
                Stuff x = stuff[i];
                fout.WriteLine(x.name + ' ' + x.price + ' ' + x.category);
            }
            fout.Close();
            fout.Dispose();
            fout = new StreamWriter(@"C:\Users\david\source\repos\Pokladni denik\files.txt");

            for (int i = 0; i < fileNames.Count; i++)
            {
                fout.WriteLine(fileNames[i]);                
            }
            fout.Close();
        }
    }
}
