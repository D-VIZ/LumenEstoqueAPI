using System.ComponentModel.DataAnnotations;

namespace LumenEstoque.DTOs.CategoriesDTOs;

public class CategoryUpdateDTO
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Este campo pode ter no máximo 50 caracteres e no mínimo 3")]
    public string? Name { get; set; }

    [Required]
    [StringLength(200, ErrorMessage = "Este campo pode ter no máximo 200 caracteres")]
    public string? Description { get; set; }
}
