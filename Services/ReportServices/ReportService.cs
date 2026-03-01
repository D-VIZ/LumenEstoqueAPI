using ClosedXML.Excel;
using LumenEstoque.Context;
using LumenEstoque.Enums;
using LumenEstoque.Pagination;
using Microsoft.EntityFrameworkCore;

namespace LumenEstoque.Services.ReportServices;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;
    public ReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<byte[]> GetStockReportAsync()
    {
        var products = await _context.Products.ToListAsync();

        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Estoque");

        // cabeçalho
        sheet.Cell("A1").Value = "SKU";
        sheet.Cell("B1").Value = "Nome";
        sheet.Cell("C1").Value = "Quantidade";
        sheet.Cell("D1").Value = "Preço";
        sheet.Cell("E1").Value = "Valor Total";

        // dados
        for (int i = 0; i < products.Count; i++)
        {
            var p = products[i];
            sheet.Cell(i + 2, 1).Value = p.Sku;
            sheet.Cell(i + 2, 2).Value = p.Name;
            sheet.Cell(i + 2, 3).Value = p.StockQuantity;
            sheet.Cell(i + 2, 4).Value = p.Price;
            sheet.Cell(i + 2, 5).Value = p.StockQuantity * p.Price;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<byte[]> GetLowStockReportAsync()
    {
        var products = await _context.Products.Where(p => p.StockQuantity <= p.MinStock).ToListAsync();

        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Estoque");

        // cabeçalho
        sheet.Cell("A1").Value = "SKU";
        sheet.Cell("B1").Value = "Nome";
        sheet.Cell("C1").Value = "Quantidade";
        sheet.Cell("D1").Value = "Preço";
        sheet.Cell("E1").Value = "Valor Total";

        // dados
        for (int i = 0; i < products.Count; i++)
        {
            var p = products[i];
            sheet.Cell(i + 2, 1).Value = p.Sku;
            sheet.Cell(i + 2, 2).Value = p.Name;
            sheet.Cell(i + 2, 3).Value = p.StockQuantity;
            sheet.Cell(i + 2, 4).Value = p.Price;
            sheet.Cell(i + 2, 5).Value = p.StockQuantity * p.Price;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<byte[]> GetFromPeriodAsync(ReportParameters reportParameters)
    {
        var products = await _context.Movements.Where(p => p.CreatedAt.Date >= reportParameters.from && p.CreatedAt.Date <= reportParameters.to).Include(p => p.Product).ToListAsync();

        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Estoque");

        // cabeçalho
        sheet.Cell("A1").Value = "Data";
        sheet.Cell("B1").Value = "Produto";
        sheet.Cell("C1").Value = "Quantidade";
        sheet.Cell("D1").Value = "Quantidade anterior";
        sheet.Cell("E1").Value = "Quantidade nova";
        sheet.Cell("F1").Value = "Tipo de movimento";
        sheet.Cell("G1").Value = "Motivo";

        // dados
        for (int i = 0; i < products.Count; i++)
        {
            var p = products[i];
            sheet.Cell(i + 2, 1).Value = p.CreatedAt;
            sheet.Cell(i + 2, 2).Value = p.Product?.Name;
            sheet.Cell(i + 2, 3).Value = p.Quantity;
            sheet.Cell(i + 2, 4).Value = p.PreviousStock;
            sheet.Cell(i + 2, 5).Value = p.NewStock;
            sheet.Cell(i + 2, 6).Value = p.Type switch
            {
                MovementType.Entry => "Entrada",
                MovementType.Exit => "Saída",
                MovementType.Adjust => "Ajuste",
                _ => "Não informado"
            };
            sheet.Cell(i + 2, 7).Value = p.Reason;

        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
