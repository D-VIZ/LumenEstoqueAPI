using LumenEstoque.DTOs.CategoriesDTOs;
using LumenEstoque.Models;

namespace LumenEstoque.DTOs.Mapping;

public static class CategoryDTOMappingExtensions
{
    public static Category ToEntity(this CategoryCreateDTO dto)
    {
        return new Category
        {
            Name = dto.Name!,
            Description = dto.Description!
        };
    }

    public static void ToUpdate(this Category category, CategoryUpdateDTO dto)
    {
        category.Name = dto.Name!;
        category.Description = dto.Description!;
    }

    public static CategoryReadDTO ToReadDTO(this Category category)
    {
        return new CategoryReadDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt
        };
    }

    public static IEnumerable<CategoryReadDTO> ToReadDTOs(this IEnumerable<Category> categories)
    {
        if (categories == null || !categories.Any())
            return Enumerable.Empty<CategoryReadDTO>();

        return categories.Select(c => c.ToReadDTO());
    }
}