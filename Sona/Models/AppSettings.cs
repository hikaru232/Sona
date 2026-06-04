using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sona.Models
{
    public partial class AppSettings : ObservableObject
    {
        public static AppSettings Default { get; } = new();

        private AppSettings() { }

        [ObservableProperty] private int _masterVolume = 100;
    }
}
