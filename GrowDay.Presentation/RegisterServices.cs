using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using Microsoft.AspNetCore.Identity;

namespace GrowDay.Presentation
{
    public static class RegisterServices
    {
        public static void AddIdentityConfigureServices(this IServiceCollection collection)
        {
            collection.AddIdentity<User, IdentityRole>(option => {
            }).AddEntityFrameworkStores<GrowDayDbContext>()
                .AddDefaultTokenProviders();
        }

        public static async void AddRoleServices(this IServiceProvider collection)
        {

            using var container = collection.CreateScope();
            var usermanager = container.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = container.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userRole = await roleManager.RoleExistsAsync("User");
            var adminRole = await roleManager.RoleExistsAsync("Admin");

            if (!adminRole)
                await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            if (!userRole)
                await roleManager.CreateAsync(new IdentityRole { Name = "User" });

            var adminUser = await usermanager.FindByNameAsync("admin");

            if (adminUser is null)
            {

                var result = await usermanager.CreateAsync(new User
                {
                    UserName = "ib5ahim",
                    FirstName = "Ibrahim",
                    LastName = "Asadov",
                    Email = "ibrahimasadov31@gmail.com",
                    EmailConfirmed = true,
                }, "ibrahiM321!");

                if (result.Succeeded)
                {
                    var user = await usermanager.FindByNameAsync("ib5ahim");
                    await usermanager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
