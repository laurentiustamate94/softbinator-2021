using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Engine.Players
{
    public class HumanPlayer : IPlayer
    {
        public string Name { get; set; }

        public HumanPlayer(string name)
        {
            Name = name;
        }

        public void InsertX(int row, int column, int[,] moves)
        {
            //
        }

        public void Insert0(int row, int column, int[,] moves)
        {
            //
        }
    }
}
