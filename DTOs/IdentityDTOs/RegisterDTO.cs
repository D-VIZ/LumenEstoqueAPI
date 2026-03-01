using System.ComponentModel.DataAnnotations;

namespace LumenEstoque.DTOs.IdentityDTOs;

public class RegisterDTO
{
    [Required(ErrorMessage = "O campo 'Usuário' é obrigatório.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "O campo 'Senha' é obrigatório.")]
    public string? Password { get; set; }
    
}
