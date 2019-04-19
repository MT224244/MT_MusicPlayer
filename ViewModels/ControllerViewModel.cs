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

namespace MT_MusicPlayer.ViewModels
{
    /// <summary>
    /// ControllerのViewModel
    /// </summary>
    class ControllerViewModel : INotifyPropertyChanged
    {
        #region プロパティ

        /// <summary>
        /// 曲名
        /// </summary>
        public string Name
        {
            get => model.Name;
            set
            {
                model.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// 現在の時間
        /// </summary>
        public TimeSpan CurrentTime
        {
            get => model.CurrentTime;
            set
            {
                model.CurrentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        /// <summary>
        /// 曲の時間
        /// </summary>
        public TimeSpan TotalTime
        {
            get => model.TotalTime;
            set
            {
                model.TotalTime = value;
                OnPropertyChanged(nameof(TotalTime));
            }
        }

        #endregion

        #region コマンド

        public ICommand PlayCommand => new RelayCommand(model.Play);
        public ICommand PauseCommand => new RelayCommand(model.Pause);
        public ICommand StopCommand => new RelayCommand(model.Stop);
        public ICommand ShowControllerCommand => new RelayCommand(model.ShowController);
        public ICommand ExitCommand => new RelayCommand(model.Exit);

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
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e.PropertyName);

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
