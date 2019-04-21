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
        private bool IsTrackMouseDown = false;

        /// <summary>
        /// マウス左ボタンプレス時処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            IsTrackMouseDown = true;

            Value = CalcValue(e.GetPosition(this));

            OnTrackMouseDown(e);

            CaptureMouse();
        }

        /// <summary>
        /// マウス移動時処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (!IsTrackMouseDown) return;

            double val = CalcValue(e.GetPosition(this));

            Value = CalcValue(e.GetPosition(this));
        }

        /// <summary>
        /// マウス左ボタンリリース時処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            IsTrackMouseDown = false;

            OnTrackMouseUp(e);

            ReleaseMouseCapture();
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            if (newValue < 0) Value = 0;

            base.OnValueChanged(oldValue, newValue);
        }

        /// <summary>
        /// Thumbの座標計算
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private double CalcValue(Point pos)
        {
            Thumb thumb = Template.FindName("Thumb", this) as Thumb;

            double val = 0;

            if (Orientation == Orientation.Horizontal)
            {
                double x = pos.X;

                if (x < 0) x = 0;
                else if (x > ActualWidth) x = ActualWidth;

                val = ((x - (thumb.ActualWidth / 2)) / (ActualWidth - thumb.ActualWidth)) * Maximum;
            }
            else
            {
                double y = pos.Y;

                if (y < 0) y = 0;
                else if (y > ActualHeight) y = ActualHeight;

                val = -((y - (thumb.ActualHeight / 2)) / (ActualHeight - thumb.ActualHeight)) + Maximum;
            }

            if (val < 0) val = 0;
            else if (val > Maximum) val = Maximum;

            return val;
        }

        public new event MouseButtonEventHandler MouseDown;
        private void OnTrackMouseDown(MouseButtonEventArgs e) => MouseDown?.Invoke(this, e);

        public new event MouseButtonEventHandler MouseUp;
        private void OnTrackMouseUp(MouseButtonEventArgs e) => MouseUp?.Invoke(this, e);
    }
}