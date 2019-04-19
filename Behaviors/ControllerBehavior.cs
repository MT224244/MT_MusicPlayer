using MT_MusicPlayer.Common;
using MT_MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace MT_MusicPlayer.Behaviors
{
    /// <summary>
    /// ControllerのBehavior
    /// </summary>
    public class ControllerBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            MessagingCenter.Subscribe<ControllerModel>(this, "ShowController", model =>
            {
                AssociatedObject.Show();
            });

            AssociatedObject.Drop += AssociatedObject_Drop;
            AssociatedObject.Closing += AssociatedObject_Closing;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Drop -= AssociatedObject_Drop;
            AssociatedObject.Closing -= AssociatedObject_Closing;
        }

        /// <summary>
        /// D&D時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            SoundManager.Destroy();
            SoundManager.AddQueue(e.Data.GetData(DataFormats.FileDrop) as string[]);
            SoundManager.Standby();
        }

        /// <summary>
        /// ウィンドウ終了前処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObject_Closing(object sender, CancelEventArgs e)
        {
            (sender as Window).Hide();
            e.Cancel = true;
        }
    }
}
