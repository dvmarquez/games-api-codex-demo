using FluentValidation;

namespace GamesApi;

internal sealed class CreateGameRequestValidator : AbstractValidator<CreateGameRequest>
{
    public CreateGameRequestValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(request => request.Genre)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(request => request.Platform)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(request => request.Price)
            .NotNull()
            .GreaterThan(0);
    }
}
