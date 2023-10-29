using FluentValidation;
using PLPlayersAPI.Models;

namespace PLPlayersAPI.Validators
{
    public class NationalityValidator : AbstractValidator<Nationality>
    {
        public NationalityValidator()
        {
            RuleFor(n=>n.Country)
              .NotEmpty()
              .MinimumLength(3)
              .WithMessage("Minimum length of the country is 3 characters");
        }
    }
}
