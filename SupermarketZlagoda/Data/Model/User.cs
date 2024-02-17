using System.ComponentModel.DataAnnotations;

namespace SupermarketZlagoda.Data.Model;

public class User
{
    public int user_id{ get; set; }
    [Required]
    [MinLength(3, ErrorMessage = "Name is too short!")]
    [StringLength(16, ErrorMessage = "Name too long (16 character limit).")]
    public string login { get; set; }
    [Required]
    [MinLength(8, ErrorMessage = "Password is too short!")]
    [StringLength(16, ErrorMessage = "Password too long (16 character limit).")]
    public string hash_password { get; set; }
    [Required]
    public UserRoleType role_type { get; set; }
}