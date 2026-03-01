using LumenEstoque.Context;
using LumenEstoque.DTOs.Mapping;
using LumenEstoque.DTOs.MovementsDTOs;
using LumenEstoque.Enums;
using LumenEstoque.Models;
using LumenEstoque.Pagination;
using Microsoft.EntityFrameworkCore;

namespace LumenEstoque.Services.MovementServices;

public class MovimentService : IMovementService
{
    private readonly AppDbContext _context;
    public MovimentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<MovementReadDTO>> GetAllAsync(MovementParameters movementParameters)
    {
        var query = _context.Movements!.Include(m => m.Product).AsQueryable();

        query = movementParameters.Type switch
        {
            MovementType.Entry => query = query.Where(m => m.Type == MovementType.Entry),
            MovementType.Exit => query = query.Where(m => m.Type == MovementType.Exit),
            MovementType.Adjust => query = query.Where(m => m.Type == MovementType.Adjust),
            _ => query
        };

        var movements = await PagedList<Movement>.ToPagedListAsync(query.OrderByDescending(m => m.CreatedAt),
            movementParameters.PageNumber,
            movementParameters.PageSize
        );

        if (!movements.Any())
        {
            return new PagedList<MovementReadDTO>(new List<MovementReadDTO>(), 0, movementParameters.PageNumber, movementParameters.PageSize);
        }

        var movementsDto = movements.ToReadDTOs().ToList();
        return new PagedList<MovementReadDTO>(movementsDto, movements.TotalCount, movements.CurrentPage, movements.PageSize);

    }

    public async Task<MovementReadDTO?> GetByIdAsync(int id)
    {
        var movement = await _context.Movements.FirstOrDefaultAsync(m => m.Id == id);
        if (movement == null)
        {
            throw new KeyNotFoundException($"Movimentação com ID {id} não encontrada");
        }

        return movement.ToReadDTO();
    }

    public async Task<MovementReadDTO> AdjustAsync(MovementCreateDTO movementCreateDTO)
    {
        int id = movementCreateDTO.ProductId;
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            throw new KeyNotFoundException($"Produto com ID {id} não encontrado");
        }

        if(movementCreateDTO.Quantity < 1)
        {
            throw new InvalidOperationException($"A quantidade para ajuste deve ser maior que 0. Quantidade fornecida: {movementCreateDTO.Quantity}");
        }

        var movement = movementCreateDTO.ToEntity(product);

        movement.Type = MovementType.Adjust;
        movement.PreviousStock = product.StockQuantity;
        product.StockQuantity = movement.Quantity;
        movement.NewStock = movement.Quantity;

        if (movement == null)
        {
            throw new ArgumentException("Erro ao converter MovementCreateDTO para Movement");
        }

        await _context.Movements.AddAsync(movement);
        await _context.SaveChangesAsync();

        return movement.ToReadDTO();
    }

    public async Task<MovementReadDTO> InAsync(MovementCreateDTO movementCreateDTO)
    {
        int id = movementCreateDTO.ProductId;
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            throw new KeyNotFoundException($"Produto com ID {id} não encontrado");
        }

        var movement = movementCreateDTO.ToEntity(product);

        movement.Type = MovementType.Entry;
        movement.PreviousStock = product.StockQuantity;
        product.StockQuantity += movement.Quantity;
        movement.NewStock = product.StockQuantity;

        if (movement == null)
        {
            throw new ArgumentException("Erro ao converter MovementCreateDTO para Movement");
        }

        await _context.Movements.AddAsync(movement);
        await _context.SaveChangesAsync();

        return movement.ToReadDTO();
    }

    public async Task<MovementReadDTO> OutAsync(MovementCreateDTO movementCreateDTO)
    {
        int id = movementCreateDTO.ProductId;
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            throw new KeyNotFoundException($"Produto com ID {id} não encontrado");
        }

        if(product.StockQuantity < movementCreateDTO.Quantity)
        {
            throw new InvalidOperationException($"Estoque insuficiente para o produto com ID {id}. Estoque atual: {product.StockQuantity}, quantidade solicitada: {movementCreateDTO.Quantity}");
        }

        var movement = movementCreateDTO.ToEntity(product);

        movement.Type = MovementType.Exit;
        movement.PreviousStock = product.StockQuantity;
        product.StockQuantity -= movement.Quantity;
        movement.NewStock = product.StockQuantity;

        if (movement == null)
        {
            throw new ArgumentException("Erro ao converter MovementCreateDTO para Movement");
        }

        await _context.Movements.AddAsync(movement);
        await _context.SaveChangesAsync();

        return movement.ToReadDTO();
    }

    public async Task<PagedList<MovementReadDTO>> GetByProduct(int id, MovementParameters movementParameters)
    {
        var movements = await PagedList<Movement>.ToPagedListAsync(
            _context.Movements!.Include(m => m.Product).Where(m => m.ProductId == id).OrderBy(m => m.CreatedAt),
            movementParameters.PageNumber,
            movementParameters.PageSize
        );

        if (!movements.Any())
        {
            return new PagedList<MovementReadDTO>(new List<MovementReadDTO>(), 0, movementParameters.PageNumber, movementParameters.PageSize);
        }

        var movementsDto = movements.ToReadDTOs().ToList();
        return new PagedList<MovementReadDTO>(movementsDto, movements.TotalCount, movements.CurrentPage, movements.PageSize);
    }
}
