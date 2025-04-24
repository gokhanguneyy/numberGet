using FluentValidation;
using numberGet.Models;

namespace numberGet.FluentValidation
{
    public class RegisterValidation : AbstractValidator<RegisterViewModel>
    {
        public RegisterValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name must not be empty")
                .MinimumLength(3).WithMessage("Name must be at least three character long");
            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname must not be empty")
                .MinimumLength(2).WithMessage("Surname must be at least two character long");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Please select uniqe username")
                .MinimumLength(1).MaximumLength(15).WithMessage("Length of username can be between 1 and 15 characters");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter the email address.")
                .EmailAddress().WithMessage("Please enter the valid email address");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("You must enter the password")
                .MinimumLength(8).WithMessage("You must enter the at least 6 characters long");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords are not equal");
        }
    }
}
