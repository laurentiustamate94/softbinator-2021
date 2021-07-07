using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        int count = 0;
        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
            // count += 4;
            CounterLabel.Text = $"Current count: {count}";

            //DotNetBotImage.RotateTo(10 * count, 1000);
        }
    }
}
