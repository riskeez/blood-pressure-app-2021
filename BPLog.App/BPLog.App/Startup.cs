using BPLog.App.Services;
using BPLog.App.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BPLog.App.ViewModels;

namespace BPLog.App
{
    public static class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<ITokenValidator, TokenValidator>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IRequestProvider, RequestProvider>();
            services.AddSingleton<IRestService, RestService>();

            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<MainViewModel>();

            ServiceProvider = services.BuildServiceProvider();
        }

        public static async Task OnAppStart()
        {
            var navigator = ServiceProvider.GetRequiredService<INavigationService>();

            if (!Settings.LoggedIn)
            {
                await navigator.GoToModalPage<LoginPage, LoginViewModel>();
            }
            else
            {
                await navigator.GoToRootLevelPage<MainPage, MainViewModel>();
            }
        }
    }
}
