using System.ComponentModel.DataAnnotations;

namespace FullStack.Produtos.Domain;

public class Produto : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Descricao { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal Preco { get; set; }
    
    [Range(0, int.MaxValue)]
    public int EstoqueAtual { get; set; }
}