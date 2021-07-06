using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Engine.Models
{
    public class GameStatus
    {
        public bool IsOver { get; set; }

        public bool IsWonByXPlayer { get; set; }

        public bool IsWonBy0Player { get; set; }
    }
}
