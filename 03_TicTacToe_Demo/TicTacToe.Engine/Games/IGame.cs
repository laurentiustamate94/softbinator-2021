using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Engine.Models;

namespace TicTacToe.Engine.Games
{
    public interface IGame
    {
        void UpdateStatus();

        void UpdateMoves(int row, int column);

        GameStatus Insert(int row, int column);

        char GetCharacter(int row, int column);

        bool IsTide();

        bool IsXTurn();

        bool Is0Turn();

        void Restart();
    }
}
