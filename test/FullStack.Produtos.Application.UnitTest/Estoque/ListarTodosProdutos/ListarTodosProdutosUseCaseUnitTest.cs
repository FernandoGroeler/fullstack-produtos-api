using FullStack.Produtos.Domain;
using Moq;

namespace FullStack.Produtos.Application.UnitTest;

public class ListarTodosProdutosUseCaseUnitTest
{
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock = new();

    [Fact]
    public void Ctor_DeveLancarArgumentNullException_QuandoRepositorioNulo()
    {
        // Arrange
        IProdutoRepository? repoNulo = null;

        // Act + Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ListarTodosProdutosUseCase(repoNulo!));
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarNotification_QuandoNaoExistemProdutos()
    {
        // Arrange
        _produtoRepositoryMock
            .Setup(r => r.ListarTodosAsync())
            .ReturnsAsync(new List<Produto>());

        var useCase = new ListarTodosProdutosUseCase(_produtoRepositoryMock.Object);

        // Act
        var response = await useCase.ExecuteAsync();

        // Assert
        Assert.NotNull(response);

        Assert.True(response.IsFailure);
        Assert.False(response.IsSuccess);

        Assert.NotNull(response.Errors);

        var notificationMessage = response.Errors.First().Description;

        Assert.Equal("Não existem produtos cadastrados.", notificationMessage);

        _produtoRepositoryMock.Verify(r => r.ListarTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarSuccess_QuandoExistemProdutos()
    {
        // Arrange
        var produto1 = new Produto{
            Id = Guid.NewGuid(),
            Nome = "Produto 1",
            Descricao = "Produto 1",
            Preco = 20.0m,
            EstoqueAtual = 7
        };

        var produto2 = new Produto
        {
            Id =  Guid.NewGuid(),
            Nome = "Produto 2",
            Descricao = "Produto 2",
            Preco = 10.0m,
            EstoqueAtual = 5
        };

        var produtos = new List<Produto> { produto1, produto2 };

        _produtoRepositoryMock
            .Setup(r => r.ListarTodosAsync())
            .ReturnsAsync(produtos);

        var useCase = new ListarTodosProdutosUseCase(_produtoRepositoryMock.Object);

        // Act
        var response = await useCase.ExecuteAsync();

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccess);
        Assert.False(response.IsFailure);

        Assert.NotNull(response.Value);

        var listaResponse = response.Value.ToList();
        Assert.Equal(2, listaResponse.Count);

        _produtoRepositoryMock.Verify(r => r.ListarTodosAsync(), Times.Once);
    }
}