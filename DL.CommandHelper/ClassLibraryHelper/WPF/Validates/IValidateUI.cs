using System.Windows;

namespace ClassLibraryHelper.WPF.Validates
{
    public interface IValidateUI
    {
        FrameworkElement Element { get; set; }

        void FailShow();

        void SuccessShow();
    }
}
