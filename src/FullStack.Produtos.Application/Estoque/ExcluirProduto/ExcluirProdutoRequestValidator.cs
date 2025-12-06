using FluentValidation;

namespace FullStack.Produtos.Application;

public class ExcluirProdutoRequestValidator : AbstractValidator<ExcluirProdutoRequest>
{
    public ExcluirProdutoRequestValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O ID do produto deve ser informado para excluir o produto");        
    }
}