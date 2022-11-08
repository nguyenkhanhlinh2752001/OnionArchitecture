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
            AddProduct();
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
                    FullName = "UserA",
                    Email = "userA@example.com",
                    UserName = "userA",
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
                _context.Categories.Add(new Category() { Name = "Lalala", CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP" });
                _context.Categories.Add(new Category() { Name = "Lelele", CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP" });
                _context.Categories.Add(new Category() { Name = "Lululu", CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP" });
            }
        }

        private void AddProduct()
        {
            if (!_context.Products.Any())
            {
                _context.Products.Add(new Product() { CategoryId = 1, Name = "Product1", Barcode = "Barcode1", Description = "Description1", Rate = 0, Price = 10, CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP", Quantity = 45 });
                _context.Products.Add(new Product() { CategoryId = 2, Name = "Product2", Barcode = "Barcode2", Description = "Description2", Rate = 0, Price = 20, CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP", Quantity = 53 });
                _context.Products.Add(new Product() { CategoryId = 3, Name = "Product3", Barcode = "Barcode3", Description = "Description3", Rate = 0, Price = 30, CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP", Quantity = 67 });
                _context.Products.Add(new Product() { CategoryId = 1, Name = "Product4", Barcode = "Barcode4", Description = "Description4", Rate = 0, Price = 40, CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP", Quantity = 71 });
                _context.Products.Add(new Product() { CategoryId = 2, Name = "Product5", Barcode = "Barcode5", Description = "Description5", Rate = 0, Price = 50, CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP", Quantity = 114 });
                _context.Products.Add(new Product() { CategoryId = 3, Name = "Product6", Barcode = "Barcode6", Description = "Description6", Rate = 0, Price = 60, CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP", Quantity = 62 });
                _context.Products.Add(new Product() { CategoryId = 1, Name = "Product7", Barcode = "Barcode7", Description = "Description7", Rate = 0, Price = 70, CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP", Quantity = 93 });
            }
        }

        private void AddPermisson()
        {
            if (!_context.Permissons.Any())
            {
                _context.Permissons.Add(new Permission() { RoleId = "9c5247a9-d894-4702-b4ff-a0452a44e75b", MenuId = 1, CanAccess = true, CanAdd = true, CanDelete = true, CanUpdate = true });
                _context.Permissons.Add(new Permission() { RoleId = "9c5247a9-d894-4702-b4ff-a0452a44e75b", MenuId = 2, CanAccess = true, CanAdd = true, CanDelete = true, CanUpdate = true });
                _context.Permissons.Add(new Permission() { RoleId = "9c5247a9-d894-4702-b4ff-a0452a44e75b", MenuId = 3, CanAccess = true, CanAdd = true, CanDelete = true, CanUpdate = true });
                _context.Permissons.Add(new Permission() { RoleId = "9c5247a9-d894-4702-b4ff-a0452a44e75b", MenuId = 4, CanAccess = true, CanAdd = true, CanDelete = true, CanUpdate = true });
                _context.Permissons.Add(new Permission() { RoleId = "9c5247a9-d894-4702-b4ff-a0452a44e75b", MenuId = 5, CanAccess = true, CanAdd = true, CanDelete = true, CanUpdate = true });

                _context.Permissons.Add(new Permission() { RoleId = "ab5f1e11-111d-4c06-bf0c-f45828430b34", MenuId = 1, CanAccess = true, CanAdd = false, CanDelete = false, CanUpdate = false });
                _context.Permissons.Add(new Permission() { RoleId = "ab5f1e11-111d-4c06-bf0c-f45828430b34", MenuId = 2, CanAccess = true, CanAdd = false, CanDelete = false, CanUpdate = false });
                _context.Permissons.Add(new Permission() { RoleId = "ab5f1e11-111d-4c06-bf0c-f45828430b34", MenuId = 3, CanAccess = false, CanAdd = false, CanDelete = false, CanUpdate = false });
                _context.Permissons.Add(new Permission() { RoleId = "ab5f1e11-111d-4c06-bf0c-f45828430b34", MenuId = 4, CanAccess = false, CanAdd = true, CanDelete = true, CanUpdate = true });
                _context.Permissons.Add(new Permission() { RoleId = "ab5f1e11-111d-4c06-bf0c-f45828430b34", MenuId = 5, CanAccess = false, CanAdd = false, CanDelete = false, CanUpdate = false });
            }
        }
    }
}