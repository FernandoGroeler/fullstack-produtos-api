using FullStack.Produtos.Domain;

namespace FullStack.Produtos.Application;

public interface IExcluirProdutoUseCase : IUseCase<ExcluirProdutoRequest, Response<bool>>;