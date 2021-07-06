using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Engine.Players;

namespace TicTacToe.Engine.Games
{
    public class MultiPlayerGame : Game
    {
        public MultiPlayerGame(HumanPlayer player1, HumanPlayer player2)
            :base(player1, player2)
        {
        }
    }
}
