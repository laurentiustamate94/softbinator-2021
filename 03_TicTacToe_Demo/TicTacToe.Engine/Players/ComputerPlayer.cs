using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Engine.Players
{
    public class ComputerPlayer : IPlayer
    {
        public string Name { get; set; }

        public ComputerPlayer(string name)
        {
            Name = name;
        }

        public void Insert0(int row, int column, int[,] moves)
        {
            FindMove(row, column, moves);
        }

        public void InsertX(int row, int column, int[,] moves)
        {
            //FindMove(row, column, moves);
        }

        private static void FindMove(int row, int column, int[,] moves)
        {
            for (int i = 0; i < 3; i++)
                if (isPossibleToMoveHere(moves, i, i))
                {
                    MoveHere(row, column, moves, i, i);
                    return;

                }

            for (int i = 1; i < 3; i++)
                if (isPossibleToMoveHere(moves, 0, i))
                {
                    MoveHere(row, column, moves, 0, i);
                    return;

                }

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (isPossibleToMoveHere(moves, i, j))
                    {
                        MoveHere(row, column, moves, i, j);
                        return;

                    }
        }

        private static bool isPossibleToMoveHere(int[,] moves, int i, int j)
        {
            return moves[i, j] == 0;
        }

        private static void MoveHere(int row, int column, int[,] moves, int i, int j)
        {
            moves[i, j] = 2;
        }

    }
}
