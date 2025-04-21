using backend.Contracts;
using FluentValidation;

namespace backend.Validators;

public class BoatCreateValidator : AbstractValidator<BoatCreateRequest>
{
    public BoatCreateValidator(IValidator<BoatData> boatInfoValidator)
    {
        RuleFor(boat => boat.Id).NotEmpty();
        RuleFor(boat => boat.Data).NotNull().SetValidator(boatInfoValidator);
    }
}