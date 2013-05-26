using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;

namespace ClassLibraryHelper.WPF.Behaviors
{
    /// <summary>
    /// 在取某个控件的只读属性的值时，用到此类。
    /// </summary>
    public class ReadOnyPropertyAction : TargetedTriggerAction<FrameworkElement>
    {
        public object BindablePropertyItem
        {
            get { return (object)GetValue(BindablePropertyItemProperty); }
            set { SetValue(BindablePropertyItemProperty, value); }
        }

        public static readonly DependencyProperty BindablePropertyItemProperty =
            DependencyProperty.Register("BindablePropertyItem", typeof(object), typeof(ReadOnyPropertyAction), new UIPropertyMetadata(null));

        public string Property { get; set; }

        protected override void Invoke(object parameter)
        {
            Dispatcher.BeginInvoke(new Action(() =>
                                                  {
                                                      BindablePropertyItem = AssociatedObject.GetType().GetProperty(Property).GetValue(AssociatedObject, null);
                                                  }));
        }
    }
}
