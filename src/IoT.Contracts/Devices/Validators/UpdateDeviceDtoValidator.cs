using FluentValidation;

namespace IoT.Contracts.Devices.Validators;

public class UpdateDeviceDtoValidator : AbstractValidator<UpdateDeviceDto>
{
    public UpdateDeviceDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.AdminStatus)
            .IsInEnum().WithMessage("Invalid admin status");
    }
}
