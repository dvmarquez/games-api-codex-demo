using FluentValidation.TestHelper;
using GamesApi;
using Xunit;

namespace GamesApi.Tests;

public sealed class UpdateGameRequestValidatorTests
{
    private readonly UpdateGameRequestValidator validator = new();

    [Fact]
    public void Validate_HasErrors_WhenRequiredFieldsAreMissing()
    {
        var request = new UpdateGameRequest(null, null, null);

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.ShouldHaveValidationErrorFor(x => x.Genre);
        result.ShouldHaveValidationErrorFor(x => x.Platform);
    }

    [Fact]
    public void Validate_DoesNotHaveErrors_WhenRequestIsValid()
    {
        var request = new UpdateGameRequest("Portal 2", "Puzzle", "PC");

        var result = validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
