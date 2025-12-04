using FullStack.Produtos.Domain;

namespace FullStack.Produtos.Application;

public interface IObterProdutoUseCase : IUseCase<ObterProdutoRequest, Response<ProdutoResponse>>;