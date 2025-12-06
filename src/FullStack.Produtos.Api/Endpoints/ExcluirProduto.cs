using FullStack.Produtos.Application;
using Microsoft.AspNetCore.Mvc;

namespace FullStack.Produtos.Api;

public class ExcluirProduto : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapDelete("produto", async (
            [FromQuery] Guid id,
            [FromServices] IExcluirProdutoUseCase useCase) =>
        {
            var retorno = await useCase.ExecuteAsync(new ExcluirProdutoRequest(id));
            return retorno.IsFailure ? Results.BadRequest(retorno) : Results.Ok(retorno);
        }).MapToApiVersion(1);
    }
}