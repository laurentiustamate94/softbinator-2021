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
            FindMove(2, moves);
        }

        public void InsertX(int row, int column, int[,] moves)
        {
            FindMove(1, moves);
        }

        private static void FindMove(int digit, int[,] moves)
        {
            for (int i = 0; i < 3; i++)
            {
                if (IsPossibleToMoveHere(moves, i, i))
                {
                    MoveHere(digit, moves, i, i);
                    return;

                }
            }

            for (int i = 1; i < 3; i++)
            {
                if (IsPossibleToMoveHere(moves, 0, i))
                {
                    MoveHere(digit, moves, 0, i);
                    return;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (IsPossibleToMoveHere(moves, i, j))
                    {
                        MoveHere(digit, moves, i, j);
                        return;

                    }
                }
            }
        }

        private static bool IsPossibleToMoveHere(int[,] moves, int i, int j)
        {
            return moves[i, j] == 0;
        }

        private static void MoveHere(int digit, int[,] moves, int i, int j)
        {
            moves[i, j] = digit;
        }
    }
}
