using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;

namespace MT_MusicPlayer.Common
{
    public class SeekBar : Slider
    {
        private Border track;
        private bool IsTrackMouseDown = false;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            ApplyTemplate();

            track = Template.FindName("border", this) as Border;
            if (track != null)
            {
                track.PreviewMouseLeftButtonDown += Track_MouseLeftButtonDown;
                track.PreviewMouseMove += Track_MouseMove;
                track.PreviewMouseLeftButtonUp += Track_MouseLeftButtonUp;
            }
        }

        #region Border イベントハンドラ

        /// <summary>
        /// マウス左ボタンプレス時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Track_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsTrackMouseDown = true;

            Value = CalcValue(e.GetPosition(this));

            OnTrackMouseDown(e);

            track.CaptureMouse();
        }

        /// <summary>
        /// マウス移動時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Track_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsTrackMouseDown) return;

            Value = CalcValue(e.GetPosition(this));

            OnTrackMouseMove(e);
        }

        /// <summary>
        /// マウス左ボタンリリース時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Track_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsTrackMouseDown = false;

            OnTrackMouseUp(e);

            track.ReleaseMouseCapture();
        }

        #endregion

        /// <summary>
        /// Thumbの座標計算
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private double CalcValue(Point pos)
        {
            if (Orientation == Orientation.Horizontal)
            {
                double x = pos.X;

                if (x < 0) x = 0;
                else if (x > ActualWidth) x = ActualWidth;

                return (x / ActualWidth) * Maximum;
            }
            else
            {
                double y = pos.Y;

                if (y < 0) y = 0;
                else if (y > ActualHeight) y = ActualHeight;

                return (y / ActualHeight) * -1 + Maximum;
            }
        }

        public new event MouseButtonEventHandler MouseDown;
        private void OnTrackMouseDown(MouseButtonEventArgs e) => MouseDown?.Invoke(this, e);

        public new event MouseButtonEventHandler MouseUp;
        private void OnTrackMouseUp(MouseButtonEventArgs e) => MouseUp?.Invoke(this, e);

        public new event MouseEventHandler MouseMove;
        private void OnTrackMouseMove(MouseEventArgs e) => MouseMove?.Invoke(this, e);
    }
}