using FullStack.Produtos.Domain;
using Microsoft.EntityFrameworkCore;

namespace FullStack.Produtos.Infra.Data;

public interface IAppDbContext
{
    DbSet<Produto> Produtos { get; }
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);    
}