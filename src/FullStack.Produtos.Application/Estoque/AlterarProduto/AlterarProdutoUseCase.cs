using FluentValidation;
using FullStack.Produtos.Domain;

namespace FullStack.Produtos.Application;

public class AlterarProdutoUseCase(IValidator<AlterarProdutoRequest> validator,
    IProdutoRepository produtoRepository) : IAlterarProdutoUseCase
{
    private readonly IValidator<AlterarProdutoRequest> _validator = validator ?? throw new ArgumentNullException(nameof(validator));    
    private readonly IProdutoRepository _produtoRepository = produtoRepository ?? throw new ArgumentNullException(nameof(produtoRepository));

    public async Task<Response<ProdutoResponse>> ExecuteAsync(AlterarProdutoRequest request, CancellationToken cancellationToken = default)
    {
        var errosValidacoes = await _validator.ObterErrosValidacoesAsync(request, cancellationToken);
        if (errosValidacoes.Any())
            return Response<ProdutoResponse>.Validation(errosValidacoes);        
        
        var produtoAlterar = await _produtoRepository.ObterPorIdAsync(request.Id);
        if (produtoAlterar == null)
            return Response<ProdutoResponse>.Notification("Produto não localizado para a alteração.");

        produtoAlterar.Nome = request.Nome;
        produtoAlterar.Descricao = request.Descricao;
        produtoAlterar.Preco = request.Preco;
        produtoAlterar.EstoqueAtual = request.EstoqueAtual;

        var participante = await _produtoRepository.AlterarAsync(produtoAlterar);

        return participante == null
            ? Response<ProdutoResponse>.Notification("Produto não alterado.")
            : Response<ProdutoResponse>.Success("Produto alterado com sucesso.", participante.ToProdutoResponse());
    }
}