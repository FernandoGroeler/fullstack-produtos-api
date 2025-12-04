using FluentValidation;
using FullStack.Produtos.Domain;

namespace FullStack.Produtos.Application;

public class IncluirProdutoUseCase(IValidator<IncluirProdutoRequest> validator,
    IProdutoRepository repository) : IIncluirProdutoUseCase
{
    private readonly IValidator<IncluirProdutoRequest> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    private readonly IProdutoRepository _produtoRepository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<Response<ProdutoResponse>> ExecuteAsync(IncluirProdutoRequest request, CancellationToken cancellationToken = default)
    {
        var errosValidacoes = await _validator.ObterErrosValidacoesAsync(request, cancellationToken);
        if (errosValidacoes.Any())
            return Response<ProdutoResponse>.Validation(errosValidacoes);        
        
        var produto = await _produtoRepository.InserirAsync(new Produto
        {
            Id = Guid.NewGuid(),
            Nome =  request.Nome,
            Descricao =  request.Descricao,
            Preco =  request.Preco,
            EstoqueAtual = request.EstoqueAtual
        });
        
        return produto == null
            ? Response<ProdutoResponse>.Notification("Produto não inserido.")
            : Response<ProdutoResponse>.Success("Produto inserido com sucesso", produto.ToProdutoResponse());
    }
}