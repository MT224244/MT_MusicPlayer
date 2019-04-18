﻿using MT_MusicPlayer.Models;
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
        public static TimeSpan CurrentTime => (Reader != null) ? Reader.CurrentTime : TimeSpan.MinValue;

        /// <summary>
        /// 曲の長さ
        /// </summary>
        public static TimeSpan TotalTime => (Reader != null) ? Reader.TotalTime : TimeSpan.MinValue;

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
        /// 再生
        /// </summary>
        public static void Play()
        {
            if (MusicQueue.Count < 1) return;

            MusicQueue.TryDequeue(out Music music);

            Reader = new AudioFileReader(music.FilePath);

            WaveOut = new WaveOutEvent();
            WaveOut.Init(Reader);
            WaveOut.Play();
        }

        /// <summary>
        /// 一時停止
        /// </summary>
        public static void Pause()
        {
            if (WaveOut == null) return;

            WaveOut.Pause();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public static void Stop()
        {
            if (WaveOut == null) return;

            WaveOut.Stop();
        }

        /// <summary>
        /// 終了
        /// </summary>
        public static void Exit()
        {
            if (Reader != null) Reader.Dispose();
            if (WaveOut != null) WaveOut.Dispose();
        }
    }
}
