using Licoreria.Domain;
using Licoreria.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin,Inventario")]
public class InventoryController : Controller
{
    private readonly ApplicationDbContext _db;
    public InventoryController(ApplicationDbContext db) => _db = db;

    [HttpPost]
    public async Task<IActionResult> Ingreso(int productId, int qty, string? reason)
    {
        if (qty <= 0) return BadRequest("Cantidad inválida");
        _db.InventoryTransactions.Add(new InventoryTransaction {
            ProductId = productId, Quantity = qty, Type = InventoryTxnType.Ingreso, Reason = reason
        });
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Ajuste(int productId, int qty, string reason)
    {
        if (qty == 0) return BadRequest("Cantidad no puede ser 0");
        _db.InventoryTransactions.Add(new InventoryTransaction {
            ProductId = productId, Quantity = qty, Type = InventoryTxnType.Ajuste, Reason = reason
        });
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Stock()
    {
        // Stock = ingresos + ajustes(±) - salidas
        var q = await _db.Products
            .Select(p => new {
                p.Id, p.SKU, p.Name,
                Stock = _db.InventoryTransactions
                    .Where(t => t.ProductId == p.Id)
                    .Sum(t => t.Type == InventoryTxnType.Salida ? -t.Quantity : t.Quantity)
            }).ToListAsync();

        return Json(q);
    }
}
