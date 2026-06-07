using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sona.Models
{
    public partial class AppSettings : ObservableObject
    {
        public static AppSettings Default { get; } = new();

        [JsonConstructor]
        private AppSettings() { }

        [ObservableProperty] private int _masterVolume = 100;

        public ObservableCollection<string> SelectedDeviceIds { get; set; } = new();
    }
}
