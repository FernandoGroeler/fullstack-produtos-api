using FullStack.Produtos.Domain;

namespace FullStack.Produtos.Application;

public interface IAlterarProdutoUseCase : IUseCase<AlterarProdutoRequest, Response<ProdutoResponse>>;