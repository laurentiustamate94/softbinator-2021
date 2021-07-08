# Disclaimer
This workshop is based on the [.NET 6 Preview 5](https://devblogs.microsoft.com/aspnet/asp-net-core-updates-in-net-6-preview-5/) and will not be update to future version, at least for now. 

# Getting started
To get started with Blazor and MAUI development in .NET 6 Preview 5, [install the .NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0). Next, if you’re on Windows using Visual Studio, we recommend [installing the latest preview of Visual Studio 2019 16.11](http://visualstudio.com/preview). If you’re on macOS, we recommend [installing the latest preview of Visual Studio 2019 for Mac 8.10](https://docs.microsoft.com/visualstudio/mac/install-preview). Next, follow the [.NET MAUI getting started guide](https://docs.microsoft.com/dotnet/maui/get-started/installation).
To verify your development environment, and install any missing components, use the maui-check utility. Install this utility using the following .NET CLI command:
```powershell
dotnet tool install -g redth.net.maui.check
```

Then, run maui-check:
```powershell
maui-check
```
If any tools and SDKs required by .NET MAUI are missing, maui-check will install them.

# Repository structure

    .
    ├── 01_Maui_Demo            # Basic Maui project showcasing animations
    ├── 02_Blazor_Maui_Demo     # Blazor Maui project showcasing interoperability
    ├── 03_TicTacToe_Demo       # Sample project used in the hands-on demo
    ├── LICENSE
    ├── softbinator-2021.pdf
    └── README.md

# 01_Maui_Demo

In this section of the workshop, we will see how to create a basic MauiApp. We will go through the folder structure, hot reload and basic animation concepts.

To rotate the DotNetBotImage we will use the command below to rotate it 10 degrees times the number of clicks we did within 1 second.
```csharp
DotNetBotImage.RotateTo(10 * count, 1000);
```

# 02_Blazor_Maui_Demo

In this section of the workshop, we will see how to create a basic MauiApp that relies on Blazor for UI rendering. We will go through the folder structure, hot reload and basic animation concepts.

We will rely on the DI container to achieve interoperability using the class below.
```csharp
using Microsoft.Maui;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace BlazorMauiApp1
{
	public static class ServiceProvider
	{
		public static TService GetService<TService>()
			=> Current.GetService<TService>();

		public static IServiceProvider Current
			=>
#if WINDOWS10_0_17763_0_OR_GREATER
			MauiWinUIApplication.Current.Services;
#elif ANDROID
			MauiApplication.Current.Services;
#elif IOS || MACCATALYST
			MauiUIApplicationDelegate.Current.Services;
#else
			null;
#endif
	}
}
```

We'll then define a service that exposes a delegate to which we will hook the action event on the MauiApp. We will link this in the Razor Counter component and see what happens :bowtie:

# 03_TicTacToe_Demo

This is the part where things get interested. Just a heads up that this section may cause anxiety, frustration and depression as most preview code does.

If you want to follow the workshop and code section by section, I suggest switching to the [workshop branch](https://github.com/laurentiustamate94/softbinator-2021/tree/workshop).

## TLDR what we'll be actually doing
1. One solution that serves Windows, macOS, Android, iOS and Web
2. Razor library that can be reused in MAUI and Blazor WebAssembly
3. State, Props and event handling
4. Leveraging existing NuGet packages
5. Accessing native components from Razor components
6. Feature management per platform

### Single solution for cross-platform development using Razor libraries

We'll start from the [workshop branch](https://github.com/laurentiustamate94/softbinator-2021/tree/workshop) with the solution and project already created. There was a bit of a struggle to hook everything up so no need to do this in the workshop. The project structure is similar with the BlazorMauiApp except we added the Web project that runs a Kestrel web server.

We already have the TicTacToe engine implementation from [a previous workshop](https://github.com/microsoft-dx/ms-monday-uwp-tic-tac-toe/) that I did so we'll bundle the code from there. You could say this is lazyness, but this code is 3 years old - what I'm trying to prove is that we can take old code (from an UWP project in this case) and retarget it to .NET 6 class library.

Interface segregation is crucial here because we want to play a different type of game depending on what we will select from the UI. For this, we have an `IGame` interface and an `IPlayer` interface, 2 game implementations (Singleplayer and Multiplayer) and 2 player implementations (human and computer).

### State, Props and event handling

Every SPA (Single Page Application) needs a global state to store things like tokens, settings and other bunch of stuff. The current best practice is to define a singleton state class that update when needed.

```csharp
// TicTacToe.AppState.cs

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
```

We also need to add this to the container, so we'll add the following line in `ServiceCollectionExtensions.cs`
```csharp
services.AddSingleton<AppState>();
```

Let's modify the only blazor component we have so far and add some logic to it.

```csharp
// TicTacToe.Pages.Index.razor

@page "/"

<h2>Tic Tac Toe game</h2>
<hr />

<div>
	<button class="btn btn-primary" @onclick="NewSinglePlayerGame">
		New single player game
	</button>
</div>
<br />
<div>
	<button class="btn btn-primary" @onclick="NewMultiPlayerGame">
		New multi player game
	</button>
</div>
```

Because Blazor is based on Razor components, we can either add the C# code directly in the component, or have a separate "backend" code file in which we do all the logic. I personally prefer this way, because it leaves the razor file with only the rendering responsability.

```csharp
// TicTacToe.Pages.Index.razor.cs

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
```

Currently, the only way to inject dependencies in a components is via the `[Inject]` attribute. Probably by the time .NET 6 becomes GA we will have the posiblity to inject them via the constructor to keep consistency with other platforms.

Our players need a name, which is why we need to collect this information. What better way to collect it if not a form. There are certain Razor components out-of-the-box that we can use to achieve this - `EditForm` and `InputText`.

```csharp
// TicTacToe.Pages.UserData.razor

@page "/userData"

<h2>Tic Tac Toe game</h2>
<hr />

<EditForm Model="@userDataModel" OnValidSubmit="@HandleValidSubmit">
	<DataAnnotationsValidator />
	<ValidationSummary />

	<div class="form-group">
		<label for="firstPlayerName">First player name</label>
		<InputText id="firstPlayerName" class="form-control" @bind-Value="userDataModel.FirstPlayerName" aria-describedby="firstPlayerName" />
	</div>
	<div class="form-group">
		<label for="secondPlayerName">Second player name</label>
		<InputText id="secondPlayerName" class="form-control" @bind-Value="userDataModel.SecondPlayerName" aria-describedby="secondPlayerName" />
	</div>
	<button type="submit" class="btn btn-primary">Submit</button>
</EditForm>
```

```csharp
// TicTacToe.Pages.UserData.razor.cs

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
```

The form will need a model to bind the information we input. For this we will create the `UserDataModel.cs` class that will hold the first player and second player names. 

```csharp
// TicTacToe.Models.UserDataModel.cs

using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Models
{
    public class UserDataModel
    {
        [Required]
        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "First player name")]
        public string FirstPlayerName { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "Second player name")]
        public string SecondPlayerName { get; set; }
    }
}
```

Our Tic-Tac-Toe board consists of 9 squares in a 3x3 matrix. We can define a `Square` component that we render within a `Board` component. When styling components we can try the default option to include everything in a big `style.css` file or we can use CSS-isolation as every SPA framework do.

```csharp
// TicTacToe.Pages.Square.razor

<div class="square" @onclick="HandleClickFromProps">@CharacterFromProps</div>
```

Then we need the backend class in which we define the parameters (props in other SPA frameworks) that we can use in the view component.

```csharp
// TicTacToe.Pages.Square.razor.cs

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
```

And to bind the CSS to this component we create the `Square.razor.css` file inside the `Pages` folder from the `TicTacToe` project.

```css
.square {
    background-color: rgba(255, 255, 255, 0.8);
    border: 1px solid rgba(0, 0, 0, 0.8);
    width: 60px;
    height: 60px;
    font-size: 30px;
    text-align: center;
    vertical-align: middle;
    line-height: 60px;
    border-radius: 10%;
    cursor: pointer;
}

.square:hover {
    background-color: rgba(106, 202, 9, 0.8);
}
```

In the `Board` component we will hold the game instance and pass the click event handler and the character to render to props.

```csharp
// TicTacToe.Pages.Board.razor

@page "/board"

<h3>Next player: "@(game.IsXTurn() ? 'X' : 'O')"</h3>

<div class="board">
	@{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				int row = i;
				int column = j;

				int squareNumber = (3 * row) + column;
		  
				<Square	@key=squareNumber
						CharacterFromProps=@game.GetCharacter(row, column)
						HandleClickFromProps="@(() => HandleClick(row, column))" />
			}
		}
	}
</div>
```

It's important to use local variables, since the `HandleClick` method is not called until the square is clicked, at which point the value of `i` and `j` is already equal to 3.

```csharp
// TicTacToe.Pages.Board.razor.cs

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

            if (result.IsOver)
            {
                var playerName = result.IsWonByXPlayer
                    ? AppState.FirstPlayerName
                    : AppState.SecondPlayerName;

                NavigationManager.NavigateTo($"/winner?player={playerName}");
            }
        }
    }
}
```

And to bind the CSS to this component we create the `Board.razor.css` file inside the `Pages` folder from the `TicTacToe` project.

```css
.board {
    display: grid;
    grid-template-columns: auto auto auto;
    background-color: #0a8efa;
    padding: 10px;
    width: 200px;
    height: 200px;
    border-radius: 10%;
}

button {
    border-radius: 10%;
    margin: 10px;
}
```

For the last part we want a winning screen for the player that wins. We can put the information in the state as we did with the game selection type... but that will not be fun, would it?

### Leveraging existing NuGet packages

We can pass the player name on the query parameters and this way we can leverage existing NuGet packages written in .NET Standard 2.0 . We need to add the `Microsoft.AspNetCore.WebUtilities` package to `TicTacToe.csproj`

```csharp
<PackageReference Include="Microsoft.AspNetCore.WebUtilities" Version="2.2.0" />
```

And we create the `Winner.razor` component

```csharp
// TicTacToe.Pages.Winner.razor

@page "/winner"
@inject NavigationManager NavigationManager
@using Microsoft.AspNetCore.WebUtilities

<h3>Winner</h3>

Congratulations @playerName

<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trophy" viewBox="0 0 16 16">
	<path d="M2.5.5A.5.5 0 0 1 3 0h10a.5.5 0 0 1 .5.5c0 .538-.012 1.05-.034 1.536a3 3 0 1 1-1.133 5.89c-.79 1.865-1.878 2.777-2.833 3.011v2.173l1.425.356c.194.048.377.135.537.255L13.3 15.1a.5.5 0 0 1-.3.9H3a.5.5 0 0 1-.3-.9l1.838-1.379c.16-.12.343-.207.537-.255L6.5 13.11v-2.173c-.955-.234-2.043-1.146-2.833-3.012a3 3 0 1 1-1.132-5.89A33.076 33.076 0 0 1 2.5.5zm.099 2.54a2 2 0 0 0 .72 3.935c-.333-1.05-.588-2.346-.72-3.935zm10.083 3.935a2 2 0 0 0 .72-3.935c-.133 1.59-.388 2.885-.72 3.935zM3.504 1c.007.517.026 1.006.056 1.469.13 2.028.457 3.546.87 4.667C5.294 9.48 6.484 10 7 10a.5.5 0 0 1 .5.5v2.61a1 1 0 0 1-.757.97l-1.426.356a.5.5 0 0 0-.179.085L4.5 15h7l-.638-.479a.501.501 0 0 0-.18-.085l-1.425-.356a1 1 0 0 1-.757-.97V10.5A.5.5 0 0 1 9 10c.516 0 1.706-.52 2.57-2.864.413-1.12.74-2.64.87-4.667.03-.463.049-.952.056-1.469H3.504z" />
</svg>

<br />

@code {
	string playerName = string.Empty;

	protected override void OnInitialized()
	{
		var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
		if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("player", out var queryParameterPlayerName))
		{
			playerName = queryParameterPlayerName;
		}
	}
}
```

### Accessing native components from Razor components

Let's step it up a notch and send notifications when on Windows when a player wins. For this we'll define an interface and implement with specific code per platform.

```csharp
// TicTacToe.Services.INotificationService.cs

namespace TicTacToe.Services
{
    public interface INotificationService
    {
        void ShowNotification(string title, string subtitle, string body);
    }
}
```

We'll then add the Windows implementation.

```csharp
// TicTacToe.Services.INotificationService.cs

using System;
using Microsoft.Toolkit.Uwp.Notifications;
using TicTacToe.Services;

namespace TicTacToe.Maui.WinUI.Windows
{
    public class NotificationService : INotificationService
	{
		public void ShowNotification(string title, string subtitle, string body)
		{
			new ToastContentBuilder()
				.AddToastActivationInfo(null, ToastActivationType.Foreground)
				.AddAppLogoOverride(new Uri("ms-appx:///Assets/dotnet_bot.png"))
				.AddText(title, hintStyle: AdaptiveTextStyle.Header)
				.AddText(subtitle, hintStyle: AdaptiveTextStyle.Subtitle)
				.AddText(body, hintStyle: AdaptiveTextStyle.Body)
				.Show();
		}
	}
}
```

And for the sake of the example, we'll add a mock implementation for the Web platform.

```csharp
// TicTacToe.Web.Services.NotificationService.cs

using System;
using Microsoft.Toolkit.Uwp.Notifications;
using TicTacToe.Services;

namespace TicTacToe.Maui.WinUI.Windows
{
    public class NotificationService : INotificationService
	{
		public void ShowNotification(string title, string subtitle, string body)
		{
			new ToastContentBuilder()
				.AddToastActivationInfo(null, ToastActivationType.Foreground)
				.AddAppLogoOverride(new Uri("ms-appx:///Assets/dotnet_bot.png"))
				.AddText(title, hintStyle: AdaptiveTextStyle.Header)
				.AddText(subtitle, hintStyle: AdaptiveTextStyle.Subtitle)
				.AddText(body, hintStyle: AdaptiveTextStyle.Body)
				.Show();
		}
	}
}
```

One last thing that we need to add is in the container records in `TicTacToe.Web.Program.cs`

```csharp
builder.Services.AddSingleton<INotificationService, NotificationService>();
```

and `TicTacToe.Maui.Startup.cs`

```csharp
                .ConfigureServices(services =>
                {
                    services.AddBlazorWebView();
                    services.AddTicTacToe();
#if WINDOWS
                    services.AddSingleton<INotificationService, WinUI.Windows.NotificationService>();
#endif
                })
```

The `Winner.razor` component should look something like this.

```csharp
// TicTacToe.Pages.Winner.razor

@page "/winner"
@inject NavigationManager NavigationManager
@inject INotificationService NotificationService
@using Microsoft.AspNetCore.WebUtilities
@using TicTacToe.Services

<h3>Winner</h3>

Congratulations @playerName

<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trophy" viewBox="0 0 16 16">
	<path d="M2.5.5A.5.5 0 0 1 3 0h10a.5.5 0 0 1 .5.5c0 .538-.012 1.05-.034 1.536a3 3 0 1 1-1.133 5.89c-.79 1.865-1.878 2.777-2.833 3.011v2.173l1.425.356c.194.048.377.135.537.255L13.3 15.1a.5.5 0 0 1-.3.9H3a.5.5 0 0 1-.3-.9l1.838-1.379c.16-.12.343-.207.537-.255L6.5 13.11v-2.173c-.955-.234-2.043-1.146-2.833-3.012a3 3 0 1 1-1.132-5.89A33.076 33.076 0 0 1 2.5.5zm.099 2.54a2 2 0 0 0 .72 3.935c-.333-1.05-.588-2.346-.72-3.935zm10.083 3.935a2 2 0 0 0 .72-3.935c-.133 1.59-.388 2.885-.72 3.935zM3.504 1c.007.517.026 1.006.056 1.469.13 2.028.457 3.546.87 4.667C5.294 9.48 6.484 10 7 10a.5.5 0 0 1 .5.5v2.61a1 1 0 0 1-.757.97l-1.426.356a.5.5 0 0 0-.179.085L4.5 15h7l-.638-.479a.501.501 0 0 0-.18-.085l-1.425-.356a1 1 0 0 1-.757-.97V10.5A.5.5 0 0 1 9 10c.516 0 1.706-.52 2.57-2.864.413-1.12.74-2.64.87-4.667.03-.463.049-.952.056-1.469H3.504z" />
</svg>

<br />

@code {
	string playerName = string.Empty;

	protected override void OnInitialized()
	{
		var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
		if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("player", out var queryParameterPlayerName))
		{
			playerName = queryParameterPlayerName;
			NotificationService.ShowNotification("TicTacToe", "Winner Winner Chicken Dinner", "You're using Blazor and .NET MAUI like pro! 😎");
		}
	}
}
```

### Feature management per platform

Oh yeah! There is one last thing. A good thing every SPA platform should have is a feature manangement system. Normally in production we need to first deliver the code, enable it to a bunch of users and then release the feature GA. For this, we will use the [feature management from AspNetCore](https://docs.microsoft.com/en-us/azure/azure-app-configuration/quickstart-feature-flag-aspnet-core).

We need to add the `Microsoft.FeatureManagement` package to `TicTacToe.csproj`

```csharp
<PackageReference Include="Microsoft.FeatureManagement" Version="2.3.0" />
```

We also need to add this to the container, so we'll add the following line in `ServiceCollectionExtensions.cs`
```csharp
services.AddFeatureManagement();
```

Based on the [documentation](https://docs.microsoft.com/en-us/azure/azure-app-configuration/quickstart-feature-flag-aspnet-core), we can control the features either via [Azure App Configuration](https://docs.microsoft.com/en-us/azure/azure-app-configuration/overview) or via json files. We will do the latter for this workshop and define the following json

```json
{
  "FeatureManagement": {
    "renderButton": false
  }
}
```

To which we'll hook into the Web project by adding the `appsettings.json` and by adding `appsettings.winui.json` in the `wwwroot` folder of the `TicTacToe.Maui` project it as an asset in the `TicTacToe.Maui.WinUI.csproj`.

```csharp
<MauiAsset Include="..\TicTacToe.Maui\wwwroot\appsettings.winui.json" Link="Resources\Raw\%(Filename)%(Extension)" />
```

The `Startup.cs` file will need to configure the app configuration method on the builder

```csharp
                .ConfigureAppConfiguration(builder =>
                {
#if WINDOWS
                    builder.AddJsonFile(Path.Combine("Assets", "Resources", "Raw", "appsettings.winui.json"));
#endif
                })
```

The `Winner.razor` component should look something like this.

```csharp
// TicTacToe.Pages.Winner.razor

@page "/winner"
@inject NavigationManager NavigationManager
@inject INotificationService NotificationService
@inject IFeatureManager FeatureManager
@using Microsoft.AspNetCore.WebUtilities
@using Microsoft.FeatureManagement
@using TicTacToe.Services

<h3>Winner</h3>

Congratulations @playerName

<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trophy" viewBox="0 0 16 16">
	<path d="M2.5.5A.5.5 0 0 1 3 0h10a.5.5 0 0 1 .5.5c0 .538-.012 1.05-.034 1.536a3 3 0 1 1-1.133 5.89c-.79 1.865-1.878 2.777-2.833 3.011v2.173l1.425.356c.194.048.377.135.537.255L13.3 15.1a.5.5 0 0 1-.3.9H3a.5.5 0 0 1-.3-.9l1.838-1.379c.16-.12.343-.207.537-.255L6.5 13.11v-2.173c-.955-.234-2.043-1.146-2.833-3.012a3 3 0 1 1-1.132-5.89A33.076 33.076 0 0 1 2.5.5zm.099 2.54a2 2 0 0 0 .72 3.935c-.333-1.05-.588-2.346-.72-3.935zm10.083 3.935a2 2 0 0 0 .72-3.935c-.133 1.59-.388 2.885-.72 3.935zM3.504 1c.007.517.026 1.006.056 1.469.13 2.028.457 3.546.87 4.667C5.294 9.48 6.484 10 7 10a.5.5 0 0 1 .5.5v2.61a1 1 0 0 1-.757.97l-1.426.356a.5.5 0 0 0-.179.085L4.5 15h7l-.638-.479a.501.501 0 0 0-.18-.085l-1.425-.356a1 1 0 0 1-.757-.97V10.5A.5.5 0 0 1 9 10c.516 0 1.706-.52 2.57-2.864.413-1.12.74-2.64.87-4.667.03-.463.049-.952.056-1.469H3.504z" />
</svg>

<br />

@{
	if (shouldRenderButton)
	{
		<p>Current count: @currentCount</p>

		<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>
	}
}

@code {
	bool shouldRenderButton = false;
	string playerName = string.Empty;
	int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
    }

	protected override void OnInitialized()
	{
		var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
		if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("player", out var queryParameterPlayerName))
		{
			playerName = queryParameterPlayerName;
			NotificationService.ShowNotification("TicTacToe", "Winner Winner Chicken Dinner", "You're using Blazor and .NET MAUI like pro! 😎");
		}
	}

	protected override async Task OnParametersSetAsync()
	{
		shouldRenderButton = await FeatureManager.IsEnabledAsync("renderButton");

		await base.OnParametersSetAsync();
	}
}
```

## Closing thoughts

That was a blast! Thank you for taking the time to go through the entire workshop :rocket:

# Notable mentions

Shout-out to the awesome open-source community that provided inspiration for making of this workshop.

- https://github.com/microsoft-dx/ms-monday-uwp-tic-tac-toe/
- https://dev.to/ysflghou/build-tic-tac-toe-game-with-blazor-webassembly-52ih
- https://github.com/danroth27/BlazorWeather
- https://github.com/davidortinau/WeatherTwentyOne
- https://docs.microsoft.com/en-us/aspnet/core/blazor/
- https://channel9.msdn.com/Shows/XamarinShow/Introduction-to-NET-MAUI-Blazor--The-Xamarin-Show
- https://devblogs.microsoft.com/dotnet/announcing-net-maui-preview-5
- https://medium.com/young-coder/can-microsoft-unify-desktop-and-mobile-34f08348178b
- https://medium.com/tastejs-blog/yet-another-framework-syndrome-yafs-cf5f694ee070
