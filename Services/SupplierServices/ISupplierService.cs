using LumenEstoque.DTOs.SuppliersDTOs;
using LumenEstoque.Pagination;

namespace LumenEstoque.Services.SupplierServices;

public interface ISupplierService
{
    Task<PagedList<SupplierReadDTO>> GetAllAsync(SupplierParameters supplierParameters);
    Task<SupplierReadDTO> GetByIdAsync(int id);
    Task<SupplierReadDTO> CreateAsync(SupplierCreateDTO supplierCreateDTO);
    Task<SupplierReadDTO> UpdateAsync(int id, SupplierUpdateDTO supplierUpdateDTO);
    Task<SupplierReadDTO> DeleteAsync(int id);
}
