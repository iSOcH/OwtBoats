using backend.Contracts;
using FluentValidation;

namespace backend.Validators;

public class BoatDataValidator : AbstractValidator<BoatData>
{
    public BoatDataValidator()
    {
        RuleFor(boat => boat.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(256);
        
        RuleFor(boat => boat.Description)
            .MinimumLength(5)
            .MaximumLength(16384);
    }
}