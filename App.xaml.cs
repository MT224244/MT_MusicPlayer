using MT_MusicPlayer.Common;
using MT_MusicPlayer.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MT_MusicPlayer
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : System.Windows.Application
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            App app = new App();
            app.InitializeComponent();

            SystemManager.Controller = new Controller();

            CreateNotifyIcon();

            Task.Run(async () => await SystemManager.Start());

            app.Run();
        }

        /// <summary>
        /// NotifyIcon作成
        /// </summary>
        private static void CreateNotifyIcon()
        {
            ContextMenu menu = new ContextMenu();
            menu.MenuItems.Add("表示", (s, e) =>
            {
                SystemManager.Controller.Show();
            });
            menu.MenuItems.Add("終了", (s, e) =>
            {
                SystemManager.Exit();
            });

            SystemManager.NotifyIcon = new NotifyIcon();
            SystemManager.NotifyIcon.Text = "MT MusicPlayer";
            SystemManager.NotifyIcon.Icon = System.Drawing.SystemIcons.Exclamation;
            SystemManager.NotifyIcon.ContextMenu = menu;
            SystemManager.NotifyIcon.Visible = true;
        }
    }
}