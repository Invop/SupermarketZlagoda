using System.ComponentModel.DataAnnotations;

namespace SupermarketZlagoda.Data
{
    public class MinimumAgeAttribute(int minimumAge) : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is not DateTime dateOfBirth) return false;
            return DateTime.Today.AddYears(-minimumAge) >= dateOfBirth;
        }
    }
}