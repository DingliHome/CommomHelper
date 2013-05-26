using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace ClassLibraryHelper.WPF.MoveResizeRotateThumb
{
    public class ResizeThumb : Thumb
    {
        private Adorner adorner;
        private double angle;
        private Canvas canvas;
        private ContentControl designerItem;
        private RotateTransform rotateTransform;
        private Point transformOrigin;

        public ResizeThumb()
        { 
            DragStarted += (sender, args) =>
                               {
                                   designerItem = DataContext as ContentControl;

                                   if (designerItem != null)
                                   {
                                       canvas = VisualTreeHelper.GetParent(designerItem) as Canvas;

                                       if (canvas != null)
                                       {
                                           transformOrigin = designerItem.RenderTransformOrigin;

                                           rotateTransform = designerItem.RenderTransform as RotateTransform;
                                           if (rotateTransform != null)
                                           {
                                               angle = rotateTransform.Angle*Math.PI/180.0;
                                           }
                                           else
                                           {
                                               angle = 0.0d;
                                           }

                                           AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(canvas);
                                           if (adornerLayer != null)
                                           {
                                               adorner = new SizeAdorner(designerItem);
                                               adornerLayer.Add(adorner);
                                           }
                                       }
                                   }
                               };
            DragDelta += (sender, e) =>
                             {
                                 if (designerItem != null)
                                 {
                                     double deltaVertical, deltaHorizontal;

                                     switch (VerticalAlignment)
                                     {
                                         case VerticalAlignment.Bottom:
                                             deltaVertical = Math.Min(-e.VerticalChange,
                                                                      designerItem.ActualHeight - designerItem.MinHeight);
                                             Canvas.SetTop(designerItem,
                                                           Canvas.GetTop(designerItem) +
                                                           (transformOrigin.Y*deltaVertical*(1 - Math.Cos(-angle))));
                                             Canvas.SetLeft(designerItem,
                                                            Canvas.GetLeft(designerItem) -
                                                            deltaVertical*transformOrigin.Y*Math.Sin(-angle));
                                             designerItem.Height -= deltaVertical;
                                             break;
                                         case VerticalAlignment.Top:
                                             deltaVertical = Math.Min(e.VerticalChange,
                                                                      designerItem.ActualHeight - designerItem.MinHeight);
                                             Canvas.SetTop(designerItem,
                                                           Canvas.GetTop(designerItem) + deltaVertical*Math.Cos(-angle) +
                                                           (transformOrigin.Y*deltaVertical*(1 - Math.Cos(-angle))));
                                             Canvas.SetLeft(designerItem,
                                                            Canvas.GetLeft(designerItem) +
                                                            deltaVertical*Math.Sin(-angle) -
                                                            (transformOrigin.Y*deltaVertical*Math.Sin(-angle)));
                                             designerItem.Height -= deltaVertical;
                                             break;
                                         default:
                                             break;
                                     }

                                     switch (HorizontalAlignment)
                                     {
                                         case HorizontalAlignment.Left:
                                             deltaHorizontal = Math.Min(e.HorizontalChange,
                                                                        designerItem.ActualWidth - designerItem.MinWidth);
                                             Canvas.SetTop(designerItem,
                                                           Canvas.GetTop(designerItem) + deltaHorizontal*Math.Sin(angle) -
                                                           transformOrigin.X*deltaHorizontal*Math.Sin(angle));
                                             Canvas.SetLeft(designerItem,
                                                            Canvas.GetLeft(designerItem) +
                                                            deltaHorizontal*Math.Cos(angle) +
                                                            (transformOrigin.X*deltaHorizontal*(1 - Math.Cos(angle))));
                                             designerItem.Width -= deltaHorizontal;
                                             break;
                                         case HorizontalAlignment.Right:
                                             deltaHorizontal = Math.Min(-e.HorizontalChange,
                                                                        designerItem.ActualWidth - designerItem.MinWidth);
                                             Canvas.SetTop(designerItem,
                                                           Canvas.GetTop(designerItem) -
                                                           transformOrigin.X*deltaHorizontal*Math.Sin(angle));
                                             Canvas.SetLeft(designerItem,
                                                            Canvas.GetLeft(designerItem) +
                                                            (deltaHorizontal*transformOrigin.X*(1 - Math.Cos(angle))));
                                             designerItem.Width -= deltaHorizontal;
                                             break;
                                         default:
                                             break;
                                     }
                                 }

                                 e.Handled = true;
                             };
            DragCompleted += (sender, args) =>
                                 {
                                     if (adorner != null)
                                     {
                                         AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(canvas);
                                         if (adornerLayer != null)
                                         {
                                             adornerLayer.Remove(adorner);
                                         }

                                         adorner = null;
                                     }
                                 };
        }
    }
}