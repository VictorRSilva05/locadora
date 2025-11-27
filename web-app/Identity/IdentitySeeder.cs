using Locadora.Dominio.Autenticacao;
using Microsoft.AspNetCore.Identity;

namespace Locadora.WebApp.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        // Criar roles
        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new Role("Admin"));

        if (!await roleManager.RoleExistsAsync("Employee"))
            await roleManager.CreateAsync(new Role("Employee"));

        // Criar admin padrão
        var adminEmail = "admin@local.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail
            };

            await userManager.CreateAsync(adminUser, "Admin123!");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

