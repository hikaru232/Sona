using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NAudio.CoreAudioApi;
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

        public AppSettings Settings => AppSettings.Default;
        
        public SettingsViewModel()
        {
            LoadDevices();
            DataService.LoadSettings();
            var saveIds = AppSettings.Default.SelectedDeviceIds;
            foreach (var device in AvailableDevices)
            {
                if (saveIds.Contains(device.Id))
                {
                    device.IsSelected = true;
                }
            }
        }

        private void LoadDevices()
        {
            var enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            AvailableDevices.Clear();
            foreach (var device in devices)
            {
                AvailableDevices.Add(new AudioDevice
                {
                    Name = device.FriendlyName,
                    Id = device.ID
                });
            }
        }

        [RelayCommand]
        public void SaveSettings()
        {
            DataService.SaveSettings(AppSettings.Default);
        }
    }
}
