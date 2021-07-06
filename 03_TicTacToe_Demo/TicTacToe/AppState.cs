using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class AppState
    {
        public bool IsSinglePlayerGame { get; set; }

        public bool IsMultiPlayerGame { get; set; }

        public string FirstPlayerName { get; set; }

        public string SecondPlayerName { get; set; }
    }
}
