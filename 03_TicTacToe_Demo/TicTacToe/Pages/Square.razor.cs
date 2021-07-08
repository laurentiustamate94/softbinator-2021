using Microsoft.AspNetCore.Components;

namespace TicTacToe.Pages
{
    public partial class Square : ComponentBase
    {
        [Parameter]
        public char CharacterFromProps { get; set; }

        [Parameter]
        public EventCallback HandleClickFromProps { get; set; }
    }
}
