using System;
using BlazorMauiApp1.Data;
using Microsoft.Maui.Controls;

namespace BlazorMauiApp1
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

			var counterService = ServiceProvider.GetService<CounterService>();
			counterService.ClickDelegate += OnCounterClicked;
		}

		int count = 0;
		private void OnCounterClicked(object sender, EventArgs e)
		{
			count++;
			count += 4;
			CounterLabel.Text = $"Current count: {count}";

			DotNetBotImage.RotateTo(10 * count, 1000);
		}
	}
}
