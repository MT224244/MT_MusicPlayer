using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MT_MusicPlayer
{
    /// <summary>
    /// 拡張メソッド群
    /// </summary>
    public static class Extentions
    {
        /// <summary>
        /// UIスレッド外からShowする
        /// </summary>
        /// <param name="window"></param>
        public static void ShowEx(this Window window) => window.Dispatcher.Invoke(() => window.Show());

        /// <summary>
        /// UIスレッド外からHideする
        /// </summary>
        /// <param name="window"></param>
        public static void HideEx(this Window window) => window.Dispatcher.Invoke(() => window.Hide());
    }
}
