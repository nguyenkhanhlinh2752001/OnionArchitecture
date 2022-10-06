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
            AddCategoryData();
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
                    Description = "Employee role with custom permission"
                };
                var employeeRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.CustomerRole);
                if (employeeRoleInDb == null)
                {
                    await _roleManager.CreateAsync(customerRole);
                    _logger.LogInformation("Seeded Employee Role.");
                }

                //Check if User Exists
                var superUser = new User()
                {
                    FullName = "NinePlus Solution ERP",
                    Email = "dtnghia2510@gmail.com",
                    UserName = "superadmin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedDate = DateTime.Now,
                    IsActive = true
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

        private void AddCategoryData()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category() { Name = "Lalala", CreatedDate = DateTime.Now });
                _context.Categories.Add(new Category() { Name = "Lololo", CreatedDate = DateTime.Now });
                _context.Categories.Add(new Category() { Name = "Lelele", CreatedDate = DateTime.Now });
            }
        }
    }
}