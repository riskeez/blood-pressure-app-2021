using BPLog.App.Models;
using BPLog.App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace BPLog.App.ViewModels
{
    public abstract class ViewModelBase : ObservableRecipient
    {
        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        private string busyMessage;
        public string BusyMessage
        {
            get => busyMessage;
            set => SetProperty(ref busyMessage, value);
        }

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
    }
}
