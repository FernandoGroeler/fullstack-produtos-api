namespace FullStack.Produtos.Domain;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> InserirAsync(TEntity entity);
    Task<TEntity?> AlterarAsync(TEntity entity);
    Task<bool> ExcluirAsync(Guid id);
    Task<TEntity?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<TEntity>> ListarTodosAsync();
}