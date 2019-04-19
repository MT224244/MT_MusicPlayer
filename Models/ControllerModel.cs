using MT_MusicPlayer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MT_MusicPlayer.Models
{
    /// <summary>
    /// ControllerのModel
    /// </summary>
    public class ControllerModel : INotifyPropertyChanged
    {
        #region プロパティ

        /// <summary>
        /// 曲名
        /// </summary>
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _Name;

        /// <summary>
        /// 現在の時間
        /// </summary>
        public TimeSpan CurrentTime
        {
            get => _CurrentTime;
            set
            {
                _CurrentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }
        private TimeSpan _CurrentTime = TimeSpan.Zero;

        /// <summary>
        /// 曲の時間
        /// </summary>
        public TimeSpan TotalTime
        {
            get => _TotalTime;
            set
            {
                _TotalTime = value;
                OnPropertyChanged(nameof(TotalTime));
            }
        }
        private TimeSpan _TotalTime = TimeSpan.Zero;

        #endregion

        public static ControllerModel Instance = Instance ?? new ControllerModel();

        // コンストラクタ
        private ControllerModel()
        {
            DispatcherTimer timer = new DispatcherTimer(
                interval:   TimeSpan.FromMilliseconds(10),
                priority:   DispatcherPriority.Send,
                callback:   Timer_Tick,
                dispatcher: Dispatcher.CurrentDispatcher);

            timer.Start();
        }

        public void Play() => SoundManager.Play();

        public void Pause() => SoundManager.Pause();

        public void Stop() => SoundManager.Stop();

        public void ShowController() => MessagingCenter.Send(this, "ShowController");

        public void Exit() => AppMain.Exit();

        /// <summary>
        /// タイマー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            Name = SoundManager.Name;
            CurrentTime = SoundManager.CurrentTime;
            TotalTime = SoundManager.TotalTime;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
