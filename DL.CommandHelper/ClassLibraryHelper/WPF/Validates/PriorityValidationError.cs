using System;

namespace ClassLibraryHelper.WPF.Validates
{
    public class PriorityValidationError
    {

        public int Priority { get; set; }

        public Guid ValidatorKey { get; set; }

        public string ErrorContent { get; set; }
    }
}
