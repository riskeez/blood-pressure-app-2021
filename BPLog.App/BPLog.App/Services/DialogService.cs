using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BPLog.App.Services
{
    public interface IDialogService
    {
        Task ShowMessage(string title, string msg);
        Task<bool> ShowMessage(string title, string msg, string btnOk, string btnCancel);
    }

    public class DialogService : IDialogService
    {
        private Page CurrentPage => Application.Current.MainPage;

        public async Task ShowMessage(string title, string msg)
        {
            await CurrentPage.DisplayAlert(title, msg, "OK");
        }

        public async Task<bool> ShowMessage(string title, string msg, string btnOk, string btnCancel)
        {
            return await CurrentPage.DisplayAlert(title, msg, btnOk, btnCancel);
        }
    }
}
