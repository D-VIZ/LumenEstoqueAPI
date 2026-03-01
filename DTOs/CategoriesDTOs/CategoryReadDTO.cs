using LumenEstoque.Models;

namespace LumenEstoque.DTOs.CategoriesDTOs;

public class CategoryReadDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
