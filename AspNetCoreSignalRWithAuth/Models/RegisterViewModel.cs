using System.ComponentModel.DataAnnotations;
namespace AspNetCoreSignalRWithAuth.Models
{
    public class RegisterViewModel
    {
        [Required,Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required, DataType(DataType.Password),MinLength(4)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password),MinLength(4),Compare(nameof(Password))
            ,Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }


    }
}
