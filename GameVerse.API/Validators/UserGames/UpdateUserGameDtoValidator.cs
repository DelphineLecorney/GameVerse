using FluentValidation;
using GameVerse.SHARED.DTOs.UserGame;

namespace GameVerse.API.Validators.UserGames
{
    public class UpdateUserGameDtoValidator : AbstractValidator<UpdateUserGameDto>
    {
        public UpdateUserGameDtoValidator()
        {
            RuleFor(x => x.RelationType)
                .NotEmpty().WithMessage("Le type de relation est obligatoire.")
                .Must(type => new[] { "Wishlist", "Library", "Favorites" }.Contains(type))
                .WithMessage("Le type de relation doit être 'Wishlist'ou 'Library'.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 10)
                .When(x => x.Rating.HasValue)
                .WithMessage("La note doit être comprise entre 0 et 10.");
        }
    }
}