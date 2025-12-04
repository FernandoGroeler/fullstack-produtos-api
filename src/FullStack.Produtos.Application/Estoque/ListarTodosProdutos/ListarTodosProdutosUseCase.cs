using FullStack.Produtos.Application.Estoque;
using FullStack.Produtos.Domain;

namespace FullStack.Produtos.Application;

public class ListarTodosProdutosUseCase(IProdutoRepository produtoRepository) : IListarTodosProdutosUseCase
{
    private readonly IProdutoRepository _produtoRepository = produtoRepository ?? throw new ArgumentNullException(nameof(produtoRepository));

    public async Task<Response<IEnumerable<ProdutoResponse>>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var produtos = await _produtoRepository.ListarTodosAsync();

        var produtosResponse = produtos
            .Select(produto => new ProdutoResponse(produto.Id, produto.Nome, produto.Descricao, produto.Preco,
                produto.EstoqueAtual))
            .ToList();

        return !produtos.Any()
            ? Response<IEnumerable<ProdutoResponse>>.Notification("Não existem produtos cadastrados.")
            : Response<IEnumerable<ProdutoResponse>>.Success(produtosResponse);
    }
}