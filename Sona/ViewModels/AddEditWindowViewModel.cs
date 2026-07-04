using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sona.Models;

namespace Sona.ViewModels
{
    public partial class AddEditWindowViewModel : ObservableObject
    {
        public event Action<bool>? CloseRequested;

        public bool IsEditMode { get; }

        [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private string _name = "";
        [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private string _filePath = "";
        [ObservableProperty] private int _volume = 80;
        [ObservableProperty] private string _hotkey = "";

        public string EditOrAdd => IsEditMode ? "編集" : "追加";

        public AddEditWindowViewModel(Song? song)
        {
            if (song != null)
            {
                IsEditMode = true;
                _name = song.Name;
                _filePath = song.FilePath;
                _volume = song.Volume;
                _hotkey = song.Hotkey;
            }
            else
            {
                IsEditMode = false;
            }
        }

        //参照ボタン
        [RelayCommand]
        private void ShowDialog()
        {
            var dlg = new OpenFileDialog();
            dlg.FileName = "song";
            dlg.Filter = "Song Files|*.wav;*.mp3;";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                FilePath = dlg.FileName;
            }
        }

        //SaveとCancelのロジック
        [RelayCommand(CanExecute = nameof(CanSave))]
        private void Save()
        {
            CloseRequested?.Invoke(true);
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(FilePath);
        }

        [RelayCommand]
        private void Cancel()
        {
            CloseRequested?.Invoke(false);
        }
    }
}
