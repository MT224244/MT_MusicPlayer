using MT_MusicPlayer.Common;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT_MusicPlayer.Models
{
    public class TaskbarIconModel : BindableBase
    {
        public static TaskbarIconModel Instance = Instance ?? new TaskbarIconModel();
        private TaskbarIconModel() { }

        public void ShowController() => MessagingCenter.Send(this, "ShowController");

        public void Exit() => AppMain.Exit();
    }
}
