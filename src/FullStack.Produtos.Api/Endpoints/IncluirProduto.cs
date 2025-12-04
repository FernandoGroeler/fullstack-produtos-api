using FullStack.Produtos.Application;
using Microsoft.AspNetCore.Mvc;

namespace FullStack.Produtos.Api;

public class IncluirProduto : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("produto", async (
            [FromBody] IncluirProdutoRequest request,
            [FromServices] IIncluirProdutoUseCase useCase) =>
        {
            var retorno = await useCase.ExecuteAsync(request);
            return retorno.IsFailure ? Results.BadRequest(retorno) : Results.Ok(retorno);
        }).MapToApiVersion(1);
    }
}