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

        partial void OnIsSelectedChanged(bool value)
        {
            if (value)
            {
                if (!AppSettings.Default.SelectedDeviceIds.Contains(Id))
                {
                    AppSettings.Default.SelectedDeviceIds.Add(Id);
                }
            }
            else
                AppSettings.Default.SelectedDeviceIds.Remove(Id);
        }
        //今後増やす予定
        //Volumeのみだが
    }
}
