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
            AddReview();
            AddProductDetail();
            AddImageReview();
            AddImageProductDetail();
            AddOrder();
            AddOrderDetail();

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
                _context.Products.Add(new Product() { CategoryId = 1, Name = "Product1", Barcode = "Barcode1", Description = "Description1", Rate = 0, CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP" });
                _context.Products.Add(new Product() { CategoryId = 2, Name = "Product2", Barcode = "Barcode2", Description = "Description2", Rate = 0, CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP" });
                _context.Products.Add(new Product() { CategoryId = 3, Name = "Product3", Barcode = "Barcode3", Description = "Description3", Rate = 0, CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP" });
                _context.Products.Add(new Product() { CategoryId = 1, Name = "Product4", Barcode = "Barcode4", Description = "Description4", Rate = 0, CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP" });
                _context.Products.Add(new Product() { CategoryId = 2, Name = "Product5", Barcode = "Barcode5", Description = "Description5", Rate = 0, CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP" });
                _context.Products.Add(new Product() { CategoryId = 3, Name = "Product6", Barcode = "Barcode6", Description = "Description6", Rate = 0, CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP" });
                _context.Products.Add(new Product() { CategoryId = 1, Name = "Product7", Barcode = "Barcode7", Description = "Description7", Rate = 0, CreatedOn = DateTime.Now, CreatedBy = "NinePlus Solution ERP" });
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

        private void AddReview()
        {
            if (!_context.Reviews.Any())
            {
                _context.Reviews.Add(new Review() { UserId = "e03ae128-10b5-4cec-b329-a7a4a9321741", ProductDetailId = 1, Content = "content1", IsCheck = true, Title = "title1", Rate = 5, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
                _context.Reviews.Add(new Review() { UserId = "e03ae128-10b5-4cec-b329-a7a4a9321741", ProductDetailId = 2, Content = "content2", IsCheck = false, Title = "title2", Rate = 4, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
                _context.Reviews.Add(new Review() { UserId = "002697fe-0cb6-4eed-9614-1d89f5d0bac7", ProductDetailId = 3, Content = "content3", IsCheck = true, Title = "title3", Rate = 3, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
                _context.Reviews.Add(new Review() { UserId = "002697fe-0cb6-4eed-9614-1d89f5d0bac7", ProductDetailId = 1, Content = "content4", IsCheck = true, Title = "title4", Rate = 2, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
                _context.Reviews.Add(new Review() { UserId = "46a40a4f-d53c-4565-b22b-1b780eef57d9", ProductDetailId = 2, Content = "content5", IsCheck = false, Title = "title5", Rate = 1, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
                _context.Reviews.Add(new Review() { UserId = "46a40a4f-d53c-4565-b22b-1b780eef57d9", ProductDetailId = 3, Content = "content6", IsCheck = true, Title = "title6", Rate = 3, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
                _context.Reviews.Add(new Review() { UserId = "002697fe-0cb6-4eed-9614-1d89f5d0bac7", ProductDetailId = 1, Content = "content7", IsCheck = true, Title = "title7", Rate = 4, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
                _context.Reviews.Add(new Review() { UserId = "e03ae128-10b5-4cec-b329-a7a4a9321741", ProductDetailId = 2, Content = "content8", IsCheck = false, Title = "title8", Rate = 5, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
                _context.Reviews.Add(new Review() { UserId = "002697fe-0cb6-4eed-9614-1d89f5d0bac7", ProductDetailId = 3, Content = "content9", IsCheck = true, Title = "title9", Rate = 3, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });

                _context.Reviews.Add(new Review() { UserId = "e03ae128-10b5-4cec-b329-a7a4a9321741", ProductDetailId = 3, Content = "content1", IsCheck = true, Title = "title1", Rate = 5, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
                _context.Reviews.Add(new Review() { UserId = "e03ae128-10b5-4cec-b329-a7a4a9321741", ProductDetailId = 4, Content = "content2", IsCheck = false, Title = "title2", Rate = 4, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
                _context.Reviews.Add(new Review() { UserId = "002697fe-0cb6-4eed-9614-1d89f5d0bac7", ProductDetailId = 5, Content = "content3", IsCheck = true, Title = "title3", Rate = 3, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
                _context.Reviews.Add(new Review() { UserId = "002697fe-0cb6-4eed-9614-1d89f5d0bac7", ProductDetailId = 3, Content = "content4", IsCheck = true, Title = "title4", Rate = 2, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
                _context.Reviews.Add(new Review() { UserId = "46a40a4f-d53c-4565-b22b-1b780eef57d9", ProductDetailId = 4, Content = "content5", IsCheck = false, Title = "title5", Rate = 1, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
                _context.Reviews.Add(new Review() { UserId = "46a40a4f-d53c-4565-b22b-1b780eef57d9", ProductDetailId = 5, Content = "content6", IsCheck = true, Title = "title6", Rate = 3, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
                _context.Reviews.Add(new Review() { UserId = "002697fe-0cb6-4eed-9614-1d89f5d0bac7", ProductDetailId = 6, Content = "content7", IsCheck = true, Title = "title7", Rate = 4, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
                _context.Reviews.Add(new Review() { UserId = "e03ae128-10b5-4cec-b329-a7a4a9321741", ProductDetailId = 7, Content = "content8", IsCheck = false, Title = "title8", Rate = 5, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
                _context.Reviews.Add(new Review() { UserId = "002697fe-0cb6-4eed-9614-1d89f5d0bac7", ProductDetailId = 8, Content = "content9", IsCheck = true, Title = "title9", Rate = 3, CreatedOn = DateTime.Now, CreatedBy = "4f5b5a8a-b552-41df-9904-a723ee1b2068" });
            }
        }

        private void AddProductDetail()
        {
            if (!_context.ProductDetails.Any())
            {
                _context.ProductDetails.Add(new ProductDetail() { Color = "Red", ProductId = 1, Quantity = 50, Price = 35, ImageUrl = "url3234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Yellow", ProductId = 1, Quantity = 45, Price = 45, ImageUrl = "url6234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Blue", ProductId = 1, Quantity = 35, Price = 54, ImageUrl = "url7234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Red", ProductId = 2, Quantity = 78, Price = 70, ImageUrl = "url7234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Yellow", ProductId = 2, Quantity = 23, Price = 80, ImageUrl = "url5234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Blue", ProductId = 2, Quantity = 56, Price = 40, ImageUrl = "url9234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Yellow", ProductId = 3, Quantity = 56, Price = 55, ImageUrl = "url0234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Blue", ProductId = 3, Quantity = 76, Price = 120, ImageUrl = "url2364" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Yellow", ProductId = 3, Quantity = 46, Price = 100, ImageUrl = "url1234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Blue", ProductId = 4, Quantity = 75, Price = 130, ImageUrl = "url2534" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Yellow", ProductId = 4, Quantity = 35, Price = 200, ImageUrl = "url2734" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Blue", ProductId = 4, Quantity = 86, Price = 240, ImageUrl = "url2934" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Yellow", ProductId = 5, Quantity = 57, Price = 81, ImageUrl = "url9234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Blue", ProductId = 5, Quantity = 35, Price = 70, ImageUrl = "url3234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Yellow", ProductId = 5, Quantity = 75, Price = 85, ImageUrl = "url0234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Blue", ProductId = 6, Quantity = 85, Price = 95, ImageUrl = "url1234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Yellow", ProductId = 6, Quantity = 83, Price = 110, ImageUrl = "url4234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Blue", ProductId = 6, Quantity = 95, Price = 120, ImageUrl = "url6234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Yellow", ProductId = 7, Quantity = 34, Price = 135, ImageUrl = "url5234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Blue", ProductId = 7, Quantity = 75, Price = 95, ImageUrl = "url7234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Yellow", ProductId = 7, Quantity = 61, Price = 73, ImageUrl = "url6234" });
                _context.ProductDetails.Add(new ProductDetail() { Color = "Blue", ProductId = 8, Quantity = 70, Price = 70, ImageUrl = "url2934" });
            }
        }

        private void AddImageReview()
        {
            if (!_context.ImageReviews.Any())
            {
                _context.ImageReviews.Add(new ImageReview() { ReviewId = 1, Url = "url1" });
                _context.ImageReviews.Add(new ImageReview() { ReviewId = 1, Url = "url2" });
                _context.ImageReviews.Add(new ImageReview() { ReviewId = 1, Url = "url3" });
                _context.ImageReviews.Add(new ImageReview() { ReviewId = 2, Url = "url4" });
                _context.ImageReviews.Add(new ImageReview() { ReviewId = 2, Url = "url5" });
                _context.ImageReviews.Add(new ImageReview() { ReviewId = 2, Url = "url6" });
                _context.ImageReviews.Add(new ImageReview() { ReviewId = 3, Url = "url7" });
                _context.ImageReviews.Add(new ImageReview() { ReviewId = 3, Url = "url8" });
                _context.ImageReviews.Add(new ImageReview() { ReviewId = 3, Url = "url9" });
            }
        }

        private void AddImageProductDetail()
        {
            if (!_context.ImageProducts.Any())
            {
                _context.ImageProducts.Add(new ImageProduct() { ProductId = 1, Url = "url11" });
                _context.ImageProducts.Add(new ImageProduct() { ProductId = 1, Url = "url11" });
                _context.ImageProducts.Add(new ImageProduct() { ProductId = 1, Url = "url113" });
                _context.ImageProducts.Add(new ImageProduct() { ProductId = 2, Url = "url131" });
                _context.ImageProducts.Add(new ImageProduct() { ProductId = 2, Url = "url121" });
                _context.ImageProducts.Add(new ImageProduct() { ProductId = 2, Url = "url511" });
                _context.ImageProducts.Add(new ImageProduct() { ProductId = 3, Url = "url118" });
                _context.ImageProducts.Add(new ImageProduct() { ProductId = 3, Url = "url311" });
                _context.ImageProducts.Add(new ImageProduct() { ProductId = 3, Url = "url211" });
                _context.ImageProducts.Add(new ImageProduct() { ProductId = 3, Url = "url011" });
            }
        }

        private void AddOrder()
        {
            if (!_context.Orders.Any())
            {
                _context.Orders.Add(new Order() { UserId = "14c249be-2fca-4887-b074-cdd317eba851", TotalPrice = 540, CreatedOn = DateTime.Now });
                _context.Orders.Add(new Order() { UserId = "14c249be-2fca-4887-b074-cdd317eba851", TotalPrice = 120, CreatedOn = DateTime.Now });
                _context.Orders.Add(new Order() { UserId = "4f5b5a8a-b552-41df-9904-a723ee1b2068", TotalPrice = 100, CreatedOn = DateTime.Now });
                _context.Orders.Add(new Order() { UserId = "4f5b5a8a-b552-41df-9904-a723ee1b2068", TotalPrice = 140, CreatedOn = DateTime.Now });
                _context.Orders.Add(new Order() { UserId = "d104f511-9ad9-4b1b-9a80-0c329b50b6b2", TotalPrice = 320, CreatedOn = DateTime.Now });
                _context.Orders.Add(new Order() { UserId = "d104f511-9ad9-4b1b-9a80-0c329b50b6b2", TotalPrice = 330, CreatedOn = DateTime.Now });
            }
        }

        private void AddOrderDetail()
        {
            if (!_context.OrderDetails.Any())
            {
                _context.OrderDetails.Add(new OrderDetail { OrderId = 1, ProductDetailId = 1, CreatedOn = DateTime.Now, Quantity = 4 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 1, ProductDetailId = 2, CreatedOn = DateTime.Now, Quantity = 5 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 1, ProductDetailId = 3, CreatedOn = DateTime.Now, Quantity = 3 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 2, ProductDetailId = 4, CreatedOn = DateTime.Now, Quantity = 2 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 2, ProductDetailId = 5, CreatedOn = DateTime.Now, Quantity = 1 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 3, ProductDetailId = 6, CreatedOn = DateTime.Now, Quantity = 2 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 3, ProductDetailId = 7, CreatedOn = DateTime.Now, Quantity = 3 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 3, ProductDetailId = 8, CreatedOn = DateTime.Now, Quantity = 6 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 4, ProductDetailId = 9, CreatedOn = DateTime.Now, Quantity = 7 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 4, ProductDetailId = 10, CreatedOn = DateTime.Now, Quantity = 4 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 4, ProductDetailId = 11, CreatedOn = DateTime.Now, Quantity = 5 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 5, ProductDetailId = 12, CreatedOn = DateTime.Now, Quantity = 2 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 5, ProductDetailId = 13, CreatedOn = DateTime.Now, Quantity = 8 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 5, ProductDetailId = 14, CreatedOn = DateTime.Now, Quantity = 4 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 5, ProductDetailId = 15, CreatedOn = DateTime.Now, Quantity = 2 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 6, ProductDetailId = 16, CreatedOn = DateTime.Now, Quantity = 5 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 6, ProductDetailId = 17, CreatedOn = DateTime.Now, Quantity = 2 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 6, ProductDetailId = 18, CreatedOn = DateTime.Now, Quantity = 6 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 2, ProductDetailId = 19, CreatedOn = DateTime.Now, Quantity = 9 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 3, ProductDetailId = 20, CreatedOn = DateTime.Now, Quantity = 7 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 4, ProductDetailId = 21, CreatedOn = DateTime.Now, Quantity = 3 });
                _context.OrderDetails.Add(new OrderDetail { OrderId = 1, ProductDetailId = 22, CreatedOn = DateTime.Now, Quantity = 2 });
            }
        }
    }
}