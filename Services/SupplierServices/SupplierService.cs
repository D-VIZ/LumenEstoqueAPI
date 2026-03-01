using LumenEstoque.Context;
using LumenEstoque.DTOs.CategoriesDTOs;
using LumenEstoque.DTOs.Mapping;
using LumenEstoque.DTOs.SuppliersDTOs;
using LumenEstoque.Models;
using LumenEstoque.Pagination;
using Microsoft.EntityFrameworkCore;

namespace LumenEstoque.Services.SupplierServices;

public class SupplierService : ISupplierService
{
    private readonly AppDbContext _context;

    public SupplierService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<SupplierReadDTO>> GetAllAsync(SupplierParameters supplierParameters)
    {
        var suppliers = await PagedList<Supplier>.ToPagedListAsync(
            _context.Suppliers.AsQueryable(),
            supplierParameters.PageNumber,
            supplierParameters.PageSize);

        if (!suppliers.Any())
        {
            return new PagedList<SupplierReadDTO>(new List<SupplierReadDTO>(), 0, supplierParameters.PageNumber, supplierParameters.PageSize);
        }

        return new PagedList<SupplierReadDTO>(suppliers.Select(c => c.ToReadDTO()).ToList(), suppliers.TotalCount, suppliers.CurrentPage, suppliers.PageSize);
    }

    public async Task<SupplierReadDTO> GetByIdAsync(int id)
    {
        var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);

        if (supplier == null)
        {
            throw new KeyNotFoundException($"Fornecedor com ID {id} não encontrado");
        }

        return supplier.ToReadDTO();
    }

    public async Task<SupplierReadDTO> CreateAsync(SupplierCreateDTO supplierCreateDTO)
    {
        var supplier = supplierCreateDTO.ToEntity();

        if (supplier == null)
        {
            throw new ArgumentException("Dados de fornecedor inválidos");
        }

        await _context.Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();

        return supplier.ToReadDTO();
    }

    public async Task<SupplierReadDTO> DeleteAsync(int id)
    {
        var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == id);

        if (supplier == null)
        {
            throw new KeyNotFoundException($"Fornecedor com ID {id} não encontrado");
        }

        bool hasProducts = await _context.Products.AnyAsync(p => p.SupplierId == id);

        if(hasProducts == true)
        {
            throw new InvalidOperationException($"Não é possível excluir o fornecedor com ID {id} porque ele está associado a produtos existentes");
        }

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();

        return supplier.ToReadDTO();
    }

    public async Task<SupplierReadDTO> UpdateAsync(int id, SupplierUpdateDTO supplierUpdateDTO)
    {
        var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);

        if (supplier == null)
        {
            throw new KeyNotFoundException($"Fornecedor com ID {id} não encontrado");
        }

        supplier.UpdatedAt = DateTime.Now;

        supplier.ToUpdate(supplierUpdateDTO);

        _context.Suppliers.Update(supplier);
        await _context.SaveChangesAsync();

        return supplier.ToReadDTO();
    }
}
