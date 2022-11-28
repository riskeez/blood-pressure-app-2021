using BPLog.App.Models;
using BPLog.App.Services;
using BPLog.App.ViewModels;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;

namespace BPLog.App.Pages
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IRestService _restService;
        private readonly IDialogService _dlgService;

        public MainViewModel(IRestService restService, IDialogService dialogService)
        {
            _restService = restService ?? throw new ArgumentNullException(nameof(restService));
            _dlgService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        public async Task Init()
        {
            LatestMeasurement = await _restService.GetLastBloodPressure();
        }

        private BloodPressure _latestBloodPressure;
        public BloodPressure LatestMeasurement
        {
            get => _latestBloodPressure;
            set
            {
                SetProperty(ref _latestBloodPressure, value);
                OnPropertyChanged(nameof(IsHighPressure));
            }
        }

        public bool IsHighPressure => _latestBloodPressure != null && _latestBloodPressure.IsHighPressure;

        ObservableRangeCollection<BloodPressure> _list;
        public ObservableRangeCollection<BloodPressure> List
        {
            get => _list ?? (_list = new ObservableRangeCollection<BloodPressure>());
        }

        private int _systolic = 120;
        public int Systolic
        {
            get => _systolic;
            set
            {
                SetProperty(ref _systolic, value);
                AddRecordCommand.NotifyCanExecuteChanged();
            }
        }

        private int _diastolic = 60;
        public int Diastolic
        {
            get => _diastolic;
            set
            {
                SetProperty(ref _diastolic, value);
                AddRecordCommand.NotifyCanExecuteChanged();
            }
        }

        private RelayCommand _addRecordCmd;
        public RelayCommand AddRecordCommand
        {
            get => _addRecordCmd ?? (_addRecordCmd = new RelayCommand(async () => await TryAddBloodPressure(), IsValidInput));
        }

        private async Task TryAddBloodPressure()
        {
            try
            {
                if (IsBusy) return;
                IsBusy = true;

                var record = new BloodPressure { DateUTC = DateTime.UtcNow, Diastolic = _diastolic, Systolic = _systolic };
                bool result = await _restService.SaveBloodPressure(record);

                if (result)
                {
                    await _dlgService.ShowMessage("Success", "New blood pressure has been added");

                    LatestMeasurement = record;
                }
                else
                {
                    await _dlgService.ShowMessage("Fail", "Something went wrong :( ");
                }
            }
            catch (Exception ex)
            {
                await _dlgService.ShowMessage("Connectivity Issue", "Check you network connection or try to login again");
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool IsValidInput() => (_systolic > 0 && _diastolic < 200) && (_diastolic > 0 && _diastolic < 200);
    }


}
