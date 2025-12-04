using FullStack.Produtos.Application.Estoque;
using FullStack.Produtos.Domain;

namespace FullStack.Produtos.Application;

public class ObterProdutoUseCase : IObterProdutoUseCase
{
    public async Task<Response<ProdutoResponse>> ExecuteAsync(ObterProdutoRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}