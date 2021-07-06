using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace TicTacToe.Pages
{
    public partial class Square : ComponentBase
    {
        [Parameter]
        public char valueFromProps { get; set; }

        [Parameter]
        public EventCallback ChangeValueFromProps { get; set; }
    }
}
