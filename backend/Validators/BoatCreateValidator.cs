using backend.Contracts;
using FluentValidation;

namespace backend.Validators;

public class BoatCreateValidator : AbstractValidator<BoatCreateRequest>
{
    public BoatCreateValidator(IValidator<BoatInfo> boatInfoValidator)
    {
        RuleFor(boat => boat.Id).NotEmpty();
        RuleFor(boat => boat.Boat).SetValidator(boatInfoValidator);
    }
}