using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Engine.Models;
using TicTacToe.Engine.Players;

namespace TicTacToe.Engine.Games
{
    public class SinglePlayerGame : Game
    {
        public SinglePlayerGame(HumanPlayer player1, ComputerPlayer player2)
            : base(player1, player2)
        {
        }

        public override GameStatus Insert(int row, int column)
        {
            var humanPlayerMove = base.Insert(row, column);

            if (humanPlayerMove.IsOver)
            {
                return humanPlayerMove;
            }

            return base.Insert(row, column);
        }
    }
}
