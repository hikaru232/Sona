using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sona.Models
{
    public partial class AppSetting : ObservableObject
    {
        public static AppSetting Default { get; } = new AppSetting();

        private AppSetting() { }

        [ObservableProperty] private int _masterVolume = 100;
    }
}
