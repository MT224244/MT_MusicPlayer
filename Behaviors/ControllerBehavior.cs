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

            MessagingCenter.Subscribe<TaskbarIconModel>(this, "ShowController", model => AssociatedObject.Show());
            MessagingCenter.Subscribe<ControllerModel>(this, "HideController", model => AssociatedObject.Hide());
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            MessagingCenter.Unsubscribe<TaskbarIconModel>(this, "ShowController");
            MessagingCenter.Unsubscribe<ControllerModel>(this, "HideController");
        }
    }
}
