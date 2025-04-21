using backend.Contracts;
using FluentValidation;

namespace backend.Validators;

public class BoatInfoValidator : AbstractValidator<BoatInfo>
{
    public BoatInfoValidator()
    {
        RuleFor(boat => boat.Name).NotEmpty().MinimumLength(3);
        RuleFor(boat => boat.Description).MinimumLength(5);
    }
}