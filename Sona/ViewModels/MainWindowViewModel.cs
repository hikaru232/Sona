using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Sona.Models;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using System.Windows;
using Sona.Views;

namespace Sona.ViewModels
{
    //とりあえず継承はせず、一度ここで必要なものを全部書く。他で必要になりそうなときに初めてViewModelBaseを作って継承させる。
    partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty] private ObservableObject? _currentPage;

        private readonly SoundPadViewModel _soundPadPage = new();
        private readonly SettingsViewModel _settingsPage = new();

        public void CleanUp()
        {
            _soundPadPage.CleanUp();
        }

        public MainWindowViewModel()
        {
            _currentPage = _soundPadPage;
        }

        [RelayCommand]
        private void ShowSoundPad()
        {
            CurrentPage = _soundPadPage;
        }

        [RelayCommand]
        private void ShowSettingsPage()
        {
            CurrentPage = _settingsPage;
        }
        
    }
}
