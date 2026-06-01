using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Sona.Models;
using NAudio.Wave;
using System.Windows;
using Sona.Views;

namespace Sona.ViewModels
{
    //とりあえず継承はせず、一度ここで必要なものを全部書く。他で必要になりそうなときに初めてViewModelBaseを作って継承させる。
    public partial class MainWindowViewModel : ObservableObject
    {
        public ObservableCollection<Song> Songs { get; } = new();

        private Song _test1 = new Song { FilePath = @"C:\Windows\Media\chimes.wav", Name = "test1", Volume = 100 };
        private Song _test2 = new Song { FilePath = @"C:\Windows\Media\tada.wav", Name = "test2", Volume = 80 };
        private Song _test3 = new Song { FilePath = @"C:\Windows\Media\tadeee.wav", Name = "test3", Volume = 90 };

        public MainWindowViewModel()
        {
            Songs.Add(_test1);
            Songs.Add(_test2);
            Songs.Add(_test3);
        }

        //音声を再生するメソッド等を書いていく
        //ここを変更する。WASAPIを使う。
        private WaveOutEvent? _player;
        private AudioFileReader? _audioFile;

        private AppSetting Settings => AppSetting.Default;

        private void StopAndDispose()
        {
            _player?.Stop();
            _player?.Dispose();
            _player = null;

            _audioFile?.Dispose();
            _audioFile = null;
        }

        public void CleanUp()
        {
            StopAndDispose();
        }

        //一旦ボタンは常に押せるようにして良いものとして、CanExecuteは常にTrueにしとく。
        [RelayCommand]
        private void PlaySound(Song song)
        {
            if (!File.Exists(song.FilePath))
            {
                MessageBox.Show($"音声が見つかりません。ファイルを確認してください。{song.FilePath}", "エラー");
                return;
            }

            StopAndDispose();
            _audioFile = new AudioFileReader(song.FilePath);
            _audioFile.Volume = (float)(song.Volume / 100f) * (Settings.MasterVolume / 100f);
            _player = new WaveOutEvent();
            _player.Init(_audioFile);
            _player.Play();
        }

        //AddEditDialogをMainWindowから開く(MVVMとしてはベストプラクティスではない)
        [RelayCommand]
        public void OpenAddEditDialog(Song? song)
        {
            var vm = new AddEditWindowViewModel(song);
            var dialog = new AddEditWindow();
            vm.CloseRequested += (result) => dialog.DialogResult = result;

            dialog.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            dialog.DataContext = vm;
            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                if (song == null)
                {
                    Songs.Add(new Song
                    {
                        Name = vm.Name,
                        FilePath = vm.FilePath,
                        Volume = vm.Volume,
                        Hotkey = vm.Hotkey
                    });
                }
                else
                {
                    song.Name = vm.Name;
                    song.FilePath = vm.FilePath;
                    song.Volume = vm.Volume;
                    song.Hotkey = vm.Hotkey;
                }
            }

        }

    }
}
