using FullStack.Produtos.Application;
using Microsoft.AspNetCore.Mvc;

namespace FullStack.Produtos.Api;

public class ListarTodosProdutos : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("listar-todos-produtos", async (
            [FromServices] IListarTodosProdutosUseCase useCase) =>
        {
            var retorno = await useCase.ExecuteAsync();
            return retorno.IsFailure ? Results.BadRequest(retorno) : Results.Ok(retorno);
        }).MapToApiVersion(1);
    }
}