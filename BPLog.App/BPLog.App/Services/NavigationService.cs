using BPLog.App.Pages;
using BPLog.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BPLog.App.Services
{
    /// <summary>
    /// Navigation service
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Go to previous page
        /// </summary>
        /// <returns></returns>
        Task GoBackAsync();

        /// <summary>
        /// Navigate to specific MODAL page resolving a view model (must be registered in services)
        /// </summary>
        /// <typeparam name="TPage"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <returns></returns>
        Task GoToModalPage<TPage, TViewModel>()
            where TPage : Page
            where TViewModel : ViewModelBase;

        /// <summary>
        /// Navigate to root level page that is inserted as a root page resolving a view model (must be registered in services)
        /// </summary>
        /// <typeparam name="TPage"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <returns></returns>
        Task GoToRootLevelPage<TPage, TViewModel>()
            where TPage : Page
            where TViewModel : ViewModelBase;
    }

    public class NavigationService : INavigationService
    {
        private Page RootPage => App.Current.MainPage;

        public async Task GoBackAsync()
        {
            if (RootPage.Navigation.ModalStack.Count > 0)
            {
                await RootPage.Navigation.PopModalAsync(true);
            }
            else
            {
                await RootPage.Navigation.PopAsync(true);
            }
        }

        public async Task GoToRootLevelPage<TPage, TViewModel>()
            where TPage : Page
            where TViewModel : ViewModelBase
        {
            Page page = CreatePage<TPage, TViewModel>();

            
            if (RootPage.Navigation.NavigationStack.Count > 0)
            {
                RootPage.Navigation.InsertPageBefore(page, RootPage.Navigation.NavigationStack[0]);
                await RootPage.Navigation.PopToRootAsync();
            }
            else
            {
                await RootPage.Navigation.PushAsync(page);
            }
        }

        public async Task GoToModalPage<TPage, TViewModel>()
            where TPage : Page
            where TViewModel : ViewModelBase
        {
            Page page = CreatePage<TPage, TViewModel>();

            await RootPage.Navigation.PushModalAsync(page);
        }

        private Page CreatePage<TPage, TViewModel>()
            where TPage : Page
            where TViewModel : ViewModelBase
        {
            Page page = (Page)Activator.CreateInstance(typeof(TPage), new object[] { });
            page.BindingContext = ViewModelLocator.Resolve<TViewModel>();

            return page;
        }
    }
}
