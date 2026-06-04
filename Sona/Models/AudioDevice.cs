using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sona.Models
{
    public partial class AudioDevice : ObservableObject
    {
        [ObservableProperty] private string _name = "";
        [ObservableProperty] private string _id = "";
        [ObservableProperty] private bool _isSelected = false;
        //今後増やす予定
        //Volumeのみだが
    }
}
