using MT_MusicPlayer.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MT_MusicPlayer.ViewModels
{
    public class TaskbarIconViewModel : BindableBase
    {
        public ICommand ShowControllerCommand => new DelegateCommand(model.ShowController);
        public ICommand ExitCommand => new DelegateCommand(model.Exit);

        private TaskbarIconModel model;

        public TaskbarIconViewModel()
        {
            model = TaskbarIconModel.Instance;
        }

        /// <summary>
        /// モデルのプロパティ変更通知を受け取る
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
    }
}
