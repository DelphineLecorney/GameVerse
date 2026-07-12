using GameVerse.API.Validators.Games;
using GameVerse.SHARED.DTOs.Games;

namespace GameVerse.TESTS.Validators
{
    public class UpdateGameDtoValidatorTests
    {
        private readonly UpdateGameDtoValidator _validator = new();

        [Fact]
        public void Title_Empty_ShouldHaveValidationError()
        {
            var dto = new UpdateGameDto
            {
                Title = "",
                Genre = "RPG",
                ReleaseDate = DateTime.UtcNow
            };

            var result = _validator.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Title");
        }

        [Fact]
        public void ReleaseDate_Before1970_ShouldHaveValidationError()
        {
            var dto = new UpdateGameDto
            {
                Title = "Test",
                Genre = "RPG",
                ReleaseDate = new DateTime(1969, 1, 1)
            };

            var result = _validator.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "ReleaseDate");
        }

        [Fact]
        public void ValidDto_ShouldPassValidation()
        {
            var dto = new UpdateGameDto
            {
                Title = "Minecraft",
                Genre = "Sandbox",
                ReleaseDate = new DateTime(2011, 11, 18)
            };

            var result = _validator.Validate(dto);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Title_NullOrWhitespace_ShouldHaveValidationError(string? invalidTitle)
        {
            var dto = new UpdateGameDto
            {
                Title = invalidTitle!,
                Genre = "RPG",
                ReleaseDate = DateTime.UtcNow
            };

            var result = _validator.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Title");
        }
    }
}
