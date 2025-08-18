using Licoreria.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class StorefrontController : Controller
{
    private readonly ApplicationDbContext _db;
    public StorefrontController(ApplicationDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Index(string? q = null, int? categoryId = null)
    {
        var query = _db.Products.Where(p => p.Active);
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(p => p.Name.Contains(q));
        if (categoryId.HasValue) query = query.Where(p => p.CategoryId == categoryId.Value);

        var vm = await query
            .OrderBy(p => p.Name)
            .Select(p => new { p.Id, p.Name, p.Brand, p.Price, p.VolumeMl, p.AlcoholVolumePct })
            .ToListAsync();

        return View(vm);
    }
}
