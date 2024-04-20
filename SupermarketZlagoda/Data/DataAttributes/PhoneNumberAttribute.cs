using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SupermarketZlagoda.Data;

public class PhoneNumberAttribute : ValidationAttribute
{
    
    public override bool IsValid(object value)
    {
        return value is string phoneNumber && Regex.IsMatch(phoneNumber, @"^\+?\d+$");
    }
}