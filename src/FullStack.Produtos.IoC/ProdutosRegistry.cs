using Microsoft.Extensions.DependencyInjection;

namespace FullStack.Produtos.IoC;

public static class ProdutosRegistry
{
    public static void RegisterDependencies(this IServiceCollection services)
    {
        services.RegisterDbContext();
        services.RegisterRepositories();
    } 
}