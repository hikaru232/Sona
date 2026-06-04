using CommunityToolkit.Mvvm.ComponentModel;
using Sona.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sona.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        public ObservableCollection<AudioDevice> AvailableDevices { get; } = new();
        public ObservableCollection<AudioDevice> SelectedDevices { get; } = new();

        [ObservableProperty] private int _volume = 100;
    }
}
