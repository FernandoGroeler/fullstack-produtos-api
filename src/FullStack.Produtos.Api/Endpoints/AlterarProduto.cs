using FullStack.Produtos.Application;
using Microsoft.AspNetCore.Mvc;

namespace FullStack.Produtos.Api;

public class AlterarProduto : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPut("produto", async (
            [FromBody] AlterarProdutoRequest request,
            [FromServices] IAlterarProdutoUseCase useCase) =>
        {
            var retorno = await useCase.ExecuteAsync(request);
            return retorno.IsFailure ? Results.BadRequest(retorno) : Results.Ok(retorno);
        }).MapToApiVersion(1);
    }
}