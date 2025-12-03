namespace FullStack.Produtos.Api;

public class ObterProduto : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        /*
        routeBuilder.MapGet("produto", async (
            [FromQuery] Guid id,
            [FromServices] IObterContaUseCase useCase) =>
        {
            var retorno = await useCase.ExecuteAsync(new ObterContaRequest(id));
            return retorno.IsFailure ? Results.BadRequest(retorno) : Results.Ok(retorno);
        }).MapToApiVersion(1);
        */
    }
}