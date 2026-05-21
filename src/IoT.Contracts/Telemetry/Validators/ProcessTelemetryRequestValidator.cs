using FluentValidation;

namespace IoT.Contracts.Telemetry.Validators;

public class ProcessTelemetryRequestValidator : AbstractValidator<ProcessTelemetryRequest>
{
    public ProcessTelemetryRequestValidator()
    {
        RuleFor(x => x.MessageId)
            .NotEmpty().WithMessage("MessageId is required");

        RuleFor(x => x.Payload)
            .NotEmpty().WithMessage("Payload is required")
            .MaximumLength(10000).WithMessage("Payload must not exceed 10000 characters");
    }
}