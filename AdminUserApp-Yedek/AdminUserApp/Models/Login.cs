using System.ComponentModel.DataAnnotations;

namespace AdminUserApp.Models
{
    public class Login
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter User name")]
        [Display(Name = "Username")]
        public string username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }
    }
}