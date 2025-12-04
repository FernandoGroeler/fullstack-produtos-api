using FluentValidation;

namespace FullStack.Produtos.Application;

public class AlterarProdutoRequestValidator : AbstractValidator<AlterarProdutoRequest>
{
    public AlterarProdutoRequestValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O ID do produto deve ser informado");
        
        RuleFor(c => c.Nome)
            .NotNull()
            .NotEmpty()
            .WithMessage("O Nome do produto deve ser informado");
        
        RuleFor(c => c.Preco)
            .GreaterThanOrEqualTo(decimal.Zero)
            .WithMessage("O Preco deve ser maior ou igual a zero");
        
        RuleFor(c => c.EstoqueAtual)
            .GreaterThanOrEqualTo(0)
            .WithMessage("O Estoque deve ser maior ou igual a zero");
    }
}