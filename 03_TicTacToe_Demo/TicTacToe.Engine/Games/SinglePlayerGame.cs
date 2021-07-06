using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Engine.Players;

namespace TicTacToe.Engine.Games
{
    public class SinglePlayerGame : Game
    {
        public SinglePlayerGame(HumanPlayer player1, ComputerPlayer player2)
            : base(player1, player2)
        {
        }
    }
}
