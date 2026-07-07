using FluentValidation;
using GameVerse.SHARED.DTOs.Games;

namespace GameVerse.API.Validators.Games
{
    public class CreateGameDtoValidator : AbstractValidator<CreateGameDto>
    {
        public CreateGameDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Le titre est obligatoire.")
                .MinimumLength(2).WithMessage("Le titre doit contenir au moins 2 caractères.");

            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Le genre est obligatoire.");

            RuleFor(x => x.ReleaseDate)
                .GreaterThan(new DateTime(1970, 1, 1))
                .LessThanOrEqualTo(DateTime.UtcNow.AddYears(1))
                .WithMessage("La date de sortie doit être comprise entre 1970 et l'année prochaine.");


        }

    }
}
