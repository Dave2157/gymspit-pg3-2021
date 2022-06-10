using System;
using System.Collections.Generic;
namespace Piškvorky
{
    class Game
    {
        Controller controller1;
        Controller controller2;

        List<Tuple<int, int>> Xs;
        List<Tuple<int, int>> Os;
        Tile[][] board;
        int boardSize;

        void CreateBoard()
        {
            board = new Tile[boardSize][];
            for (int i = 0; i < boardSize; i++)
            {
                board[i] = new Tile[boardSize];
                for (int j = 0; j < boardSize; j++)
                {
                    board[i][j] = new Tile(' ', i, j);
                }
            }
        }
        void DrawBoard()
        {
            Console.Clear();
            int widthInChars = boardSize * 4 + 1;
            int heightInChars = boardSize * 2 + 1;
            Console.Write('┏');
            for (int i = 0; i < widthInChars - 2; i++)
            {
                Console.Write((i + 1) % 4 == 0 ? '┳' : '━');
            }
            Console.WriteLine('┓');
            for (int i = 0; i < heightInChars - 2; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < widthInChars; j++)
                    {
                        Console.Write(j % 4 == 0 ? '┃' : (j + 1) % 2 == 0 ? ' ' : board[(j - 2) / 4][i / 2].mark);
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.Write('┣');
                    for (int j = 0; j < widthInChars - 2; j++)
                        Console.Write((j + 1) % 4 == 0 ? '╋' : '━');
                    Console.WriteLine('┫');
                }
            }
            Console.Write('┗');
            for (int i = 0; i < widthInChars - 2; i++)
            {
                Console.Write((i + 1) % 4 == 0 ? '┻' : '━');
            }
            Console.WriteLine('┛');
        }
        void UpdateBoard(int x, int y)
        {
            for (int i = -4, j = -4; i <= 4; i++, j++)
            {
                if (i == 0 || x + i >= boardSize || x + i < 0 || y + j >= boardSize || y + j < 0)
                    continue;
                board[x + i][y + j].UpdateMMIR(board, boardSize);
                if (y - j >= boardSize || y - j < 0)
                    continue;
                board[x + i][y - j].UpdateMMIR(board, boardSize);
            }
            for (int i = -4; i <= 4; i++)
            {
                if (i == 0 || x + i >= boardSize || x + i < 0)
                    continue;
                board[x + i][y].UpdateMMIR(board, boardSize);
            }
            for (int i = -4; i <= 4; i++)
            {
                if (i == 0 || y + i >= boardSize || y + i < 0)
                    continue;
                board[x][y + i].UpdateMMIR(board, boardSize);
            }
        }
        int GameOverCheck(int x, int y)
        {
            int lengthOfAConnectedLine = 0;
            for (int i = 1; i <= 4; i++)
            {
                int j = i;
                if (i == 0 || x + i >= boardSize || x + i < 0 || y + j >= boardSize || y + j < 0)
                    continue;
                if (board[x + i][y + j].mark == board[x][y].mark)
                {
                    lengthOfAConnectedLine++;
                }
                else
                    break;
            }
            for (int i = -1; i >= -4; i--)
            {
                int j = i;
                if (i == 0 || x + i >= boardSize || x + i < 0 || y + j >= boardSize || y + j < 0)
                    continue;
                if (board[x + i][y + j].mark == board[x][y].mark)
                {
                    lengthOfAConnectedLine++;
                }
                else
                    continue;
            }
            if (lengthOfAConnectedLine >= 4)
                switch (board[x][y].mark)
                {
                    case 'X':
                        return 1;
                    case 'O':
                        return 2;
                }
            lengthOfAConnectedLine = 0;


            for (int i = 1; i <= 4; i++)
            {
                int j = -i;
                if (i == 0 || x + i >= boardSize || x + i < 0 || y + j >= boardSize || y + j < 0)
                    continue;
                if (board[x + i][y + j].mark == board[x][y].mark)
                {
                    lengthOfAConnectedLine++;
                }
                else
                    break;
            }
            for (int i = -1; i >= -4; i--)
            {
                int j = -i;
                if (i == 0 || x + i >= boardSize || x + i < 0 || y + j >= boardSize || y + j < 0)
                    continue;
                if (board[x + i][y + j].mark == board[x][y].mark)
                {
                    lengthOfAConnectedLine++;
                }
                else
                    continue;
            }
            if (lengthOfAConnectedLine >= 4)
                switch (board[x][y].mark)
                {
                    case 'X':
                        return 1;
                    case 'O':
                        return 2;
                }
            lengthOfAConnectedLine = 0;


            for (int i = 1; i <= 4; i++)
            {
                if (i == 0 || x + i >= boardSize || x + i < 0)
                    continue;
                if (board[x + i][y].mark == board[x][y].mark)
                {
                    lengthOfAConnectedLine++;
                }
                else
                    break;
            }
            for (int i = -1; i >= -4; i--)
            {
                if (i == 0 || x + i >= boardSize || x + i < 0)
                    continue;
                if (board[x + i][y].mark == board[x][y].mark)
                {
                    lengthOfAConnectedLine++;
                }
                else
                    break;
            }
            if (lengthOfAConnectedLine >= 4)
                switch (board[x][y].mark)
                {
                    case 'X':
                        return 1;
                    case 'O':
                        return 2;
                }
            lengthOfAConnectedLine = 0;

            for (int i = 1; i <= 4; i++)
            {
                if (i == 0 || y + i >= boardSize || y + i < 0)
                    continue;
                if (board[x][y + i].mark == board[x][y].mark)
                {
                    lengthOfAConnectedLine++;
                }
                else
                    break;
            }
            for (int i = -1; i >= -4; i--)
            {
                if (i == 0 || y + i >= boardSize || y + i < 0)
                    continue;
                if (board[x][y + i].mark == board[x][y].mark)
                {
                    lengthOfAConnectedLine++;
                }
                else
                    break;
            }
            if (lengthOfAConnectedLine >= 4)
                switch (board[x][y].mark)
                {
                    case 'X':
                        return 1;
                    case 'O':
                        return 2;
                }
            if (Xs.Count + Os.Count == boardSize * boardSize)
                return 0;
            return -1;
        }
        void HighlightTile(int x, int y)
        {
            Console.SetCursorPosition(1 + x * 4, 1 + y * 2);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(' ');
            Console.Write(board[x][y].mark);
            Console.Write(' ');
            Console.SetCursorPosition(0, 8 + boardSize * 2);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
        void UnhighlightTile(int x, int y)
        {
            Console.SetCursorPosition(1 + x * 4, 1 + y * 2);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(' ');
            Console.Write(board[x][y].mark);
            Console.Write(' ');
            Console.SetCursorPosition(0, 8 + boardSize * 2);
        }
        void HighlightWinningLine(int x, int y, char winner)
        {
            List<int> winningLines = new List<int>();

            board[x][y].UpdateMMIR(board, boardSize);

            for (int i = 0; i < 4; i++)
                if (board[x][y].matchingMarksInRange[i] + board[x][y].matchingMarksInRange[i + 4] >= 4)
                    winningLines.Add(i);

            foreach (int index in winningLines)
            {
                switch (index)
                {
                    case 0:
                        for (int i = 0; i <= 4; i++)
                        {
                            if (y + i >= boardSize || board[x][y + i].mark != winner)
                                break;
                            HighlightTile(x, y);
                        }
                        for (int i = -1; i >= -4; i++)
                        {
                            if (y + i < 0 || board[x][y + i].mark != winner)
                                break;
                            HighlightTile(x, y);
                        }
                        break;
                    case 1:
                        for (int i = 0; i <= 4; i++)
                        {
                            if (x + i >= boardSize || y + i >= boardSize || board[x + i][y + i].mark != winner)
                                break;
                            HighlightTile(x, y);
                        }
                        for (int i = -1; i >= -4; i++)
                        {
                            if (x + i < 0 || y + i < 0 || board[x + i][y + i].mark != winner)
                                break;
                            HighlightTile(x, y);
                        }
                        break;
                    case 2:
                        for (int i = 0; i <= 4; i++)
                        {
                            if (x + i >= boardSize || board[x + i][y].mark != winner)
                                break;
                            HighlightTile(x, y);
                        }
                        for (int i = -1; i >= -4; i++)
                        {
                            if (x + i < 0 || board[x + i][y].mark != winner)
                                break;
                            HighlightTile(x, y);
                        }
                        break;
                    case 3:
                        for (int i = 0; i <= 4; i++)
                        {
                            if (x + i >= boardSize || y - i >= boardSize || board[x + i][y - i].mark != winner)
                                break;
                            HighlightTile(x, y);
                        }
                        for (int i = -1; i >= -4; i++)
                        {
                            if (x + i < 0 || y - i < 0 || board[x + i][y - i].mark != winner)
                                break;
                            HighlightTile(x, y);
                        }
                        break;
                }


            }
        }
        public Controller Start()
        {
            int turnCounter = 0;
            int GameOverToken = -1;
            Tuple<int, int> coordsOfPlacedMark;
            int x = 0, y = 0;
            CreateBoard();

            DrawBoard();

            while (GameOverToken == -1)
            {
                
                coordsOfPlacedMark = (turnCounter % 2 == 0 ? controller1 : controller2).PlaceMark(board, boardSize);
                UnhighlightTile(x, y);
                x = coordsOfPlacedMark.Item1;
                y = coordsOfPlacedMark.Item2;
                board[x][y].mark = turnCounter % 2 == 0 ? 'X' : 'O';
                (turnCounter % 2 == 0 ? Xs : Os).Add(new Tuple<int, int>(x, y));

                UpdateBoard(x, y);

                GameOverToken = GameOverCheck(x, y);
                turnCounter++;
                
                DrawBoard();
                HighlightTile(x, y);
            }


                HighlightWinningLine(x, y, (turnCounter - 1) % 2 == 0 ? 'X' : 'O');
            if (GameOverToken == 1)
                return controller1;
            if (GameOverToken == 2)
                return controller2;

            return null;
        }
        public Game(Controller c1, Controller c2, int bs)
        {
            controller1 = c1;
            controller2 = c2;
            boardSize = bs;
            Xs = new List<Tuple<int, int>>();
            Os = new List<Tuple<int, int>>();

        }
    }
}
