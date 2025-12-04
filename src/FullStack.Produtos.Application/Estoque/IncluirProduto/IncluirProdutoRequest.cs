namespace FullStack.Produtos.Application;

public record IncluirProdutoRequest(
    string Nome,
    string Descricao,
    decimal Preco,
    int EstoqueAtual
);