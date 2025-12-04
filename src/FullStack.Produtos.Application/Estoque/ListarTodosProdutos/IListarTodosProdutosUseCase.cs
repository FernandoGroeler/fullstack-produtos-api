using FullStack.Produtos.Application.Estoque;
using FullStack.Produtos.Domain;

namespace FullStack.Produtos.Application;

public interface IListarTodosProdutosUseCase : IUseCase<Response<IEnumerable<ProdutoResponse>>>;