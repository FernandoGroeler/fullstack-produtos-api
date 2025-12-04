namespace FullStack.Produtos.Application.Estoque;

public record ProdutoResponse(
    Guid Id,
    string Nome,
    string? Descricao,
    decimal Preco,
    int EstoqueAtual);