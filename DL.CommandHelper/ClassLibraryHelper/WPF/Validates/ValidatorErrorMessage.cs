namespace ClassLibraryHelper.WPF.Validates
{
    internal class ValidatorErrorMessage
    {
        public const string DefaultErrorMessage = "验证出错";

        public const string RequiredValidatorErrorMessage = "值不得為空,并不能為空格";

        public const string StringLengthValidatorErrorMessage = "字符長度必須介於{0}和{1}之間";

        public const string EmailValidatorErrorMessage = "邮件地址格式错误";

        public const string PasswordCompareValidatorErrorMessage = "密码不匹配";

        public const string EmptyCollectionValidatorErrorMessage = "列表中存在空值,請檢查";

        public const string IpAddressErrorMessage = "Ip地址格式错误";
    }
}
