using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using TicTacToe.Services;

namespace TicTacToe.Maui.Android.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            this.logger = logger;
        }

        public void ShowNotification(string title, string subtitle, string body)
        {
            logger.LogInformation("not implemeted yet!");
        }
    }
}
