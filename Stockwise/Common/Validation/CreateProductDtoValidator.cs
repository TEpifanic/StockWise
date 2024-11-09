using FluentValidation;
using StockWise.Common.DTOs;

namespace StockWise.Common.Validation
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name ne doit pas être vide")
                .MaximumLength(100)
                .WithMessage("Le nom du produit est requis et ne doit pas dépasser 100 caractères.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(x => x.Description != null)
                .WithMessage("La description ne doit pas dépasser 500 caractères.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Le prix doit être supérieur à 0.");

            RuleFor(x => x.ImageUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.ImageUrl))
                .WithMessage("L'URL de l'image n'est pas valide.");
        }
    }
}
