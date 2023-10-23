using FluentValidation;
using PLPlayersAPI.Models.DTOs;

namespace PLPlayersAPI.Validators
{
    public class UserValidator : AbstractValidator<UserDTO>
    {
        public UserValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty()
                .MinimumLength(4)
                .WithMessage("The minimum length of the username is 4 characters");
            RuleFor(u => u.Password)
                .NotEmpty()
                .MinimumLength(5)
                .WithMessage("The minimum length of the password is 5 characters")
                .Must(ContainCapitalLetter)
                .WithMessage("Password must contain at least one capital letter");
        }

        private bool ContainCapitalLetter(string password)
        {
            return password.Any(char.IsUpper);
        }
    }
}
