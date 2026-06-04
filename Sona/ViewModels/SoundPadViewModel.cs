using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using Sona.Models;
using Sona.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace Sona.ViewModels
{
    public partial class SoundPadViewModel : ObservableObject
    {
        public ObservableCollection<Song> Songs { get; } = new();

        private Song _test1 = new Song { FilePath = @"C:\Windows\Media\chimes.wav", Name = "test1", Volume = 100 };
        private Song _test2 = new Song { FilePath = @"C:\Windows\Media\tada.wav", Name = "test2", Volume = 80 };
        private Song _test3 = new Song { FilePath = @"C:\Windows\Media\tadeee.wav", Name = "test3", Volume = 90 };

        public SoundPadViewModel()
        {
            Songs.Add(_test1);
            Songs.Add(_test2);
            Songs.Add(_test3);
        }

        //音声を再生するメソッド等を書いていく
        //ここを変更する。WASAPIを使う。
        private WasapiOut? _player;
        private AudioFileReader? _audioFile;

        private void StopAndDispose()
        {
            if (_player != null)
            {
                _player?.Stop();
                _player?.Dispose();
                _player = null;
            }

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

            //ここのtry-catchは本当に必要か
            try
            {
                _audioFile = new AudioFileReader(song.FilePath);
                _audioFile.Volume = (float)(song.Volume / 100f) * (AppSettings.Default.MasterVolume / 100f);

                //ここは既定のデバイスになっているが、Settingsから取得するように変更する
                var enumerator = new MMDeviceEnumerator();
                //変更する。もし再生に支障があれば、ここに問題がある。
                MMDevice defaultDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);

                _player = new WasapiOut(defaultDevice, AudioClientShareMode.Shared, true, 50);
                _player.Init(_audioFile);
                _player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"音声の再生に失敗しました。\n{ex.Message}", "エラー");
                StopAndDispose();
            }
        }

        [RelayCommand]
        public void ShowAddEditDialog(Song? song)
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
