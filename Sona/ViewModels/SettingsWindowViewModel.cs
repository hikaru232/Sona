using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sona.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sona.ViewModels
{
    public partial class SettingsWindowViewModel : ObservableObject
    {
        public event Action? CloseRequested;
        public ObservableCollection<AudioDevice> AvailableDevices { get; } = new();
        public ObservableCollection<AudioDevice> SelectedDevices { get; } = new();

    }
}
