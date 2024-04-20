using System;
using System.ComponentModel.DataAnnotations;

namespace SupermarketZlagoda.Data.Model
{
    public class MinimumAgeAttribute(int minimumAge) : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is not DateOnly dateOfBirth) return false;
            return DateOnly.FromDateTime(DateTime.Today.AddYears(-minimumAge)) >= dateOfBirth;
        }
        
    }
}