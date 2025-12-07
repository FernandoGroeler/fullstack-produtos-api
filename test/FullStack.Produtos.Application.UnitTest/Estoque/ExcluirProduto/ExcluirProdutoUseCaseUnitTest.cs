using FluentValidation;
using FluentValidation.Results;
using FullStack.Produtos.Domain;
using Moq;

namespace FullStack.Produtos.Application.UnitTest;

public class ExcluirProdutoUseCaseUnitTest
{
    private readonly Mock<IValidator<ExcluirProdutoRequest>> _validatorMock = new();
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock = new();

    [Fact]
    public void Ctor_DeveLancarArgumentNullException_QuandoValidatorNulo()
    {
        // Arrange
        IValidator<ExcluirProdutoRequest>? validatorNulo = null;

        // Act + Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ExcluirProdutoUseCase(validatorNulo!, _produtoRepositoryMock.Object));
    }

    [Fact]
    public void Ctor_DeveLancarArgumentNullException_QuandoRepositoryNulo()
    {
        // Arrange
        IProdutoRepository? repoNulo = null;

        // Act + Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ExcluirProdutoUseCase(_validatorMock.Object, repoNulo!));
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarValidation_QuandoRequestInvalido()
    {
        // Arrange
        var request = new ExcluirProdutoRequest(Guid.Empty);

        var validationResult = new ValidationResult([
            new ValidationFailure("Id", "Id inválido")
        ]);

        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var useCase = new ExcluirProdutoUseCase(_validatorMock.Object, _produtoRepositoryMock.Object);

        // Act
        var response = await useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);

        Assert.True(response.IsFailure);
        Assert.False(response.IsSuccess);

        Assert.NotNull(response.Errors);

        var errorMessage = response.Errors.First().Description;

        Assert.Equal("Id inválido", errorMessage);

        // Não deve chamar o repositório se a validação falhar
        _produtoRepositoryMock.Verify(
            r => r.ExcluirAsync(It.IsAny<Guid>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarNotification_QuandoProdutoNaoForExcluido()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new ExcluirProdutoRequest(id);

        var validationResult = new ValidationResult(); // sem erros de validação
        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _produtoRepositoryMock
            .Setup(r => r.ExcluirAsync(id))
            .ReturnsAsync(false);

        var useCase = new ExcluirProdutoUseCase(_validatorMock.Object, _produtoRepositoryMock.Object);

        // Act
        var response = await useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);

        Assert.True(response.IsFailure); // ou IsNotification
        Assert.False(response.IsSuccess);

        Assert.NotNull(response.Errors);

        var notificationMessage = response.Errors.First().Description;
        Assert.Equal("Produto não foi excluído.", notificationMessage);

        _produtoRepositoryMock.Verify(
            r => r.ExcluirAsync(id),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarSuccess_QuandoProdutoForExcluidoComSucesso()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new ExcluirProdutoRequest(id);

        var validationResult = new ValidationResult(); // sem erros
        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _produtoRepositoryMock
            .Setup(r => r.ExcluirAsync(id))
            .ReturnsAsync(true);

        var useCase = new ExcluirProdutoUseCase(_validatorMock.Object, _produtoRepositoryMock.Object);

        // Act
        var response = await useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccess);
        Assert.False(response.IsFailure);

        Assert.True(response.Value);

        _produtoRepositoryMock.Verify(
            r => r.ExcluirAsync(id),
            Times.Once);
    }
}