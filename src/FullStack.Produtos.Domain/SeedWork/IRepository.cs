namespace FullStack.Produtos.Domain;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> InserirAsync(TEntity entity);
    TEntity? Alterar(TEntity entity);
    bool Excluir(Guid id);
    Task<TEntity?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<TEntity>> ListarTodosAsync();
}