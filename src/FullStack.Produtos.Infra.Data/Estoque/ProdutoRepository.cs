using FullStack.Produtos.Domain;

namespace FullStack.Produtos.Infra.Data;

public class ProdutoRepository(IAppDbContext appDbContext) : BaseRepository<Produto>(appDbContext), IProdutoRepository;