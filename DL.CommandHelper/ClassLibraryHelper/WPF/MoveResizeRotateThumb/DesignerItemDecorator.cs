using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ClassLibraryHelper.WPF.MoveResizeRotateThumb
{
    public class DesignerItemDecorator : Control
    {
        public DesignerItemDecorator()
        {
            Unloaded += (sender, args) =>
                            {
                                if (this._adorner != null)
                                {
                                    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                                    if (adornerLayer != null)
                                    {
                                        adornerLayer.Remove(this._adorner);
                                    }

                                    this._adorner = null;
                                }
                            };
        }

        private Adorner _adorner;

        public bool ShowDecorator
        {
            get { return (bool)GetValue(ShowDecoratorProperty); }
            set { SetValue(ShowDecoratorProperty, value); }
        }

        public static readonly DependencyProperty ShowDecoratorProperty =
            DependencyProperty.Register("ShowDecorator", typeof(bool), typeof(DesignerItemDecorator),
            new UIPropertyMetadata(false, ((o, args) =>
                                               {
                                                   var decorator = o as DesignerItemDecorator;
                                                   var newValue = (bool)args.NewValue;
                                                   if (newValue)
                                                       decorator.ShowAdorner();
                                                   else
                                                       decorator.HideAdorner();
                                               })));

        private void ShowAdorner()
        {
            if (_adorner == null)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    var contentControl = this.DataContext as ContentControl;
                    var canvas = VisualTreeHelper.GetParent(contentControl) as Canvas;
                    _adorner = new ResizeRotateAdorner(contentControl);
                    adornerLayer.Add(_adorner);
                    this._adorner.Visibility = ShowDecorator ? Visibility.Visible : Visibility.Hidden;
                }
            }
            else
                _adorner.Visibility = Visibility.Visible;

        }
        private void HideAdorner()
        {
            if (_adorner != null)
                _adorner.Visibility = Visibility.Hidden;
        }
    }
}
