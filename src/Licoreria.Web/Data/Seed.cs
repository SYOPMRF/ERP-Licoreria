using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Licoreria.Web.Data;

public static class Seed
{
    public static async Task EnsureRolesAndAdminAsync(IServiceProvider sp)
    {
        var roleMgr = sp.GetRequiredService<RoleManager<IdentityRole>>();
        var userMgr = sp.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = { "Admin", "Cajero", "Inventario", "Consulta" };
        foreach (var r in roles)
            if (!await roleMgr.RoleExistsAsync(r))
                await roleMgr.CreateAsync(new IdentityRole(r));

        var adminEmail = "admin@licoreria.local";
        var admin = await userMgr.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            await userMgr.CreateAsync(admin, "Admin!12345");
            await userMgr.AddToRoleAsync(admin, "Admin");
        }
    }
}
