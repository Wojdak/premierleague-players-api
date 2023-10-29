using FluentValidation;
using PLPlayersAPI.Models;

namespace PLPlayersAPI.Validators
{
    public class PositionValidator : AbstractValidator<Position>
    {
        public PositionValidator()
        {
            RuleFor(p => p.Name)
              .NotEmpty()
              .MinimumLength(5)
              .WithMessage("The minimum length of the position is 5 characters");
        }
    }
}
