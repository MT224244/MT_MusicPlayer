using MT_MusicPlayer.Models;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TagLib;
using Queue = System.Collections.ObjectModel.ObservableCollection<MT_MusicPlayer.Models.Music>;

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

        public static BitmapFrame AlbumArt => _AlbumArt;
        private static BitmapFrame _AlbumArt = null;

        #endregion

        private static AudioFileReader Reader = null;
        private static WaveOutEvent WaveOut = null;
        private static Tag tag = null;

        /// <summary>
        /// キューに追加
        /// </summary>
        /// <param name="path"></param>
        public static void AddQueue(params string[] paths)
        {
            foreach (var path in paths)
            {
                if (!CheckPlayableFile(path))
                {
                    System.Windows.MessageBox.Show("再生できないファイルです");
                    continue;
                }

                using (AudioFileReader reader = new AudioFileReader(path))
                {
                    Music music = new Music()
                    {
                        Name = Path.GetFileNameWithoutExtension(path),
                        FilePath = path,
                        TotalTime = reader.TotalTime
                    };
                    MusicQueue.Add(music);
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

            Music music = MusicQueue[MusicQueue.Count() - 1];
            //MusicQueue.RemoveAt(MusicQueue.Count() - 1);

            Tag tag = TagLib.File.Create(music.FilePath).Tag;
            _AlbumArt = (tag.Pictures.Count() > 0) ? AlbumArtConvert(tag.Pictures[0]) : null;

            //Console.WriteLine(
            //    $"Album: {tag.Album}\n" +
            //    $"AlbumArtists: {string.Join(", ", tag.AlbumArtists)}\n" +
            //    $"AlbumArtistsSort: {string.Join(", ", tag.AlbumArtistsSort)}\n" +
            //    $"AlbumSort: {tag.AlbumSort}\n" +
            //    $"AmazonId: {tag.AmazonId}\n" +
            //    $"BeatsPerMinutes: {tag.BeatsPerMinute}\n" +
            //    $"Comment: {tag.Comment}\n" +
            //    $"Composers: {string.Join(", ", tag.Composers)}\n" +
            //    $"ComposersSort: {string.Join(", ", tag.ComposersSort)}\n" +
            //    $"Conductor: {tag.Conductor}\n" +
            //    $"Copyright: {tag.Copyright}\n" +
            //    $"Disc: {tag.Disc}\n" +
            //    $"DiscCount: {tag.DiscCount}\n" +
            //    $"FirstAlbumArtist: {tag.FirstAlbumArtist}\n" +
            //    $"FirstAlbumArtistSort: {tag.FirstAlbumArtistSort}\n" +
            //    $"FirstComposer: {tag.FirstComposer}\n" +
            //    $"FirstComposerSort: {tag.FirstComposerSort}\n" +
            //    $"FirstGenre: {tag.FirstGenre}\n" +
            //    $"FirstPerformer: {tag.FirstPerformer}\n" +
            //    $"FirstPerformerSort: {tag.FirstPerformerSort}\n" +
            //    $"Genres: {string.Join(", ", tag.Genres)}\n" +
            //    $"Grouping: {tag.Grouping}\n" +
            //    $"IsEmpty: {tag.IsEmpty}\n" +
            //    $"JoinedAlbumArtists: {tag.JoinedAlbumArtists}\n" +
            //    $"JoinedComposers: {tag.JoinedComposers}\n" +
            //    $"JoinedGenres: {tag.JoinedGenres}\n" +
            //    $"JoinedPerformers: {tag.JoinedPerformers}\n" +
            //    $"JoinedPerformersSort: {tag.JoinedPerformersSort}\n" +
            //    $"Lyrics: {tag.Lyrics}\n" +
            //    $"MusicBrainzArtistId: {tag.MusicBrainzArtistId}\n" +
            //    $"MusicBrainzDiscId: {tag.MusicBrainzDiscId}\n" +
            //    $"MusicBrainzReleaseArtistId: {tag.MusicBrainzReleaseArtistId}\n" +
            //    $"MusicBrainzReleaseCountry: {tag.MusicBrainzReleaseCountry}\n" +
            //    $"MusicBrainzReleaseId: {tag.MusicBrainzReleaseId}\n" +
            //    $"MusicBrainzReleaseStatus: {tag.MusicBrainzReleaseStatus}\n" +
            //    $"MusicBrainzReleaseType: {tag.MusicBrainzReleaseType}\n" +
            //    $"MusicBrainzTrackId: {tag.MusicBrainzTrackId}\n" +
            //    $"MusicIpId: {tag.MusicIpId}\n" +
            //    $"Performers: {string.Join(", ", tag.Performers)}\n" +
            //    $"PerformersSort: {string.Join(", ", tag.PerformersSort)}\n" +
            //    $"TagTypes: {tag.TagTypes}\n" +
            //    $"Title: {tag.Title}\n" +
            //    $"TitleSort: {tag.TitleSort}\n" +
            //    $"Track: {tag.Track}\n" +
            //    $"TrackCount: {tag.TrackCount}\n" +
            //    $"Year: {tag.Year}");

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
            Reader = null;
            WaveOut = null;
            tag = null;
        }

        /// <summary>
        /// 再生可能ファイルかをチェック
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static bool CheckPlayableFile(string filePath)
        {
            bool isPlayable = false;

            try
            {
                using (AudioFileReader reader = new AudioFileReader(filePath))
                using (WaveOutEvent waveOut = new WaveOutEvent())
                {
                    waveOut.Init(reader);
                }

                isPlayable = true;
            }
            catch (Exception)
            {
                isPlayable = false;
            }

            return isPlayable;
        }

        private static BitmapFrame AlbumArtConvert(IPicture pic)
        {
            BitmapFrame bf = null;
            using (MemoryStream ms = new MemoryStream(pic.Data.Data))
            {
                bf = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
            return bf;
        }
    }
}
