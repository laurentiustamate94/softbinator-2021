using System.IO;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;
using TicTacToe.Services;

namespace TicTacToe.Maui
{
    public class Startup : IStartup
    {
        public void Configure(IAppHostBuilder appBuilder)
        {
            appBuilder
                .RegisterBlazorMauiWebView(typeof(Startup).Assembly)
                .UseMicrosoftExtensionsServiceProviderFactory()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                .ConfigureServices(services =>
                {
                    services.AddBlazorWebView();
                    services.AddTicTacToe();
#if WINDOWS
                    services.AddSingleton<INotificationService, WinUI.Services.NotificationService>();
#elif ANDROID
                    services.AddSingleton<INotificationService, Android.Services.NotificationService>();
#elif IOS
                    services.AddSingleton<INotificationService, iOS.Services.NotificationService>();
#endif
                })
                .ConfigureAppConfiguration(builder =>
                {
#if WINDOWS
                    builder.AddJsonFile(Path.Combine("Assets", "Resources", "Raw", "appsettings.winui.json"));
#endif
                })
                .ConfigureLifecycleEvents(lifecycle =>
                {
#if ANDROID
                    lifecycle.AddAndroid(d =>
                    {
                        d.OnBackPressed(activity =>
                        {
                            System.Diagnostics.Debug.WriteLine("Back button pressed!");
                        });
                    });
#endif
                });
        }
    }
}