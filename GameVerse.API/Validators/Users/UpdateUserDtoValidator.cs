using FluentValidation;
using GameVerse.API.DTOs.Users;

namespace GameVerse.API.Validators.Users
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Le nom d'utilisateur est obligatoire.")
                .MinimumLength(3).WithMessage("Le nom d'utilisateur doit contenir au moins 3 caractères.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("L'email est obligatoire.")
                .EmailAddress().WithMessage("L'email n'est pas valide.");

        }
    }
}
