using FullStack.Produtos.Domain;

namespace FullStack.Produtos.Application;

public class ObterProdutoUseCase(IProdutoRepository produtoRepository) : IObterProdutoUseCase
{
    private readonly IProdutoRepository _produtoRepository = produtoRepository ?? throw new ArgumentNullException(nameof(produtoRepository));

    public async Task<Response<ProdutoResponse>> ExecuteAsync(ObterProdutoRequest request, CancellationToken cancellationToken = default)
    {
        var participante = await _produtoRepository.ObterPorIdAsync(request.Id);

        return participante == null
            ? Response<ProdutoResponse>.Notification("Produto não localizado.")
            : Response<ProdutoResponse>.Success(participante.ToProdutoResponse());
    }
}