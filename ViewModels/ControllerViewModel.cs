using MT_MusicPlayer.Common;
using MT_MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Prism.Mvvm;
using Prism.Commands;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using System.IO;
using Queue = System.Collections.ObjectModel.ObservableCollection<MT_MusicPlayer.Models.Music>;

namespace MT_MusicPlayer.ViewModels
{
    /// <summary>
    /// ControllerのViewModel
    /// </summary>
    class ControllerViewModel : BindableBase
    {
        #region プロパティ

        public Queue MusicList => model.MusicList;

        /// <summary>
        /// 曲名
        /// </summary>
        public string Name
        {
            get => model.Name;
            set => model.Name = value;
        }

        /// <summary>
        /// 現在の時間
        /// </summary>
        public TimeSpan CurrentTime
        {
            get => model.CurrentTime;
            set => model.CurrentTime = value;
        }

        /// <summary>
        /// 曲の時間
        /// </summary>
        public TimeSpan TotalTime
        {
            get => model.TotalTime;
            set => model.TotalTime = value;
        }

        public float Volume
        {
            get => model.Volume;
            set => model.Volume = value;
        }

        public BitmapFrame AlbumArt
        {
            get => model.AlbumArt;
        }

        #endregion

        #region コマンド

        public ICommand PlayCommand => new DelegateCommand(model.Play);
        public ICommand PauseCommand => new DelegateCommand(model.Pause);
        public ICommand StopCommand => new DelegateCommand(model.Stop);
        public ICommand SeekBarMouseDownCommand => new DelegateCommand<MouseButtonEventArgs>(model.SeekBar_MouseDown);
        public ICommand SeekBarMouseUpCommand => new DelegateCommand<MouseButtonEventArgs>(model.SeekBar_MouseUp);
        public ICommand MusicDoubleClick => new DelegateCommand<Music>(model.MusicDoubleClick);
        public ICommand DropCommand => new DelegateCommand<DragEventArgs>(model.Drop);
        public ICommand ClosingCommand => new DelegateCommand<CancelEventArgs>(model.Closing);

        #endregion

        /// <summary>
        /// モデル
        /// </summary>
        private ControllerModel model;

        // コンストラクタ
        public ControllerViewModel()
        {
            model = ControllerModel.Instance;

            model.PropertyChanged += Model_PropertyChanged;
        }

        /// <summary>
        /// モデルのプロパティ変更通知を受け取る
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
    }
}
