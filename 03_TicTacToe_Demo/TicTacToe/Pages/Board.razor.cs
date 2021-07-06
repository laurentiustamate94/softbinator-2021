using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TicTacToe.Engine.Games;
using TicTacToe.Engine.Players;

namespace TicTacToe.Pages
{
    public partial class Board : ComponentBase
    {
        [Inject]
        public AppState AppState { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        private IGame game;

        protected override void OnInitialized()
        {
            if (AppState.IsSinglePlayerGame)
            {
                game = new SinglePlayerGame(new HumanPlayer(AppState.FirstPlayerName), new ComputerPlayer(AppState.SecondPlayerName));
            }
            else
            {
                game = new MultiPlayerGame(new HumanPlayer(AppState.FirstPlayerName), new HumanPlayer(AppState.SecondPlayerName));
            }
        }

        private void HandleClick(int row, int column)
        {
            var result = game.Insert(row, column);

            if(result.IsOver)
            {
                var playerName = result.IsWonByXPlayer
                    ? AppState.FirstPlayerName
                    : AppState.SecondPlayerName;

                NavigationManager.NavigateTo($"/winner?player={playerName}");
            }
        }
    }
}
