using MT_MusicPlayer.Models;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Queue = System.Collections.Concurrent.ConcurrentQueue<MT_MusicPlayer.Models.Music>;

namespace MT_MusicPlayer.Common
{
    /// <summary>
    /// サウンド管理クラス
    /// </summary>
    public static class SoundManager
    {
        #region プロパティ

        /// <summary>
        /// キュー
        /// </summary>
        public static Queue MusicQueue { get; set; } = new Queue();

        /// <summary>
        /// 曲名
        /// </summary>
        public static string Name => (Reader != null) ? Path.GetFileNameWithoutExtension(Reader.FileName) : "";

        /// <summary>
        /// 現在の再生時間
        /// </summary>
        public static TimeSpan CurrentTime => (Reader != null) ? Reader.CurrentTime : TimeSpan.Zero;

        /// <summary>
        /// 曲の長さ
        /// </summary>
        public static TimeSpan TotalTime => (Reader != null) ? Reader.TotalTime : TimeSpan.Zero;

        /// <summary>
        /// ボリューム
        /// </summary>
        public static float Volume
        {
            get => _Volume;
            set
            {
                _Volume = value;

                if (WaveOut != null) WaveOut.Volume = value;
            }
        }
        private static float _Volume = 0.5f;

        #endregion

        private static AudioFileReader Reader = null;
        private static WaveOutEvent WaveOut = null;

        /// <summary>
        /// キューに追加
        /// </summary>
        /// <param name="path"></param>
        public static void AddQueue(params string[] paths)
        {
            foreach (var path in paths)
            {
                using (Reader = new AudioFileReader(path))
                {
                    Music music = new Music()
                    {
                        Name = Path.GetFileNameWithoutExtension(path),
                        FilePath = path,
                        TotalTime = Reader.TotalTime
                    };
                    MusicQueue.Enqueue(music);
                }
            }
        }

        /// <summary>
        /// 現在の再生時間をセットします
        /// </summary>
        /// <param name="currentTime"></param>
        public static void SetCurrentTime(TimeSpan currentTime)
        {
            if (Reader != null) Reader.CurrentTime = currentTime;
        }

        public static void SetVolume(float volume)
        {
            if (WaveOut != null) WaveOut.Volume = volume;
        }

        /// <summary>
        /// 再生準備
        /// </summary>
        public static void Standby()
        {
            if (MusicQueue.Count < 1) return;

            MusicQueue.TryDequeue(out Music music);

            Reader = new AudioFileReader(music.FilePath);
            Reader.Volume = Volume;

            WaveOut = new WaveOutEvent();
            WaveOut.Init(Reader);
        }

        /// <summary>
        /// 再生
        /// </summary>
        public static void Play()
        {
            WaveOut?.Play();
        }

        /// <summary>
        /// 一時停止
        /// </summary>
        public static void Pause()
        {
            WaveOut?.Pause();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public static void Stop()
        {
            WaveOut?.Stop();
            if (Reader != null) Reader.Position = 0;
        }

        /// <summary>
        /// 破棄する
        /// </summary>
        public static void Destroy()
        {
            Reader?.Dispose();
            WaveOut?.Dispose();
        }
    }
}
