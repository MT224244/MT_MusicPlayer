using MT_MusicPlayer.Views;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace MT_MusicPlayer.Common
{
    /// <summary>
    /// システム管理クラス
    /// </summary>
    public static class SystemManager
    {
        #region プロパティ

        /// <summary>
        /// コントローラー
        /// </summary>
        public static Controller Controller { get; set; }

        /// <summary>
        /// タスクトレイアイコン
        /// </summary>
        public static NotifyIcon NotifyIcon { get; set; }

        #endregion

        /// <summary>
        /// キャンセルトークン
        /// </summary>
        private static CancellationTokenSource CTS;

        /// <summary>
        /// 開始
        /// </summary>
        /// <returns></returns>
        public static async Task Start()
        {
            Controller.ShowEx();

            CTS = new CancellationTokenSource();

            // アプリ終了まで待機
            await Task.Delay(-1, CTS.Token);
        }

        /// <summary>
        /// 終了
        /// </summary>
        public static void Exit()
        {
            SoundManager.Exit();
            App.Current.Shutdown();
            CTS.Cancel();
        }
    }
}
