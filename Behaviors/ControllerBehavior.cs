using MT_MusicPlayer.Common;
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
            SoundManager.AddQueue(e.Data.GetData(DataFormats.FileDrop) as string[]);
            SoundManager.Play();
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
