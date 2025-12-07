using FluentValidation;
using FluentValidation.Results;
using FullStack.Produtos.Domain;
using Moq;

namespace FullStack.Produtos.Application.UnitTest;

public class AlterarProdutoUseCaseUnitTest
{
    private readonly Mock<IValidator<AlterarProdutoRequest>> _validatorMock = new();
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock = new();

    [Fact]
    public void Ctor_DeveLancarArgumentNullException_QuandoValidatorNulo()
    {
        // Arrange
        IValidator<AlterarProdutoRequest>? validatorNulo = null;

        // Act + Assert
        Assert.Throws<ArgumentNullException>(() =>
            new AlterarProdutoUseCase(validatorNulo!, _produtoRepositoryMock.Object));
    }

    [Fact]
    public void Ctor_DeveLancarArgumentNullException_QuandoRepositoryNulo()
    {
        // Arrange
        IProdutoRepository? repoNulo = null;

        // Act + Assert
        Assert.Throws<ArgumentNullException>(() =>
            new AlterarProdutoUseCase(_validatorMock.Object, repoNulo!));
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarValidation_QuandoRequestInvalido()
    {
        // Arrange
        var request = new AlterarProdutoRequest(Guid.Empty, "", "Desc", 0, -1);

        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Id", "Id inválido")
        });

        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var useCase = new AlterarProdutoUseCase(_validatorMock.Object, _produtoRepositoryMock.Object);

        // Act
        var response = await useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsFailure); // ajuste conforme sua Response<T>
        Assert.False(response.IsSuccess);

        Assert.NotNull(response.Errors);
        Assert.Single(response.Errors);

        var errorMessage = response.Errors.First().Description;
        Assert.Equal("Id inválido", errorMessage);

        _produtoRepositoryMock.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>()), Times.Never);
        _produtoRepositoryMock.Verify(r => r.AlterarAsync(It.IsAny<Produto>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarNotification_QuandoProdutoNaoEncontrado()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new AlterarProdutoRequest(id, "Novo Nome", "Nova desc", 10m, 5);

        var validationResult = new ValidationResult(); // sem erros
        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _produtoRepositoryMock
            .Setup(r => r.ObterPorIdAsync(id))
            .ReturnsAsync((Produto)null!);

        var useCase = new AlterarProdutoUseCase(_validatorMock.Object, _produtoRepositoryMock.Object);

        // Act
        var response = await useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsFailure);
        Assert.False(response.IsSuccess);

        Assert.NotNull(response.Errors);

        var notificationMessage = response.Errors.First().Description;
        Assert.Equal("Produto não localizado para a alteração.", notificationMessage);

        _produtoRepositoryMock.Verify(r => r.ObterPorIdAsync(id), Times.Once);
        _produtoRepositoryMock.Verify(r => r.AlterarAsync(It.IsAny<Produto>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarNotification_QuandoAlterarAsyncRetornarNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new AlterarProdutoRequest(id, "Nome Alterado", "Descricao Alterada", 200m, 20);

        var validationResult = new ValidationResult();
        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var produtoExistente = new Produto
        {
            Id = id,
            Nome = "Nome Original",
            Descricao = "Descricao Original",
            Preco = 100m,
            EstoqueAtual = 10
        };

        _produtoRepositoryMock
            .Setup(r => r.ObterPorIdAsync(id))
            .ReturnsAsync(produtoExistente);

        _produtoRepositoryMock
            .Setup(r => r.AlterarAsync(It.IsAny<Produto>()))
            .ReturnsAsync((Produto)null!);

        var useCase = new AlterarProdutoUseCase(_validatorMock.Object, _produtoRepositoryMock.Object);

        // Act
        var response = await useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsFailure);
        Assert.False(response.IsSuccess);

        Assert.NotNull(response.Errors);

        var notificationMessage = response.Errors.First().Description;
        Assert.Equal("Produto não alterado.", notificationMessage);

        _produtoRepositoryMock.Verify(r => r.ObterPorIdAsync(id), Times.Once);

        _produtoRepositoryMock.Verify(r =>
                r.AlterarAsync(It.Is<Produto>(p =>
                    p.Id == id &&
                    p.Nome == request.Nome &&
                    p.Descricao == request.Descricao &&
                    p.Preco == request.Preco &&
                    p.EstoqueAtual == request.EstoqueAtual
                )),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarSuccess_QuandoProdutoAlteradoComSucesso()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new AlterarProdutoRequest(id, "Nome Alterado", "Descricao Alterada", 200m,20);

        var validationResult = new ValidationResult();
        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var produtoExistente = new Produto
        {
            Id = id,
            Nome = "Nome Original",
            Descricao = "Descricao Original",
            Preco = 100m,
            EstoqueAtual = 10
        };

        _produtoRepositoryMock
            .Setup(r => r.ObterPorIdAsync(id))
            .ReturnsAsync(produtoExistente);

        var produtoAlterado = new Produto
        {
            Id = id,
            Nome = request.Nome,
            Descricao = request.Descricao,
            Preco = request.Preco,
            EstoqueAtual = request.EstoqueAtual
        };

        _produtoRepositoryMock
            .Setup(r => r.AlterarAsync(It.IsAny<Produto>()))
            .ReturnsAsync(produtoAlterado);

        var useCase = new AlterarProdutoUseCase(_validatorMock.Object, _produtoRepositoryMock.Object);

        // Act
        var response = await useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccess);
        Assert.False(response.IsFailure);

        Assert.NotNull(response.Value);

        _produtoRepositoryMock.Verify(r => r.ObterPorIdAsync(id), Times.Once);

        _produtoRepositoryMock.Verify(r =>
                r.AlterarAsync(It.Is<Produto>(p =>
                    p.Id == id &&
                    p.Nome == request.Nome &&
                    p.Descricao == request.Descricao &&
                    p.Preco == request.Preco &&
                    p.EstoqueAtual == request.EstoqueAtual
                )),
            Times.Once);
    }
}