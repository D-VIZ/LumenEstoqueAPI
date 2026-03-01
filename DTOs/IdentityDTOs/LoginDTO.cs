using System.ComponentModel.DataAnnotations;

namespace LumenEstoque.DTOs.IdentityDTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "O campo Usuário é obrigatório.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    public string? Password { get; set; }
}
