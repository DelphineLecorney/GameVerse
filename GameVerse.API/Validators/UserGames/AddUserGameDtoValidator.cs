using FluentValidation;
using GameVerse.SHARED.DTOs.UserGame;

namespace GameVerse.API.Validators.UserGames
{
    public class AddUserGameDtoValidator : AbstractValidator<AddUserGameDto>
    {
        public AddUserGameDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");

            RuleFor(x => x.GameId)
                .GreaterThan(0).WithMessage("Le jeu est invalide.");

            RuleFor(x => x.RelationType)
                .NotEmpty().WithMessage("Le type de relation est obligatoire.")
                .Must(type => type == "Wishlist" || type == "Owned" || type == "Finished")
                .WithMessage("Le type de relation doit être 'Wishlist', 'Owned' ou 'Finished'.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 10)
                .When(x => x.Rating.HasValue)
                .WithMessage("La note doit être comprise entre 0 et 10.");
        }
    }
}
