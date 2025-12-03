using FullStack.Produtos.Domain;
using FullStack.Produtos.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FullStack.Produtos.IoC;

internal static class DataDependencyInjection
{
    internal static void RegisterDbContext(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContextInMemory>(opt => opt.UseInMemoryDatabase("Produto"));
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContextInMemory>());
    }

    internal static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
    }
}