using FluentValidation;
using FluentValidation.Results;
using FullStack.Produtos.Domain;
using Moq;

namespace FullStack.Produtos.Application.UnitTest;

public class ObterProdutoUseCaseUnitTest
{
    private readonly Mock<IValidator<ObterProdutoRequest>> _validatorMock = new();
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock = new();

    [Fact]
    public async Task ExecuteAsync_DeveRetornarValidation_QuandoRequestInvalido()
    {
        // Arrange
        var request = new ObterProdutoRequest(Guid.Empty);

        var validationResult = new ValidationResult([
            new ValidationFailure("Id", "Id inválido")
        ]);

        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var useCase = new ObterProdutoUseCase(_validatorMock.Object, _produtoRepositoryMock.Object);

        // Act
        var response = await useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsFailure);
        Assert.False(response.IsSuccess);

        Assert.NotNull(response.Errors);
        Assert.Single(response.Errors);
        Assert.Equal("Id inválido", response.Errors.First().Description);

        _produtoRepositoryMock.Verify(
            r => r.ObterPorIdAsync(It.IsAny<Guid>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarNotification_QuandoProdutoNaoEncontrado()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var request = new ObterProdutoRequest(requestId);

        var validationResult = new ValidationResult();
        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _produtoRepositoryMock
            .Setup(r => r.ObterPorIdAsync(requestId))
            .ReturnsAsync((Produto)null!);

        var useCase = new ObterProdutoUseCase(_validatorMock.Object, _produtoRepositoryMock.Object);

        // Act
        var response = await useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsFailure);
        Assert.False(response.IsSuccess);

        Assert.NotNull(response.Errors.First().Description);

        var notificationMessage = response.Errors.First().Description;

        Assert.Equal("Produto não localizado.", notificationMessage);

        _produtoRepositoryMock.Verify(
            r => r.ObterPorIdAsync(requestId),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarSuccess_QuandoProdutoEncontrado()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var request = new ObterProdutoRequest(requestId);

        var validationResult = new ValidationResult();
        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Crie um produto de domínio real conforme sua implementação
        var produto = new Produto
            {
                Id = requestId,
                Nome = "Produto Teste",
                Descricao = "Descrição do produto Teste",
                Preco = 10.5m,
                EstoqueAtual = 5
            };

        _produtoRepositoryMock
            .Setup(r => r.ObterPorIdAsync(requestId))
            .ReturnsAsync(produto);

        var useCase = new ObterProdutoUseCase(_validatorMock.Object, _produtoRepositoryMock.Object);

        // Act
        var response = await useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccess);
        Assert.False(response.IsFailure);

        Assert.NotNull(response.Value);

        _produtoRepositoryMock.Verify(
            r => r.ObterPorIdAsync(requestId),
            Times.Once);
    }
}