using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Engine.Models;
using TicTacToe.Engine.Players;

namespace TicTacToe.Engine.Games
{
    public abstract class Game : IGame
    {
        public int turn = 0;
        public int[,] moves = new int[3, 3];
        public bool isWonByXPlayer;
        public bool isWonBy0Player;
        private const int numberOfRowsAndColumns = 3;
        private const int maximumNumberOfTurns = 9;

        protected Game(IPlayer player1, IPlayer player2)
        {
            Player1 = player1;
            Player2 = player2;

            moves = new int[3, 3];

            Restart();
        }

        public IPlayer Player1 { get; set; }

        public IPlayer Player2 { get; set; }

        public virtual GameStatus Insert(int row, int column)
        {
            UpdateMoves(row, column);
            turn++;

            return CheckGameStatus();
        }

        public char GetCharacter(int row, int column)
        {
            if (moves[row, column] == 1)
            {
                return 'X';
            }

            if (moves[row, column] == 2)
            {
                return 'O';
            }

            return ' ';
        }

        private GameStatus CheckGameStatus()
        {
            UpdateStatus();

            var isOver = IsOver();
            if (isOver)
            {
                Restart();
            }

            return new GameStatus
            {
                IsOver = isOver,
                IsWonByXPlayer = isWonByXPlayer,
                IsWonBy0Player = isWonBy0Player,
            };
        }

        private bool IsOver()
        {
            return isWonByXPlayer || isWonBy0Player || IsTide();
        }

        public bool IsTide()
        {
            return turn == maximumNumberOfTurns;
        }

        public void Restart()
        {
            int row, column;

            for (row = 0; row < numberOfRowsAndColumns; row++)
            {
                for (column = 0; column < numberOfRowsAndColumns; column++)
                {
                    moves[row, column] = 0;
                }
            }

            turn = 0;

            isWonBy0Player = false;
            isWonByXPlayer = false;
        }

        public void UpdateMoves(int row, int column)
        {
            if (IsXTurn())
            {
                Player1.InsertX(row, column, moves);
            }

            if (Is0Turn())
            {
                Player2.Insert0(row, column, moves);
            }
        }

        public void UpdateStatus()
        {
            for (int row = 0; row < numberOfRowsAndColumns; row++)
            {
                IsCompleteLine(row);
            }

            for (int column = 0; column < numberOfRowsAndColumns; column++)
            {
                IsCompleteColumn(column);
            }

            IsCompleteFirstDiagonal();
            IsCompleteSecondDiagonal();
        }

        public bool IsXTurn()
        {
            return turn % 2 == 0;
        }

        public bool Is0Turn()
        {
            return turn % 2 != 0;
        }

        private void IsCompleteSecondDiagonal()
        {
            if (moves[0, 2] == moves[1, 1] && moves[1, 1] == moves[2, 0])
            {
                if (moves[1, 1] == 2)
                {
                    isWonBy0Player = true;
                }

                if (moves[1, 1] == 1)
                {
                    isWonByXPlayer = true;
                }
            }
        }

        private void IsCompleteFirstDiagonal()
        {
            if (moves[0, 0] == moves[1, 1] && moves[1, 1] == moves[2, 2])
            {
                if (moves[0, 0] == 2)
                {
                    isWonBy0Player = true;
                }

                if (moves[0, 0] == 1)
                {
                    isWonByXPlayer = true;
                }
            }
        }

        private void IsCompleteColumn(int column)
        {
            if (moves[column, 0] == moves[column, 1] && moves[column, 1] == moves[column, 2])
            {
                if (moves[column, 0] == 2)
                {
                    isWonBy0Player = true;
                }

                if (moves[column, 1] == 1)
                {
                    isWonByXPlayer = true;
                }
            }
        }

        private void IsCompleteLine(int line)
        {
            if (moves[0, line] == moves[1, line] && moves[1, line] == moves[2, line])
            {
                if (moves[0, line] == 1)
                {
                    isWonByXPlayer = true;
                }

                if (moves[0, line] == 2)
                {
                    isWonBy0Player = true;
                }
            }
        }
    }
}
