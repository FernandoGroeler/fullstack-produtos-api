using FullStack.Produtos.Domain;
using Microsoft.EntityFrameworkCore;

namespace FullStack.Produtos.Infra.Data;

public class AppDbContextInMemory(DbContextOptions options) : DbContext(options), IAppDbContext
{
    public DbSet<Produto> Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Produto>().HasData(
            new Produto
            {
                Id = Guid.NewGuid(),
                Nome = "Cafeteira",
                Descricao = "Cafeteira elétrica 1L",
                Preco = 129.90m,
                EstoqueAtual = 10
            },
            new Produto
            {
                Id = Guid.NewGuid(),
                Nome = "Abajur",
                Descricao = "Abajur de mesa",
                Preco = 89.50m,
                EstoqueAtual = 20
            }
        );
    }
}