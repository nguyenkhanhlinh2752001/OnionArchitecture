using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Persistence.Constants;
using Persistence.Context;
using Persistence.Interfaces;

namespace Persistence.DatabaseSeeder
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly ILogger<DatabaseSeeder> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public DatabaseSeeder(ILogger<DatabaseSeeder> logger, ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            AddAdministrator();
            AddMenu();
            AddPermisson();
            AddCategory();
            
            _context.SaveChanges();
        }

        private void AddAdministrator()
        {
            Task.Run(async () =>
            {
                var adminRole = new Role()
                {
                    Name = RoleConstants.AdministratorRole,
                    Description = "Administrator role with full permission"
                };
                var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    _logger.LogInformation("Seeded Administrator Role.");
                }

                var customerRole = new Role()
                {
                    Name = RoleConstants.CustomerRole,
                    Description = "Customer role with custom permission"
                };
                var employeeRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.CustomerRole);
                if (employeeRoleInDb == null)
                {
                    await _roleManager.CreateAsync(customerRole);
                    _logger.LogInformation("Seeded Customer Role.");
                }

                //Check if User Exists
                var superUser = new User()
                {
                    FullName = "NinePlus Solution ERP",
                    Email = "admin@example.com",
                    UserName = "superadmin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedOn = DateTime.Now,
                };
                var superUserInDb = await _userManager.FindByNameAsync(superUser.UserName);
                if (superUserInDb == null)
                {
                    await _userManager.CreateAsync(superUser, UserConstants.DefaultPassword);
                    var result = await _userManager.AddToRoleAsync(superUser, RoleConstants.AdministratorRole);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Seeded Default SuperAdmin User.");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }
            }).GetAwaiter().GetResult();
        }

        

        private void AddMenu()
        {
            if (!_context.Menus.Any())
            {
                _context.Menus.Add(new Menu() { Name = "Product", Url = "/product" });
                _context.Menus.Add(new Menu() { Name = "Category", Url = "/category" });
                _context.Menus.Add(new Menu() { Name = "User", Url = "/user" });
                _context.Menus.Add(new Menu() { Name = "Order", Url = "/order" });
                _context.Menus.Add(new Menu() { Name = "Role", Url = "/role" });
            }
        }

        private void AddCategory()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category() { Name = "Lalala", CreatedOn = DateTime.Now, CreatedBy = "b4b1cfc8-5b00-450b-8800-cd5bc57b7213" });
                _context.Categories.Add(new Category() { Name = "Lelele", CreatedOn = DateTime.Now, CreatedBy = "b4b1cfc8-5b00-450b-8800-cd5bc57b7213" });
                _context.Categories.Add(new Category() { Name = "Lululu", CreatedOn = DateTime.Now, CreatedBy = "b4b1cfc8-5b00-450b-8800-cd5bc57b7213" });
            }
        }

        

        private void AddPermisson()
        {
            if (!_context.Permissons.Any())
            {
                _context.Permissons.Add(new Permission() { RoleId = "fc2ba846-8d3f-4786-a5b0-b19ae195200d", MenuId = 1, CanAccess = true, CanAdd = true, CanDelete = true, CanUpdate = true });
                _context.Permissons.Add(new Permission() { RoleId = "fc2ba846-8d3f-4786-a5b0-b19ae195200d", MenuId = 2, CanAccess = true, CanAdd = true, CanDelete = true, CanUpdate = true });
                _context.Permissons.Add(new Permission() { RoleId = "fc2ba846-8d3f-4786-a5b0-b19ae195200d", MenuId = 3, CanAccess = true, CanAdd = true, CanDelete = true, CanUpdate = true });
                _context.Permissons.Add(new Permission() { RoleId = "fc2ba846-8d3f-4786-a5b0-b19ae195200d", MenuId = 4, CanAccess = true, CanAdd = true, CanDelete = true, CanUpdate = true });
                _context.Permissons.Add(new Permission() { RoleId = "fc2ba846-8d3f-4786-a5b0-b19ae195200d", MenuId = 5, CanAccess = true, CanAdd = true, CanDelete = true, CanUpdate = true });

                _context.Permissons.Add(new Permission() { RoleId = "a5535804-d840-4ae7-8937-324d8282de8d", MenuId = 1, CanAccess = true, CanAdd = false, CanDelete = false, CanUpdate = false });
                _context.Permissons.Add(new Permission() { RoleId = "a5535804-d840-4ae7-8937-324d8282de8d", MenuId = 2, CanAccess = true, CanAdd = false, CanDelete = false, CanUpdate = false });
                _context.Permissons.Add(new Permission() { RoleId = "a5535804-d840-4ae7-8937-324d8282de8d", MenuId = 3, CanAccess = false, CanAdd = false, CanDelete = false, CanUpdate = false });
                _context.Permissons.Add(new Permission() { RoleId = "a5535804-d840-4ae7-8937-324d8282de8d", MenuId = 4, CanAccess = true, CanAdd = true, CanDelete = true, CanUpdate = true });
                _context.Permissons.Add(new Permission() { RoleId = "a5535804-d840-4ae7-8937-324d8282de8d", MenuId = 5, CanAccess = false, CanAdd = false, CanDelete = false, CanUpdate = false });
            }
        }
    }
}