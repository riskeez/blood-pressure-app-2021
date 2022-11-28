using BPLog.App.Pages;
using BPLog.App.Services;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BPLog.App.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IRestService _restService;
        private readonly INavigationService _navigator;
        private readonly IDialogService _dlgService;

        public LoginViewModel(IRestService restService, INavigationService navigator, IDialogService dialogService)
        {
            _restService = restService ?? throw new ArgumentNullException(nameof(restService));
            _navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            _dlgService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                SetProperty(ref _login, value);
                LoginCommand.NotifyCanExecuteChanged();
                RegisterCommand.NotifyCanExecuteChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                LoginCommand.NotifyCanExecuteChanged();
                RegisterCommand.NotifyCanExecuteChanged();
            }
        }

        private RelayCommand _loginCmd;
        public RelayCommand LoginCommand
        {
            get => _loginCmd ?? (_loginCmd = new RelayCommand(async () => await TryLogin(), IsValidInput));
        }

        private RelayCommand _registerCmd;
        public RelayCommand RegisterCommand
        {
            get => _registerCmd ?? (_registerCmd = new RelayCommand(async () => await TryRegister(), IsValidInput));
        }

        private async Task TryLogin()
        {
            try
            {
                if (IsBusy) return;
                IsBusy = true;

                string dlgTitle = "Login";

                string token = await _restService.Login(_login, _password);
                if (string.IsNullOrWhiteSpace(token))
                {
                    await _dlgService.ShowMessage(dlgTitle, "Login failed. Check your credentials");
                    return;
                }

                Settings.Login = _login;
                Settings.Token = token;

                if (!Settings.LoggedIn)
                {
                    await _dlgService.ShowMessage(dlgTitle, "Your session is already expired");
                }

                await _navigator.GoToRootLevelPage<MainPage, MainViewModel>();
            }
            catch (Exception ex)
            {
                await _dlgService.ShowMessage("Connectivity Issue", "Check you network connection");
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task TryRegister()
        {
            try
            {
                if (IsBusy) return;
                IsBusy = true;

                string dlgTitle = "New user";

                bool isRegistered = await _restService.RegisterUser(_login, _password);
                if (isRegistered)
                {
                    await _dlgService.ShowMessage(dlgTitle, "User has been registered!");
                    return;
                }
                await _dlgService.ShowMessage(dlgTitle, "User is already exists");
            }
            catch (Exception ex)
            {
                await _dlgService.ShowMessage("Connectivity Issue", "Check you network connection");
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool IsValidInput() => !string.IsNullOrWhiteSpace(_login) && !string.IsNullOrWhiteSpace(_password);
    }
}
