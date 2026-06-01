using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sona.Models
{
    public partial class Song : ObservableObject
    {
        [ObservableProperty] private string _filePath = "";
        [ObservableProperty] private int _volume = 50;
        [ObservableProperty] private string _name = "";
        [ObservableProperty] private string _hotkey = "";
    }
}
