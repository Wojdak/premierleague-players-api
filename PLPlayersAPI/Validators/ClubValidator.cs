using FluentValidation;
using PLPlayersAPI.Models;

namespace PLPlayersAPI.Validators
{
    public class ClubValidator : AbstractValidator<Club>
    {
        public ClubValidator()
        {
            RuleFor(c=>c.Name)
              .NotEmpty()
              .MinimumLength(3)
              .WithMessage("The minimum length of the club is 3 characters");
        }
    }
}
