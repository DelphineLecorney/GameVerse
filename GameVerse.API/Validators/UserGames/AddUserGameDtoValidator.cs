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
                .Must(type => new[] { "Wishlist", "Library" }.Contains(type))
                .WithMessage("Le type de relation doit être 'Wishlist' ou 'Library'.");
            
            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 10)
                .When(x => x.Rating.HasValue)
                .WithMessage("La note doit être comprise entre 0 et 10.");
        }
    }
}
