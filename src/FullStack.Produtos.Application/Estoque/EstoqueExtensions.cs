using FullStack.Produtos.Domain;

namespace FullStack.Produtos.Application;

public static class EstoqueExtensions
{
    public static ProdutoResponse ToProdutoResponse(this Produto produto)
    {
        return new ProdutoResponse(produto.Id, produto.Nome, produto.Descricao, produto.Preco, produto.EstoqueAtual);
    }    
}