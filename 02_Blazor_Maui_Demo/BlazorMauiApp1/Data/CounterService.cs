using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMauiApp1.Data
{
    public class CounterService
    {
        public Action<object, EventArgs> ClickDelegate { get; internal set; }
    }
}
