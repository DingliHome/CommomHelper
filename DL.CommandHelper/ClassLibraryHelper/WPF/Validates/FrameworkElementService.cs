using System.Windows;

namespace ClassLibraryHelper.WPF.Validates
{
    public class FrameworkElementService
    {

        #region TagProperty

        public static object GetTag(DependencyObject obj)
        {
            return (object)obj.GetValue(TagProperty);
        }

        public static void SetTag(DependencyObject obj, object value)
        {
            obj.SetValue(TagProperty, value);
        }

        /// <summary>
        /// Privide another Tag replace with the Tag of FrameworkElement
        /// </summary>
        public static readonly DependencyProperty TagProperty =
            DependencyProperty.RegisterAttached("Tag", typeof(object), typeof(FrameworkElementService),
                                                new UIPropertyMetadata(null));

        #endregion

        public static ActionCondition GetUnLoadedCondition(DependencyObject obj)
        {
            return (ActionCondition)obj.GetValue(UnLoadedConditionProperty);
        }

        public static void SetUnLoadedCondition(DependencyObject obj, ActionCondition value)
        {
            obj.SetValue(UnLoadedConditionProperty, value);
        }

        public static readonly DependencyProperty UnLoadedConditionProperty =
            DependencyProperty.RegisterAttached("UnLoadedCondition", typeof(ActionCondition), typeof(FrameworkElementService),
                                                new UIPropertyMetadata(null));


        /// <summary>
        /// Gets the value of the IsFirstVisit attached property for the specified DependencyObject
        ///When you first change The propery's value,then set IsFirstVisit as mark
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetIsFirstVisit(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFirstVisitProperty);
        }

        public static void SetIsFirstVisit(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFirstVisitProperty, value);
        }

        public static readonly DependencyProperty IsFirstVisitProperty =
            DependencyProperty.RegisterAttached("IsFirstVisit", typeof(bool), typeof(FrameworkElementService),
                                                new UIPropertyMetadata(true));


        public static bool GetMarkEdit(DependencyObject obj)
        {
            return (bool)obj.GetValue(MarkEditProperty);
        }

        public static void SetMarkEdit(DependencyObject obj, bool value)
        {
            obj.SetValue(MarkEditProperty, value);
        }

        public static readonly DependencyProperty MarkEditProperty =
            DependencyProperty.RegisterAttached("MarkEdit", typeof(bool), typeof(FrameworkElementService),
                                                new PropertyMetadata(true));


    }
}