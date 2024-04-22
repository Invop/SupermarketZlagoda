using System.ComponentModel.DataAnnotations;

namespace SupermarketZlagoda.Data.Model;

public class PasswordModel
{
    private string _newPassword;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Password should be between 6 and 20 characters.")]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Password should consist of alphanumeric characters only.")]
    public string NewPassword
    {
        get => _newPassword;
        set
        {
            if (_newPassword != value)
            {
                _newPassword = value;
                NewPasswordChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public event EventHandler? NewPasswordChanged;
}