using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Services
{
    public interface INotificationService
    {
        void ShowNotification(string title, string subtitle, string body);
    }
}
