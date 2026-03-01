using LumenEstoque.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LumenEstoque.DTOs.MovementsDTOs;

public class MovementCreateDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "O valor tem que ser maior que 1")]
    public int Quantity { get; set; }

    [StringLength(100, ErrorMessage = "A razão deve ter no máximo 100 caracteres")]
    public string? Reason { get; set; }

    public string? ReferenceDoc { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O valor do ID tem que ser maior que 1")]
    public int ProductId { get; set; }

    [JsonIgnore]
    public Product? Product { get; set; }
}
