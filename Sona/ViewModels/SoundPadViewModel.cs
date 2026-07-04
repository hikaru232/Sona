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

        public SoundPadViewModel()
        {
            var loadedSongs = DataService.LoadSongs();

            if (loadedSongs == null)
            {
                Songs.Add(new Song { FilePath = @"C:\Windows\Media\chimes.wav", Name = "test1", Volume = 100 });
                Songs.Add(new Song { FilePath = @"C:\Windows\Media\tada.wav", Name = "test2", Volume = 80 });
                Songs.Add(new Song { FilePath = @"C:\Users\hikar\Downloads\要らないもの\Morning.mp3", Name = "test3", Volume = 10 });
                DataService.SaveSongs(Songs);
            }
            else
            {
                foreach (var song in loadedSongs)
                {
                    Songs.Add(song);
                }
            }
        }

        //音声を再生するメソッド等を書いていく
        //ここを変更する。WASAPIを使う。
        private readonly List<WasapiOut> _player = new();
        private readonly List<AudioFileReader> _audioFile = new();

        private void StopAndDispose()
        {
            foreach (var player in _player)
            {
                player.Stop();
                player.Dispose();
            }
            _player.Clear();

            foreach (var audioFile in _audioFile)
            {
                audioFile.Dispose();
            }
            _audioFile.Clear();
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
                var selectedIds = AppSettings.Default.SelectedDeviceIds;
                var enumerator = new MMDeviceEnumerator();
                
                foreach (var deviceId in selectedIds)
                {
                    var device = enumerator.GetDevice(deviceId);
                    var audioFile = new AudioFileReader(song.FilePath);
                    audioFile.Volume = (float)(song.Volume / 100f) * (AppSettings.Default.MasterVolume / 100f);

                    var player = new WasapiOut(device, AudioClientShareMode.Shared, true, 50);
                    player.Init(audioFile);
                    player.Play();

                    _player.Add(player);
                    _audioFile.Add(audioFile);
                }
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
            DataService.SaveSongs(Songs);
        }

        [RelayCommand]
        public void RemoveSong(Song? song)
        {
            if (song == null) return;

            var result = MessageBox.Show(
                $"「{song.Name}」をリストから削除してもよろしいですか？\n※実際の音声ファイルは削除されません。", "削除の確認",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Songs.Remove(song);
                DataService.SaveSongs(Songs);
            }
        }
    }
}
