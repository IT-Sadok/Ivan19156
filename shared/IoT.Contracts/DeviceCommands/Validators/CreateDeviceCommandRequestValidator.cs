using FluentValidation;

namespace IoT.Contracts.DeviceCommands.Validators;

public class CreateDeviceCommandRequestValidator : AbstractValidator<CreateDeviceCommandRequest>
{
    public CreateDeviceCommandRequestValidator()
    {
        RuleFor(x => x.CommandTypeSlug)
            .IsInEnum().WithMessage("Invalid command type");

        RuleFor(x => x.Priority)
            .InclusiveBetween(0, 2).WithMessage("Priority must be between 0 and 2");

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(DateTime.UtcNow).WithMessage("ExpiresAt must be in the future")
            .When(x => x.ExpiresAt.HasValue);
    }
}
