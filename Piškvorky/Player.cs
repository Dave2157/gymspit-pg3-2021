using System;
namespace Piškvorky
{
    class Player : Controller
    {
        public Tuple<int, int> PlaceMark(Tile[][] board, int boardSize)
        {
            Tuple<int, int> targetTile;
            ConsoleKeyInfo pressedKey;
            int highlightedTileX, highlightedTileY;
            highlightedTileX = highlightedTileY = 0;

            do
            {
                Console.SetCursorPosition(2 + highlightedTileX * 4, 1 + highlightedTileY * 2);
                pressedKey = Console.ReadKey(true);
                if (pressedKey.Key == ConsoleKey.UpArrow || pressedKey.Key == ConsoleKey.W)
                {
                    highlightedTileY--;
                    if (highlightedTileY < 0)
                        highlightedTileY = boardSize - 1;
                }
                else if (pressedKey.Key == ConsoleKey.DownArrow || pressedKey.Key == ConsoleKey.S)
                {
                    highlightedTileY++;
                    if (highlightedTileY == boardSize)
                        highlightedTileY = 0;
                }
                else if (pressedKey.Key == ConsoleKey.LeftArrow || pressedKey.Key == ConsoleKey.A)
                {
                    highlightedTileX--;
                    if (highlightedTileX < 0)
                        highlightedTileX = boardSize - 1;
                }
                else if (pressedKey.Key == ConsoleKey.RightArrow || pressedKey.Key == ConsoleKey.D)
                {
                    highlightedTileX++;
                    if (highlightedTileX== boardSize)
                        highlightedTileX = 0;
                }

            } while (pressedKey.Key != ConsoleKey.Enter || board[highlightedTileX][highlightedTileY].mark != ' ');
            Console.SetCursorPosition(0, 8 + boardSize * 2);
            targetTile = new Tuple<int, int>(highlightedTileX, highlightedTileY);
            return targetTile;
        }
    }
}
