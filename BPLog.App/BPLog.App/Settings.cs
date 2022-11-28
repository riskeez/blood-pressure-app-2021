using BPLog.App.Services;
using Xamarin.Essentials;
using BPLog.App.ViewModels;

namespace BPLog.App
{
    public class Settings
    {
        public static string URL
        {
            get => Preferences.Get(nameof(URL), "https://mybplogapi.azurewebsites.net/");
            set => Preferences.Set(nameof(URL), value);
        }

        public static string Login
        {
            get => Preferences.Get(nameof(Login), "demo");
            set => Preferences.Set(nameof(Login), value);
        }

        public static string Token
        {
            get => Preferences.Get(nameof(Token), string.Empty);
            set => Preferences.Set(nameof(Token), value);
        }

        public static bool LoggedIn
        {
            get
            {
                var validator = ViewModelLocator.Resolve<ITokenValidator>();
                return validator.IsValid(Token);
            }
        }
    }
}
