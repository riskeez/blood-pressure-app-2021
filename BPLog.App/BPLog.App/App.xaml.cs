using BPLog.App.Services;
using BPLog.App.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BPLog.App
{
    public partial class App : Application
    {
        public App()
        {
            Startup.ConfigureServices();

            InitializeComponent();

            MainPage = new NavigationPage();
        }

        protected override async void OnStart()
        {
            await Startup.OnAppStart();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
