using LumenEstoque.Enums;

namespace LumenEstoque.Pagination;

public class MovementParameters : QueryStringParameters
{
    public MovementType? Type { get; set; } = null!; 
}
