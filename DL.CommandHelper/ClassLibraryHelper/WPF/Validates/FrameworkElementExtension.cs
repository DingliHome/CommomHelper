using System;
using System.Windows;

namespace ClassLibraryHelper.WPF.Validates
{
    public static class FrameworkElementExtension
    {
        public static void LoadOnce(this FrameworkElement element, Action action, Func<bool> condition)
        {
            element.SetValue(FrameworkElementService.UnLoadedConditionProperty, new ActionCondition(action, condition));
            element.Loaded += OnceLoaded;
        }

        private static void OnceLoaded(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            var condition = FrameworkElementService.GetUnLoadedCondition(element);
            if (condition.Condition())
            {
                condition.Action();
                element.Loaded -= OnceLoaded;
                element.ClearValue(FrameworkElementService.UnLoadedConditionProperty);
            }
        }


        public static void Once(this FrameworkElement element, Action action, Func<bool> condition)
        {
            //element.SetValue(FrameworkElementService.UnLoadedConditionProperty, new ActionCondition(action, condition));
            //element.Loaded += OnceLoaded;
        }
    }
}