using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Sona.ViewModels
{
    //とりあえず継承はせず、一度ここで必要なものを全部書く。他で必要になりそうなときに初めてViewModelBaseを作って継承させる。
    partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty] private ObservableObject? _currentPage;

        private readonly SoundPadViewModel _soundPadPage = new();
        private readonly SettingsViewModel _settingsPage = new();

        public void CleanUp()
        {
            _soundPadPage.CleanUp();
        }

        public MainWindowViewModel()
        {
            _currentPage = _soundPadPage;
        }

        [RelayCommand]
        private void ShowSoundPad()
        {
            CurrentPage = _soundPadPage;
        }

        [RelayCommand]
        private void ShowSettings()
        {
            CurrentPage = _settingsPage;
        }

        [RelayCommand]
        private void ShowAddDialog()
        {
            _soundPadPage.ShowAddEditDialog(null);
        }

        //[RelayCommand]
        //private void ShowEditDialog()
        //{
        //    _soundPadPage.ShowAddEditDialog();
        //}


    }
}
