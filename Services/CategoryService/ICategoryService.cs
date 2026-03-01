using LumenEstoque.DTOs.CategoriesDTOs;
using LumenEstoque.Pagination;

namespace LumenEstoque.Services.CategoryService;

public interface ICategoryService
{
    Task<PagedList<CategoryReadDTO>> GetAllAsync(CategoryParameters categoryParameters);
    Task<CategoryReadDTO?> GetByIdAsync(int id);
    Task<CategoryReadDTO> CreateAsync(CategoryCreateDTO categoryCreateDTO);
    Task<CategoryReadDTO> UpdateAsync(int id, CategoryUpdateDTO categoryUpdateDTO);
    Task<CategoryReadDTO> DeleteAsync(int id);
}
