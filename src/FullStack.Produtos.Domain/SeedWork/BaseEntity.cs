using System.ComponentModel.DataAnnotations;

namespace FullStack.Produtos.Domain;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; }    
}