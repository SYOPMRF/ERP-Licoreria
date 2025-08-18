using Licoreria.Domain;
using Licoreria.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize(Roles = "Admin,Cajero")]
public class BillingController : Controller
{
    private readonly ApplicationDbContext _db;
    private const decimal TAX_RATE = 0.19m;

    public BillingController(ApplicationDbContext db) => _db = db;

    public class LineDto { public int ProductId { get; set; } public int Qty { get; set; } }

    [HttpPost]
    public async Task<IActionResult> CrearFactura(int? customerId, List<LineDto> lines)
    {
        if (lines is null || lines.Count == 0) return BadRequest("Sin ítems");

        var prodIds = lines.Select(l => l.ProductId).ToList();
        var prods = await _db.Products.Where(p => prodIds.Contains(p.Id)).ToListAsync();

        var invoice = new Invoice
        {
            CustomerId = customerId,
            CashierUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
        };

        foreach (var l in lines)
        {
            var p = prods.First(x => x.Id == l.ProductId);
            invoice.Lines.Add(new InvoiceLine {
                ProductId = p.Id, Quantity = l.Qty, UnitPrice = p.Price
            });

            _db.InventoryTransactions.Add(new InventoryTransaction {
                ProductId = p.Id, Quantity = l.Qty, Type = InventoryTxnType.Salida, Reason = "Venta"
            });
        }

        invoice.RecalcTotals(TAX_RATE);
        _db.Invoices.Add(invoice);

        await _db.SaveChangesAsync();
        return Json(new { invoice.Id, invoice.Subtotal, invoice.Tax, invoice.Total });
    }
}
