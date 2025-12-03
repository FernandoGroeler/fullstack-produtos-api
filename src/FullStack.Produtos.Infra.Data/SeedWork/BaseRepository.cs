using FullStack.Produtos.Domain;
using Microsoft.EntityFrameworkCore;

namespace FullStack.Produtos.Infra.Data;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly IAppDbContext AppDbContext;
    protected readonly DbSet<TEntity> DbSet;

    protected BaseRepository(IAppDbContext appDbContext)
    {
        AppDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        DbSet = AppDbContext.Set<TEntity>();
    }


    public async Task<TEntity?> InserirAsync(TEntity entity)
    {
        return (await DbSet.AddAsync(entity)).Entity;
    }

    public TEntity? Alterar(TEntity entity)
    {
        return DbSet.Update(entity).Entity;
    }

    public bool Excluir(Guid id)
    {
        var entity = DbSet.Find(id);
        if (entity != null)
        {
            return DbSet.Remove(entity).State == EntityState.Deleted;
        }

        return false;
    }

    public async Task<TEntity?> ObterPorIdAsync(Guid id)
    {
        return await DbSet.FindAsync(new[] { id });
    }
}