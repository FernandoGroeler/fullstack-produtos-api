using FluentValidation;
using FullStack.Produtos.Domain;

namespace FullStack.Produtos.Application;

public class ObterProdutoUseCase(IValidator<ObterProdutoRequest> validator,
    IProdutoRepository produtoRepository) : IObterProdutoUseCase
{
    private readonly IValidator<ObterProdutoRequest> _validator = validator ?? throw new ArgumentNullException(nameof(validator));    
    private readonly IProdutoRepository _produtoRepository = produtoRepository ?? throw new ArgumentNullException(nameof(produtoRepository));

    public async Task<Response<ProdutoResponse>> ExecuteAsync(ObterProdutoRequest request, CancellationToken cancellationToken = default)
    {
        var errosValidacoes = await _validator.ObterErrosValidacoesAsync(request, cancellationToken);
        if (errosValidacoes.Any())
            return Response<ProdutoResponse>.Validation(errosValidacoes);        
        
        var participante = await _produtoRepository.ObterPorIdAsync(request.Id);

        return participante == null
            ? Response<ProdutoResponse>.Notification("Produto não localizado.")
            : Response<ProdutoResponse>.Success(participante.ToProdutoResponse());
    }
}