using System;
namespace Piškvorky
{
    interface Controller
    {
        public Tuple<int, int> PlaceMark(Tile[][] board, int boardSize);
    }
}
