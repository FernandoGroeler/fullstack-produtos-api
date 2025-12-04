using FullStack.Produtos.Domain;

namespace FullStack.Produtos.Application;

public interface IIncluirProdutoUseCase : IUseCase<IncluirProdutoRequest, Response<ProdutoResponse>>;