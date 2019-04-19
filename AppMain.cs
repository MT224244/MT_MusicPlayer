using MT_MusicPlayer.Common;
using MT_MusicPlayer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT_MusicPlayer
{
    public class AppMain
    {
        /// <summary>
        /// コントローラー
        /// </summary>
        private static Controller Controller;

        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            App app = new App();

            Controller = new Controller();
            Controller.Show();

            app.Run();
        }

        /// <summary>
        /// アプリケーション終了
        /// </summary>
        public static void Exit()
        {
            SoundManager.Destroy();
            App.Current.Shutdown();
        }
    }
}
