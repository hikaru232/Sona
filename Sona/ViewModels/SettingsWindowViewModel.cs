using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sona.Models;

namespace Sona.ViewModels
{
    public partial class SettingsWindowViewModel : ObservableObject
    {
        public ObservableCollection<AudioDevice> AvailableDevices { get; } = new();
        public ObservableCollection<AudioDevice> SelectedDevices { get; } = new();

        [ObservableProperty] private int _volume = 100;
    }
}
