using FluentValidation;

namespace IoT.Contracts.Devices.Validators;

public class CreateDeviceDtoValidator : AbstractValidator<CreateDeviceDto>
{
    public CreateDeviceDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid device type");
    }
}
