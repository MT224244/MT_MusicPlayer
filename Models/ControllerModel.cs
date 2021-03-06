﻿using MT_MusicPlayer.Common;
using NAudio.Dsp;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Queue = System.Collections.ObjectModel.ObservableCollection<MT_MusicPlayer.Models.Music>;

namespace MT_MusicPlayer.Models
{
    /// <summary>
    /// ControllerのModel
    /// </summary>
    public class ControllerModel : BindableBase
    {
        #region プロパティ

        public Queue MusicList => SoundManager.MusicQueue;

        /// <summary>
        /// 曲名
        /// </summary>
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }
        private string _Name;

        /// <summary>
        /// 現在の時間
        /// </summary>
        public TimeSpan CurrentTime
        {
            get => _CurrentTime;
            set => SetProperty(ref _CurrentTime, value);
        }
        private TimeSpan _CurrentTime = TimeSpan.Zero;

        /// <summary>
        /// 曲の時間
        /// </summary>
        public TimeSpan TotalTime
        {
            get => _TotalTime;
            set => SetProperty(ref _TotalTime, value);
        }
        private TimeSpan _TotalTime = TimeSpan.Zero;

        /// <summary>
        /// ボリューム
        /// </summary>
        public float Volume
        {
            get => _Volume;
            set
            {
                SetProperty(ref _Volume, value);
                SoundManager.SetVolume(value);
            }
        }
        private float _Volume = SoundManager.Volume;

        public BitmapFrame AlbumArt
        {
            get => _AlbumArt;
            set => SetProperty(ref _AlbumArt, value);
        }
        private BitmapFrame _AlbumArt = null;

        #endregion

        public static ControllerModel Instance = Instance ?? new ControllerModel();

        private bool IsSeekbarMouseDown = false;

        // コンストラクタ
        private ControllerModel()
        {
            DispatcherTimer timer = new DispatcherTimer(
                interval: TimeSpan.FromMilliseconds(10),
                priority: DispatcherPriority.Send,
                callback: Timer_Tick,
                dispatcher: Dispatcher.CurrentDispatcher);

            timer.Start();
        }

        public void Play() => SoundManager.Play();

        public void Pause() => SoundManager.Pause();

        public void Stop() => SoundManager.Stop();

        public void SeekBar_MouseDown(MouseButtonEventArgs e)
        {
            IsSeekbarMouseDown = true;
        }

        public void SeekBar_MouseUp(MouseButtonEventArgs e)
        {
            IsSeekbarMouseDown = false;
            SoundManager.SetCurrentTime(CurrentTime);
        }

        public void MusicDoubleClick(Music music)
        {
            SoundManager.Destroy();
            SoundManager.SelectMusic(music);
            Name = SoundManager.Name;
            TotalTime = SoundManager.TotalTime;
            AlbumArt = SoundManager.AlbumArt;
            SoundManager.Play();
        }

        public void Drop(DragEventArgs e)
        {
            SoundManager.Destroy();
            SoundManager.AddQueue(e.Data.GetData(DataFormats.FileDrop) as string[]);
            SoundManager.Standby();
            Name = SoundManager.Name;
            TotalTime = SoundManager.TotalTime;
            AlbumArt = SoundManager.AlbumArt;
        }

        public void Closing(CancelEventArgs e)
        {
            MessagingCenter.Send(this, "HideController");
            e.Cancel = true;
        }

        /// <summary>
        /// タイマー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!IsSeekbarMouseDown) CurrentTime = SoundManager.CurrentTime;
        }
    }
}
