using System.ComponentModel.DataAnnotations;

namespace numberGet.Models.AuthenticationModels
{
    public class SignInModels
    {
        public int CustomerId { get; set; } = 0;
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        

    }
}
