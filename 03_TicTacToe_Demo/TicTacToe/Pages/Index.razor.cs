using Microsoft.AspNetCore.Components;

namespace TicTacToe.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public AppState AppState { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private void NewSinglePlayerGame()
        {
            AppState.IsSinglePlayerGame = true;
            AppState.IsMultiPlayerGame = false;

            NavigationManager.NavigateTo("/userData");
        }

        private void NewMultiPlayerGame()
        {
            AppState.IsSinglePlayerGame = false;
            AppState.IsMultiPlayerGame = true;


            NavigationManager.NavigateTo("/userData");
        }
    }
}
