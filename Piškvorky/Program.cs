using System;
using System.Collections;
using System.Linq;
namespace Piškvorky
{
    class Program
    {
        static int boardSize;
        static void SetBoardSize()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Set the size of the board");
                if (int.TryParse(Console.ReadLine(), out boardSize) && boardSize > 0)
                    break;
                Console.WriteLine("Enter a positive number!");
                Console.ReadKey(true);
            }

            
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            SetBoardSize();
            
            Player p1 = new Player();
            AI ai1 = new AI('O', 2, 4, 0);
            AI ai2 = new AI('X', 3, 3, 0);
            AI ai3 = new AI('X', 3, 1, 0);
            Game g1 = new Game(ai3, p1, boardSize);
            g1.Start();
            
        }
    }
}
