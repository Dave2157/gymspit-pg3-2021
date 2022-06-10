using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Piškvorky
{
    class AI : Controller
    {

        
        class TurnOption
        {
            public Tuple<int, int> coordinates;
            public float heuristicValue;
        }
        class TurnOptionSorter : IComparer<TurnOption>
        {
            public int Compare(TurnOption a, TurnOption b)
            {
                float difference = a.heuristicValue - b.heuristicValue;
                if (difference > 0)
                    return -1;
                else if (difference < 0)
                    return 1;

                return 0;
            }
        }


        char mark;
        char opponentsMark;
        int branchingFactor;
        int searchDepth;
        float randomness;

        public AI(char m, int bf, int d, float r)
        {
            mark = m;
            opponentsMark = mark == 'X' ? 'O' : 'X';
            branchingFactor = bf;
            searchDepth = d;
            randomness = r;
        }

        bool OutOfBounds(int x, int y, int offsetX, int offsetY, int boardSize)
        {
            if (x + offsetX >= boardSize || x + offsetX < 0)
                return true;
            if (y + offsetY >= boardSize || y + offsetY < 0)
                return true;
            return false;
        }

        

        List<Tuple<int, int>> GetTilesToBeInspected(Tile[][] board, int boardSize)
        {
            List<Tuple<int, int>> tilesWithMarks = new List<Tuple<int, int>>();

            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                {
                    if (board[i][j].mark != ' ')
                    {
                        tilesWithMarks.Add(new Tuple<int, int>(i, j));
                    }
                }

            List<Tuple<int, int>> tilesToBeReturned = new List<Tuple<int, int>>();
            foreach (Tuple<int, int> tile in tilesWithMarks)
            {
                for (int i = -2; i <= 2; i++)
                    for (int j = -2; j <= 2; j++)
                    {
                        if (OutOfBounds(tile.Item1, tile.Item2, i, j, boardSize))
                            continue;
                        if (board[tile.Item1 + i][tile.Item2 + j].mark != ' ')
                            continue;
                        tilesToBeReturned.Add(new Tuple<int, int>(tile.Item1 + i, tile.Item2 + j));
                    }
            }

            return tilesToBeReturned.Distinct().ToList();
        }

        int SearchForASequence(Tile[][] board, int boardSize, List<char> sequence)
        {
            int numberOfOccurences = 0;

            List<char> reversedSequence = new List<char>(sequence);
            reversedSequence.Reverse();
            
            if (!sequence.Equals(reversedSequence))
            {
                for (int i = 0; i < boardSize; i++)
                    for (int j = 0; j < boardSize; j++)
                    {
                        if (board[i][j].mark == reversedSequence[0])
                        {
                            int k = 1;
                            while (k < reversedSequence.Count() && !OutOfBounds(i, j, k, 0, boardSize) && board[i + k][j].mark == reversedSequence[k])
                                k++;
                            if (k == reversedSequence.Count())
                                numberOfOccurences++;

                            k = 1;
                            while (k < reversedSequence.Count() && !OutOfBounds(i, j, 0, k, boardSize) && board[i][j + k].mark == reversedSequence[k])
                                k++;
                            if (k == reversedSequence.Count())
                                numberOfOccurences++;

                            k = 1;
                            while (k < reversedSequence.Count() && !OutOfBounds(i, j, k, k, boardSize) && board[i + k][j + k].mark == reversedSequence[k])
                                k++;
                            if (k == reversedSequence.Count())
                                numberOfOccurences++;

                            k = 1;
                            while (k < reversedSequence.Count() && !OutOfBounds(i, j, k, -k, boardSize) && board[i + k][j - k].mark == reversedSequence[k])
                                k++;
                            if (k == reversedSequence.Count())
                                numberOfOccurences++;

                        }
                    }
            }

            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                {
                    if (board[i][j].mark == sequence[0])
                    {
                        int k = 1;
                        while (k < sequence.Count() && !OutOfBounds(i, j, k, 0, boardSize) && board[i + k][j].mark == sequence[k])
                            k++;
                        if (k == sequence.Count())
                            numberOfOccurences++;

                        k = 1;
                        while (k < sequence.Count() && !OutOfBounds(i, j, 0, k, boardSize) && board[i][j + k].mark == sequence[k])
                            k++;
                        if (k == sequence.Count())
                            numberOfOccurences++;

                        k = 1;
                        while (k < sequence.Count() && !OutOfBounds(i, j, k, k, boardSize) && board[i + k][j + k].mark == sequence[k])
                            k++;
                        if (k == sequence.Count())
                            numberOfOccurences++;

                        k = 1;
                        while (k < sequence.Count() && !OutOfBounds(i, j, k, -k, boardSize) && board[i + k][j - k].mark == sequence[k])
                            k++;
                        if (k == sequence.Count())
                            numberOfOccurences++;

                    }
                }
            return numberOfOccurences;
        }

        float GetAttackValue(Tile[][] board, int boardSize, Tuple<int, int> coordinates, int urgencyDeterminedByCurrentDepth)
        {
            urgencyDeterminedByCurrentDepth = searchDepth - urgencyDeterminedByCurrentDepth + 1;

            float attackValue = 0;
            if (SearchForASequence(board, boardSize, new List<char> { mark, mark, mark, mark, mark }) != 0)
                return 1000000 * urgencyDeterminedByCurrentDepth;
            if (SearchForASequence(board, boardSize, new List<char> { ' ', mark, mark, mark, mark, ' ' }) != 0)
                attackValue += 100000.0f * urgencyDeterminedByCurrentDepth;
            attackValue += SearchForASequence(board, boardSize, new List<char> { ' ', ' ', mark, mark, mark, ' ', ' ' }) * 200.0f;

            attackValue += SearchForASequence(board, boardSize, new List<char> { opponentsMark, ' ', mark, mark, mark, ' ', ' '}) * 100.0f;
            attackValue += SearchForASequence(board, boardSize, new List<char> { opponentsMark, mark, mark, mark, ' ', ' ' }) * 50.0f;

            attackValue += SearchForASequence(board, boardSize, new List<char> { ' ', mark, ' ', mark, mark, ' ' }) * 75.0f;
            attackValue += SearchForASequence(board, boardSize, new List<char> { opponentsMark, mark, ' ', mark, mark, ' ', ' ' }) * 50.0f;
            attackValue += SearchForASequence(board, boardSize, new List<char> { ' ', ' ', mark, ' ', mark, mark, opponentsMark }) * 50.0f;

            attackValue += SearchForASequence(board, boardSize, new List<char> { ' ', ' ', mark, mark, ' ', ' ' }) * 50.0f;
            attackValue += SearchForASequence(board, boardSize, new List<char> { opponentsMark, mark, mark, ' ', ' ' }) * 20.0f;
            attackValue += SearchForASequence(board, boardSize, new List<char> { opponentsMark, mark, mark, ' ' }) * 10.0f;
            attackValue += SearchForASequence(board, boardSize, new List<char> { opponentsMark, mark, ' ' }) * 5.0f;

            return attackValue;
        }
        float GetDefenceValue(Tile[][] board, int boardSize, Tuple<int, int> coordinates, int depth)
        {
            int urgencyDeterminedByCurrentDepth = searchDepth - depth + 1;

            float defenceValue = 0;
            if (SearchForASequence(board, boardSize, new List<char> { opponentsMark, opponentsMark, opponentsMark, opponentsMark, opponentsMark }) != 0)
                return -1000000 * urgencyDeterminedByCurrentDepth;
            if (SearchForASequence(board, boardSize, new List<char> { opponentsMark, opponentsMark, opponentsMark, opponentsMark, ' ' }) != 0)
                defenceValue -= 100000.0f * urgencyDeterminedByCurrentDepth;
            if (SearchForASequence(board, boardSize, new List<char> { opponentsMark, opponentsMark, ' ', opponentsMark, opponentsMark }) != 0)
                defenceValue -= 10000.0f * urgencyDeterminedByCurrentDepth;

            defenceValue -= SearchForASequence(board, boardSize, new List<char> { ' ', ' ', opponentsMark, opponentsMark, opponentsMark, ' ', ' ' }) * 500.0f;

            int soooss = SearchForASequence(board, boardSize, new List<char> { ' ', opponentsMark, opponentsMark, opponentsMark, ' ', ' ' });

            defenceValue -=  soooss * 500.0f;
            defenceValue -= SearchForASequence(board, boardSize, new List<char> {/* mark,*/ opponentsMark, opponentsMark, opponentsMark, ' ', ' ' }) * 50.0f;

            defenceValue -= SearchForASequence(board, boardSize, new List<char> { ' ', opponentsMark, ' ', opponentsMark, opponentsMark, ' ' }) * 500.0f;
            defenceValue -= SearchForASequence(board, boardSize, new List<char> { /*mark,*/ opponentsMark, ' ', opponentsMark, opponentsMark, ' ', ' ' }) * 50.0f;
            defenceValue -= SearchForASequence(board, boardSize, new List<char> { ' ', ' ', opponentsMark, ' ', opponentsMark, opponentsMark/*, mark*/ }) * 50.0f;

            defenceValue -= SearchForASequence(board, boardSize, new List<char> { ' ', ' ', opponentsMark, opponentsMark, ' ', ' ' }) * 50.0f;
            defenceValue -= SearchForASequence(board, boardSize, new List<char> { /*mark,*/ opponentsMark, opponentsMark, ' ', ' ' }) * 20.0f;
            defenceValue -= SearchForASequence(board, boardSize, new List<char> { /*mark,*/ opponentsMark, opponentsMark, ' ' }) * 10.0f;
            
            return defenceValue;
        }

        TurnOption AssignHeuristicValueToCoordinates(Tile[][] board, int boardSize, Tuple<int, int> coordinates, int depth)
        {
            TurnOption turnOptionToBeReturned = new TurnOption();
            turnOptionToBeReturned.coordinates = coordinates;

            float attackValue = GetAttackValue(board, boardSize, coordinates, depth);
            float defenceValue = GetDefenceValue(board, boardSize, coordinates, depth);

            turnOptionToBeReturned.heuristicValue = attackValue + defenceValue;

            return turnOptionToBeReturned;
        }

        TurnOption FindMaximalHeuristicValue(List<TurnOption> options)
        {
            if (options.Count() == 0)
                return new TurnOption();

            TurnOption optionToBeReturned = options[0];
            for (int i = 1; i < options.Count(); i++)
            {
                if (options[i].heuristicValue > optionToBeReturned.heuristicValue)
                    optionToBeReturned = options[i];
            }
            return optionToBeReturned;
        }

        TurnOption FindMinimalHeuristicValue(List<TurnOption> options)
        {
            if (options.Count() == 0)
                return new TurnOption();

            TurnOption optionToBeReturned = options[0];
            for (int i = 1; i < options.Count(); i++)
            {
                if (options[i].heuristicValue < optionToBeReturned.heuristicValue)
                    optionToBeReturned = options[i];
            }
            return optionToBeReturned;
        }

        Tile[][] CreateNewBoard(Tile[][] board, int boardSize)
        {
            Tile[][] newBoard = new Tile[boardSize][];

            for (int i = 0; i < boardSize; i++)
            {
                newBoard[i] = new Tile[boardSize];
                for (int j = 0; j < boardSize; j++)
                {
                    newBoard[i][j] = new Tile(board[i][j].mark, board[i][j].x, board[i][j].y);

                    board[i][j].matchingMarksInRange.CopyTo(newBoard[i][j].matchingMarksInRange, 0);
                }
            }
            return newBoard;
        }

        TurnOption Minimax(Tile[][] board, int boardSize, int depth)
        {
            List<Tuple<int, int>> allConsideredTurnCoordinates = GetTilesToBeInspected(board, boardSize);

            if (allConsideredTurnCoordinates.Count() == 0)
            {
                TurnOption firstMove = new TurnOption();
                Random rng = new Random(Guid.NewGuid().GetHashCode());
                int x = rng.Next(boardSize / 3, 2 * boardSize / 3);
                int y = rng.Next(boardSize / 3, 2 * boardSize / 3);
                firstMove.coordinates = new Tuple<int, int>(x, y);
                return firstMove;
            }

            List<TurnOption> turnOptionsWithHeuristicValue = new List<TurnOption>();
            foreach (Tuple<int, int> coords in allConsideredTurnCoordinates)
            {
                Tile[][] alteredBoard = CreateNewBoard(board, boardSize);

                alteredBoard[coords.Item1][coords.Item2].mark = depth % 2 == 0 ? opponentsMark : mark;

                turnOptionsWithHeuristicValue.Add(AssignHeuristicValueToCoordinates(alteredBoard, boardSize, coords, depth));
            }

            turnOptionsWithHeuristicValue.Sort(new TurnOptionSorter());

            if (depth % 2 == 0)
                turnOptionsWithHeuristicValue.Reverse();
            if (depth == searchDepth * 2)
                return turnOptionsWithHeuristicValue[0];
            if (turnOptionsWithHeuristicValue[0].heuristicValue >= 1000000 || turnOptionsWithHeuristicValue[0].heuristicValue <= -1000000)
                return turnOptionsWithHeuristicValue[0];

            List<TurnOption> turnOptionsToBeBranched = new List<TurnOption>();
            for (int i = 0; i < branchingFactor && i < turnOptionsWithHeuristicValue.Count; i++)
                turnOptionsToBeBranched.Add(turnOptionsWithHeuristicValue[i]);

            List<TurnOption> scoresReturnedByMinimax = new List<TurnOption>();

            foreach (TurnOption turnOption in turnOptionsToBeBranched)
            {
                Tile[][] alteredBoard = CreateNewBoard(board, boardSize);

                alteredBoard[turnOption.coordinates.Item1][turnOption.coordinates.Item2].mark = depth % 2 == 0 ? opponentsMark : mark;

                turnOption.heuristicValue = Minimax(alteredBoard, boardSize, depth + 1).heuristicValue;
                scoresReturnedByMinimax.Add(turnOption);
            }
            if (depth % 2 == 0)
                return FindMinimalHeuristicValue(scoresReturnedByMinimax);

            return FindMaximalHeuristicValue(scoresReturnedByMinimax);


        }

        public Tuple<int, int> PlaceMark(Tile[][] board, int boardSize)
        {
            return Minimax(board, boardSize, 1).coordinates;
        }
    }
}
