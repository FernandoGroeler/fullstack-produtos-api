using FluentValidation;

namespace FullStack.Produtos.Application;

public class ObterProdutoRequestValidator : AbstractValidator<ObterProdutoRequest>
{
    public ObterProdutoRequestValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O ID do produto deve ser informado para obter o produto");
    }
}