using LumenEstoque.Pagination;

namespace LumenEstoque.Services.ReportServices;

public interface IReportService
{
    Task<byte[]> GetStockReportAsync();
    Task<byte[]> GetLowStockReportAsync();
    Task<byte[]> GetFromPeriodAsync(ReportParameters reportParameters);
}
