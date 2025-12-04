using FullStack.Produtos.Application;
using FullStack.Produtos.Domain;
using FullStack.Produtos.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FullStack.Produtos.IoC;

public static class ProdutosRegistry
{
    public static void RegisterDependencies(this IServiceCollection services)
    {
        RegisterDbContext(services);
        RegisterRepositories(services);
        RegisterUseCases(services);
    }
    
    private static void RegisterDbContext(IServiceCollection services)
    {
        services.AddDbContext<AppDbContextInMemory>(opt => opt.UseInMemoryDatabase("ProdutosDb"));
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContextInMemory>());
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
    }

    private static void RegisterUseCases(IServiceCollection services)
    {
        services.AddScoped<IListarTodosProdutosUseCase, ListarTodosProdutosUseCase>();
        services.AddScoped<IObterProdutoUseCase, ObterProdutoUseCase>();
    }
}