using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Engine.Players
{
    public interface IPlayer
    {
        string Name { get; set; }

        void InsertX(int row, int column, int[,] moves);

        void Insert0(int row, int column, int[,] moves);
    }
}
