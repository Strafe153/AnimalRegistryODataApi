using System.Text.RegularExpressions;

namespace Application.Validators;

public static partial class CustomValidators
{
    private const string PhoneNumberPattern = @"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$";

    [GeneratedRegex(PhoneNumberPattern)]
    public static partial Regex PhoneNumberValidator();
}