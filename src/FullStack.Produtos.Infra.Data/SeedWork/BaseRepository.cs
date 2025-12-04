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
        var entry = await DbSet.AddAsync(entity);
        await AppDbContext.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<TEntity?> AlterarAsync(TEntity entity)
    {
        var entry = DbSet.Update(entity);
        await AppDbContext.SaveChangesAsync();
        return entry.Entity;        
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var entity = await ObterPorIdAsync(id);
        if (entity == null) 
            return false;
        
        var entry = DbSet.Remove(entity); 
        var excluido = entry.State == EntityState.Deleted;
        await AppDbContext.SaveChangesAsync();
        return excluido;
    }

    public async Task<TEntity?> ObterPorIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> ListarTodosAsync()
    {
        return await DbSet.AsNoTracking().ToListAsync();
    }
}