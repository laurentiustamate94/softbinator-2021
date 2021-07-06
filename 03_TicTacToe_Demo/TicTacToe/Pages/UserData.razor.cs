using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using TicTacToe.Models;

namespace TicTacToe.Pages
{
    public partial class UserData : ComponentBase
    {
        [Inject]
        public AppState AppState { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ILogger<UserData> Logger { get; set; }

        private UserDataModel userDataModel = new();

        private void HandleValidSubmit()
        {
            Logger.LogInformation("HandleValidSubmit called");

            AppState.FirstPlayerName = userDataModel.FirstPlayerName;
            AppState.SecondPlayerName = userDataModel.SecondPlayerName;

            NavigationManager.NavigateTo("/board");
        }
    }
}
