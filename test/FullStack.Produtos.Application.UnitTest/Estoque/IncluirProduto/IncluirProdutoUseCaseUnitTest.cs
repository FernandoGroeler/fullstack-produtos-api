using FluentValidation;
using FluentValidation.Results;
using FullStack.Produtos.Domain;
using Moq;

namespace FullStack.Produtos.Application.UnitTest;

public class IncluirProdutoUseCaseUnitTest
{
    private readonly Mock<IValidator<IncluirProdutoRequest>> _validatorMock = new();
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock = new();

    [Fact]
    public void Ctor_DeveLancarArgumentNullException_QuandoValidatorNulo()
    {
        // Arrange
        IValidator<IncluirProdutoRequest>? validatorNulo = null;

        // Act + Assert
        Assert.Throws<ArgumentNullException>(() =>
            new IncluirProdutoUseCase(validatorNulo!, _produtoRepositoryMock.Object));
    }

    [Fact]
    public void Ctor_DeveLancarArgumentNullException_QuandoRepositoryNulo()
    {
        // Arrange
        IProdutoRepository? repoNulo = null;

        // Act + Assert
        Assert.Throws<ArgumentNullException>(() =>
            new IncluirProdutoUseCase(_validatorMock.Object, repoNulo!));
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarValidation_QuandoRequestInvalido()
    {
        // Arrange
        var request = new IncluirProdutoRequest("", "desc",0, -1);

        var validationResult = new ValidationResult([
            new ValidationFailure("Nome", "Nome é obrigatório")
        ]);

        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var useCase = new IncluirProdutoUseCase(_validatorMock.Object, _produtoRepositoryMock.Object);

        // Act
        var response = await useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);

        Assert.True(response.IsFailure);
        Assert.False(response.IsSuccess);

        Assert.NotNull(response.Errors);
        Assert.Single(response.Errors);

        var errorMessage = response.Errors.First().Description;

        Assert.Equal("Nome é obrigatório", errorMessage);

        // Não deve chamar o repositório se houver erro de validação
        _produtoRepositoryMock.Verify(
            r => r.InserirAsync(It.IsAny<Produto>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarNotification_QuandoRepositorioRetornaNull()
    {
        // Arrange
        var request = new IncluirProdutoRequest("Produto Teste", "Descricao teste",
            100m, 10);

        var validationResult = new ValidationResult(); // sem erros
        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _produtoRepositoryMock
            .Setup(r => r.InserirAsync(It.IsAny<Produto>()))
            .ReturnsAsync((Produto)null!);

        var useCase = new IncluirProdutoUseCase(_validatorMock.Object, _produtoRepositoryMock.Object);

        // Act
        var response = await useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsFailure);
        Assert.False(response.IsSuccess);

        Assert.NotNull(response.Errors);

        var notificationMessage = response.Errors.First().Description;
        Assert.Equal("Produto não inserido.", notificationMessage);

        _produtoRepositoryMock.Verify(
            r => r.InserirAsync(It.IsAny<Produto>()),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarSuccess_QuandoProdutoInseridoComSucesso()
    {
        // Arrange
        var request = new IncluirProdutoRequest("Produto Teste", "Descricao teste",
            100m, 10);

        var validationResult = new ValidationResult(); // sem erros
        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Simula o produto que será retornado pelo repositório
        var produtoInserido = new Produto
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome,
            Descricao = request.Descricao,
            Preco = request.Preco,
            EstoqueAtual = request.EstoqueAtual
        };

        _produtoRepositoryMock
            .Setup(r => r.InserirAsync(It.IsAny<Produto>()))
            .ReturnsAsync(produtoInserido);

        var useCase = new IncluirProdutoUseCase(_validatorMock.Object, _produtoRepositoryMock.Object);

        // Act
        var response = await useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccess);
        Assert.False(response.IsFailure);

        Assert.NotNull(response.Value);

        _produtoRepositoryMock.Verify(r =>
                r.InserirAsync(It.Is<Produto>(p =>
                        p.Nome == request.Nome &&
                        p.Descricao == request.Descricao &&
                        p.Preco == request.Preco &&
                        p.EstoqueAtual == request.EstoqueAtual &&
                        p.Id != Guid.Empty // Id é gerado no use case
                )),
            Times.Once);
    }
}