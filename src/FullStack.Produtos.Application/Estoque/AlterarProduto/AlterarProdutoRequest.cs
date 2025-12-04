namespace FullStack.Produtos.Application;

public record AlterarProdutoRequest(
    Guid Id,
    string Nome,
    string Descricao,
    decimal Preco,
    int EstoqueAtual    
);