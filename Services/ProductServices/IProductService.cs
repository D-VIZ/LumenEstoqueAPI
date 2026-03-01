using LumenEstoque.DTOs.ProductsDTOs;
using LumenEstoque.Pagination;

namespace LumenEstoque.Services.ProductServices;

public interface IProductService
{
    Task<ProductReadDTO?> GetByIdAsync(int id);
    Task<PagedList<ProductReadDTO>> GetAllAsync(ProductParameters productParameters);
    Task<ProductReadDTO> GetBySku(string sku);
    Task<PagedList<ProductReadDTO>> GetMinStockAsync(ProductParameters productParameters);
    Task<ProductReadDTO> CreateAsync(ProductCreateDTO productCreateDTO);
    Task<ProductReadDTO> UpdateAsync(int id, ProductUpdateDTO productUpdateDTO);
    Task<ProductReadDTO> UpdateMinStockAsync(int id, int quantity);
    Task<ProductReadDTO> UpdateActiveAsync(int id, ProductActiveUpdateDTO productUpdateDTO);
    Task<ProductReadDTO> DeleteAsync(int id);      
}
