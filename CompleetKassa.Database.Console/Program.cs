using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompleetKassa.Database.Context;
using CompleetKassa.Database.Core.Entities;
using CompleetKassa.Database.ObjectMapper;
using CompleetKassa.Database.Services;
using CompleetKassa.Log;
using CompleetKassa.Log.Core;
using CompleetKassa.Models;
using Microsoft.EntityFrameworkCore;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace CompleetKassa.Database.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();

            #region SQL Server
            //var dbConnection = ConfigurationManager.ConnectionStrings["AppDbConnection"].ConnectionString;
            //container.RegisterType<IDatabaseConnection, DefaultDatabaseConnection>(new InjectionConstructor(dbConnection));
            //var options = new DbContextOptionsBuilder<AppDbContext>()
            //.UseSqlServer(dbConnection)
            //.Options;
            #endregion SQL Server

            #region SQLite
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("Data Source=CompleetKassa.db3;").Options;
            #endregion SQLite

            container.RegisterType<AppDbContext>(new TransientLifetimeManager(), new InjectionConstructor(options));

            container.RegisterType<ObjectMapperProvider>(new TransientLifetimeManager());
            container.RegisterInstance(container.Resolve<ObjectMapperProvider>().Mapper);

            container.RegisterType<IAppUser, AppUser>(new InjectionConstructor(1, "LoggedUser"));
            container.RegisterType<ILogger, Logger>(new InjectionConstructor());
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<IAccountService, AccountService>();

            InitializeDatabase(container).Wait();

            // Get All Products with complete details

        }

        private static async Task InitializeDatabase(IUnityContainer container)
        {
            IProductService productService = container.Resolve<IProductService>();

            await productService.AddProductAsync(CreateProduct(1, brand: "AAA"));
            await productService.AddProductAsync(CreateProduct(2, brand: "BBB"));
            await productService.AddProductAsync(CreateProduct(3, brand: "CCC"));
            await productService.AddProductAsync(CreateProduct(3, brand: "DDD"));

            await productService.AddCategoryAsync(CreateCategory("cat1", "red"));
            await productService.AddCategoryAsync(CreateCategory("cat2", "red"));
            await productService.AddCategoryAsync(CreateCategory("cat11", "red", 1 /*cat1 ID*/));
            await productService.AddCategoryAsync(CreateCategory("cat22", "red", 2 /*cat2 ID*/));
        }

        #region Factory
        private static int _productCount = 0;
        private static ProductModel CreateProduct(int count, string brand, int status = 1)
        {
            int code = 0;

            _productCount++;
            code = 100 * _productCount;

            return new ProductModel
            {
                Code = code.ToString(),
                Name = $"{code}{code}{code}{code}",
                Brand = $"Brand{brand}",
                Price = code,
                SalePrice = (decimal)(code - (code * .1)),
                Image = "Image Path",
                Favorite = true,
                Quantity = _productCount,
                MinimumStock = _productCount,
                Status = status
            };
        }

        private static CategoryModel CreateCategory(string name, string color, int parentID = 0)
        {
            return new CategoryModel
            {
                Name = name,
                Detail = $"{name} details",
                Parent = parentID,
                ParentName = "",
                Status = 1,
                Color = color
            };
        }
        #endregion Factory

        private static async Task UserTest(IUnityContainer container)
        {
            IAccountService repo = container.Resolve<IAccountService>();

            var newUser = new UserModel
            {
                FirstName = "User-" + DateTime.Now.ToString(),
                LastName = "Last Name"
            };


            await repo.AddUserAsync(newUser);

            var updateUser = repo.GetUserByIDAsync(1);
            updateUser.Result.Model.FirstName = "Modified First Name";

            await repo.UpdateUserAsync(updateUser.Result.Model);

            var list = repo.GetUsersAsync().Result.Model.ToList();


            System.Console.WriteLine(list.Count);
        }

        private static async Task UserWithRolesAndResourcesTest(IUnityContainer container)
        {
            IAccountService accountService = container.Resolve<IAccountService>();

            // About this test
            // 1. Create User
            // 2. Create Roles
            // 3. Create Resources
            // 4. Create Role <-> Resources
            // 5. Create User <-> Role

            // 1. Create User
            var newUser = new UserModel
            {
                FirstName = "User-" + DateTime.Now.ToString(),
                LastName = "Last Name",
                UserName = "User Name",
                Password = "Password"
            };

            await accountService.AddUserAsync(newUser);

            #region "Roles"
            // 2. Create Roles
            var role1 = new RoleModel
            {
                Name = "Role 1",
                Description = "Role 1 Description"
            };

            await accountService.AddRoleAsync(role1);

            var role2 = new RoleModel
            {
                Name = "Role 2",
                Description = "Role 2 Description"
            };

            await accountService.AddRoleAsync(role2);
            #endregion "Roles"

            #region "Resources"
            // 3. Create Resources
            var resource1 = new ResourceModel
            {
                Name = "Resource 1",
                Description = "Resource 1 Description"
            };

            await accountService.AddResourceAsync(resource1);

            var resource2 = new ResourceModel
            {
                Name = "Resource 2",
                Description = "Resource 2 Description"
            };

            await accountService.AddResourceAsync(resource2);

            var resource3 = new ResourceModel
            {
                Name = "Resource 3",
                Description = "Resource 3 Description"
            };

            await accountService.AddResourceAsync(resource3);

            var resource4 = new ResourceModel
            {
                Name = "Resource 4",
                Description = "Resource 4 Description"
            };

            await accountService.AddResourceAsync(resource4);
            #endregion "Resources"

            #region Role Resources
            // 4. Create Role <-> Resources


            #endregion Role Resources

        }

        private static async Task AccountServiceTest(IUnityContainer container)
        {
            IAccountService accountService = container.Resolve<IAccountService>();

            #region "Roles"
            // Create Roles
            var role1 = new RoleModel
            {
                Name = "Role 1",
                Description = "Role 1 Description"
            };

            var userRole1 = await accountService.AddRoleAsync(role1);

            var role2 = new RoleModel
            {
                Name = "Role 2",
                Description = "Role 2 Description"
            };

            var userRole2 = await accountService.AddRoleAsync(role2);

            var role3 = new RoleModel
            {
                Name = "Role 3",
                Description = "Role 3 Description"
            };

            var userRole3 = await accountService.AddRoleAsync(role3);
            #endregion "Roles"

            #region "Resources"
            // Create Resources
            var resource1 = new ResourceModel
            {
                Name = "Resource 1",
                Description = "Resource 1 Description"
            };

            var userResource1 = await accountService.AddResourceAsync(resource1);

            var resource2 = new ResourceModel
            {
                Name = "Resource 2",
                Description = "Resource 2 Description"
            };

            var userResource2 = await accountService.AddResourceAsync(resource2);

            var resource3 = new ResourceModel
            {
                Name = "Resource 3",
                Description = "Resource 3 Description"
            };

            var userResource3 = await accountService.AddResourceAsync(resource3);

            var resource4 = new ResourceModel
            {
                Name = "Resource 4",
                Description = "Resource 4 Description"
            };

            var userResource4 = await accountService.AddResourceAsync(resource4);
            #endregion "Resources"

            #region Role Resources
            await accountService.AddRoleResourceAsync(userRole1.Model.ID, userResource1.Model.ID);
            await accountService.AddRoleResourceAsync(userRole1.Model.ID, userResource2.Model.ID);
            await accountService.AddRoleResourceAsync(userRole1.Model.ID, userResource3.Model.ID);
            await accountService.AddRoleResourceAsync(userRole2.Model.ID, userResource1.Model.ID);
            await accountService.AddRoleResourceAsync(userRole2.Model.ID, userResource4.Model.ID);
            await accountService.AddRoleResourceAsync(userRole3.Model.ID, userResource1.Model.ID);
            #endregion Role Resources

            // Create User
            var newUser = new UserModel
            {
                FirstName = "User-" + DateTime.Now.ToString(),
                LastName = "Last Name",
                UserName = "User Name",
                Password = "Password",
                Roles = new List<RoleModel> { userRole1.Model, userRole2.Model }
            };

            var response = await accountService.AddUserAccountAsync(newUser);

            var userResponse = await accountService.AddUserRoleAsync(response.Model.ID, userRole3.Model.ID);
        }

        private static async Task GetUserEagerTest(IUnityContainer container)
        {
            IAccountService accountService = container.Resolve<IAccountService>();

            var response = await accountService.GetFirstOrDefaultAsync(1);
        }
    }
}