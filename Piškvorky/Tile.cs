namespace Piškvorky
{
    class Tile
    {
        public char mark;
        public int x;
        public int y;
        public int[] matchingMarksInRange;
        public Tile(char m, int nx, int ny)
        {
            mark = m;
            x = nx;
            y = ny;
            matchingMarksInRange = new int[8];
        }
        public void UpdateMMIR(Tile[][] board, int boardSize)
        {
            for (int i = 0; i < 8; i++)
            {
                matchingMarksInRange[i] = 0;
            }
            int j;
            for (int i = -4; i <= 4; i++)
            {
                j = i;
                if (i == 0 || x + i >= boardSize || x + i < 0 || y + j >= boardSize || y + j < 0)
                    continue;

                if (board[x + i][y + j].mark == ' ')
                    continue;

                if (board[x + i][y + j].mark == mark)
                {
                    if (i < 0)
                    {
                        matchingMarksInRange[7]++;
                    }
                    else
                    {
                        matchingMarksInRange[3]++;
                    }
                }
                else
                {
                    if (i < 0)
                    {
                        matchingMarksInRange[7] = 0;
                    }
                    else
                    {
                        break;
                    }
                }
                
            }
            for (int i = -4; i <= 4; i++)
            {
                j = -i;
                if (i == 0 || x + i >= boardSize || x + i < 0 || y + j >= boardSize || y + j < 0)
                    continue;

                if (board[x + i][y + j].mark == ' ')
                    continue;

                if (board[x + i][y + j].mark == mark)
                {
                    if (i < 0)
                    {
                        matchingMarksInRange[5]++;
                    }
                    else
                    {
                        matchingMarksInRange[1]++;
                    }
                }
                else if (board[x + i][y + j].mark != ' ')
                {
                    if (i < 0)
                    {
                        matchingMarksInRange[5] = 0;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int i = -4; i <= 4; i++)
            {
                if (i == 0 || y + i >= boardSize || y + i < 0)
                    continue;

                if (board[x][y + i].mark == ' ')
                    continue;

                if (board[x][y + i].mark == mark)
                {
                    if (i < 0)
                    {
                        matchingMarksInRange[0]++;
                    }
                    else
                    {
                        matchingMarksInRange[4]++;
                    }
                }
                else if (board[x][y + i].mark != ' ')
                {
                    if (i < 0)
                    {
                        matchingMarksInRange[0] = 0;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int i = -4; i <= 4; i++)
            {
                if (i == 0 || x + i >= boardSize || x + i < 0)
                    continue;

                if (board[x + i][y].mark == ' ')
                    continue;

                if (board[x + i][y].mark == mark)
                {
                    if (i < 0)
                    {
                        matchingMarksInRange[6]++;
                    }
                    else
                    {
                        matchingMarksInRange[2]++;
                    }
                }
                else if (board[x + i][y].mark != ' ')
                {
                    if (i < 0)
                    {
                        matchingMarksInRange[6] = 0;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
