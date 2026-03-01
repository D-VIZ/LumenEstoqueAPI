using System.ComponentModel.DataAnnotations;

namespace LumenEstoque.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Este campo pode ter no máximo 50 caracteres e no mínimo 3")]
    public string? Name { get; set; }

    [Required]
    [StringLength(200, ErrorMessage = "Este campo pode ter no máximo 200 caracteres")]
    public string? Description { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<Product>? Products { get; set; } = new List<Product>();
}

