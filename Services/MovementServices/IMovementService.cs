using LumenEstoque.DTOs.MovementsDTOs;
using LumenEstoque.Pagination;

namespace LumenEstoque.Services.MovementServices;

public interface IMovementService
{
    Task<MovementReadDTO?> GetByIdAsync(int id);
    Task<PagedList<MovementReadDTO>> GetAllAsync(MovementParameters movementParameters);
    Task<MovementReadDTO> InAsync(MovementCreateDTO movementCreateDTO);
    Task<MovementReadDTO> OutAsync(MovementCreateDTO movementCreateDTO);
    Task<MovementReadDTO> AdjustAsync(MovementCreateDTO movementCreateDTO);
    Task<PagedList<MovementReadDTO>> GetByProduct(int id, MovementParameters movementParameters);
}
