using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ClassLibraryHelper.WPF.MoveResizeRotateThumb
{
    public class MoveThumb : Thumb
    {
        private RotateTransform _rotateTransform;
        private ContentControl _contentControl;
        public MoveThumb()
        {

            DragStarted += (sender, args) =>
                               {
                                    _contentControl = DataContext as ContentControl;
                                   if (_contentControl != null)
                                   {
                                       _rotateTransform = _contentControl.RenderTransform as RotateTransform;
                                   }
                               };
            DragDelta += (sender, args) =>
                             {
                                 if (_contentControl != null)
                                 {
                                     var point = new Point(args.HorizontalChange, args.VerticalChange);
                                     if (_rotateTransform != null)
                                     {
                                         point = _rotateTransform.Transform(point);
                                     }

                                     Canvas.SetTop(_contentControl, Canvas.GetTop(_contentControl) + point.Y);
                                     Canvas.SetLeft(_contentControl, Canvas.GetLeft(_contentControl) + point.X);
                                 }
                             };
        }
    }
}
