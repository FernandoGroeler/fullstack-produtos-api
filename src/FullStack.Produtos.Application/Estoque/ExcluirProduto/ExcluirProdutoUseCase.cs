using FluentValidation;
using FullStack.Produtos.Domain;

namespace FullStack.Produtos.Application;

public class ExcluirProdutoUseCase(IValidator<ExcluirProdutoRequest> validator,
    IProdutoRepository produtoRepository) : IExcluirProdutoUseCase
{
    private readonly IValidator<ExcluirProdutoRequest> _validator = validator ?? throw new ArgumentNullException(nameof(validator));    
    private readonly IProdutoRepository _produtoRepository = produtoRepository ?? throw new ArgumentNullException(nameof(produtoRepository));    
    
    public async Task<Response<bool>> ExecuteAsync(ExcluirProdutoRequest request, CancellationToken cancellationToken = default)
    {
        var errosValidacoes = await _validator.ObterErrosValidacoesAsync(request, cancellationToken);
        if (errosValidacoes.Any())
            return Response<bool>.Validation(errosValidacoes);

        var produtoExcluido = await _produtoRepository.ExcluirAsync(request.Id);
        
        return !produtoExcluido
            ? Response<bool>.Notification("Produto não foi excluído.")
            : Response<bool>.Success("Produto excluído com sucesso.", produtoExcluido);
    }
}