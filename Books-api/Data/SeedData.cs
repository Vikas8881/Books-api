using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books_api.Data
{
    public static class SeedData
    {
        public async static Task Seed(UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
           await SeedRoles(roleManager);
           await SeedUsers(userManager);
        }
        private async static Task SeedUsers(UserManager<IdentityUser> userManager)
        {
         if(await userManager.FindByEmailAsync("admin@gmail.com")==null)
            {
                var user = new IdentityUser
                {
                    UserName = "Admin", 
                    Email = "admin@gmail.com",
                };
               var result= await userManager.CreateAsync(user,"P@ssword1");
            if(result.Succeeded)
                {
                  await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            if (await userManager.FindByEmailAsync("customer@gmail.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "Customer",
                    Email = "Customer@gmail.com",
                };
                var result = await userManager.CreateAsync(user, "P@ssword1");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                }
            }
        }
        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var role = new IdentityRole
                {
                    Name = "Admin"
                };
                await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                var role = new IdentityRole
                {
                    Name = "Customer"
                };
                await roleManager.CreateAsync(role);
            }

        }
    }
}
