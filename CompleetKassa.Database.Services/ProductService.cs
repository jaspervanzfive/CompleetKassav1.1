using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using CompleetKassa.Database.Context;
using CompleetKassa.Database.Core.Entities;
using CompleetKassa.Database.Core.Exception;
using CompleetKassa.Database.Core.Services.ResponseTypes;
using CompleetKassa.Database.Entities;
using CompleetKassa.Database.Repositories;
using CompleetKassa.Database.Services.Extensions;
using CompleetKassa.Log.Core;
using CompleetKassa.Models;
using Microsoft.EntityFrameworkCore;

namespace CompleetKassa.Database.Services
{
    public class ProductService : BaseService, IProductService
    {
        private IProductRepository ProductRepository { get; }
        private ICategoryRepository CategoryRepository { get; }

        public ProductService(ILogger logger, IMapper mapper, IAppUser userInfo, AppDbContext dbContext)
            : base(logger, mapper, userInfo, dbContext)
        {
            ProductRepository = new ProductRepository(userInfo, DbContext);
            CategoryRepository = new CategoryRepository(userInfo, DbContext);
        }

        #region Product
        #region Read Product
        public async Task<IListResponse<ProductModel>> GetProductsAsync(int pageSize = 0, int pageNumber = 0)
        {
            Logger?.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new ListResponse<ProductModel>();

            try
            {
                response.Model = await ProductRepository.GetAll(pageSize, pageNumber).Select(o => Mapper.Map<ProductModel>(o)).ToListAsync();
            }
            catch (Exception ex)
            {
                response.SetError(ex, Logger);
            }

            return response;
        }

        public async Task<IListResponse<ProductModel>> GetProductsWithDetailsAsync(int pageSize = 0, int pageNumber = 0)
        {
            Logger?.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new ListResponse<ProductModel>();

            try
            {
                response.Model = await ProductRepository.GetAllWithCategory(pageSize, pageNumber).Select(o => Mapper.Map<ProductModel>(o)).ToListAsync();
            }
            catch (Exception ex)
            {
                response.SetError(ex, Logger);
            }

            return response;
        }

        public async Task<ISingleResponse<ProductModel>> GetProductByIDAsync(int productID)
        {
            Logger?.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new SingleResponse<ProductModel>();

            try
            {
                response.Model = Mapper.Map<ProductModel>(await ProductRepository.GetByIDAsync(productID));
            }
            catch (Exception ex)
            {
                response.SetError(ex, Logger);
            }

            return response;
        }

        public async Task<ISingleResponse<ProductModel>> GetProductByIDWithDetailsAsync(int productID)
        {
            Logger?.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new SingleResponse<ProductModel>();

            try
            {
                response.Model = Mapper.Map<ProductModel>(await ProductRepository.GetByIDWithCategoryAsync(productID));
            }
            catch (Exception ex)
            {
                response.SetError(ex, Logger);
            }

            return response;
        }
        #endregion Read Product

        #region Write Product
        public async Task<ISingleResponse<ProductModel>> AddProductAsync(ProductModel details)
        {
            var response = new SingleResponse<ProductModel>();

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var product = Mapper.Map<Product>(details);

                    await ProductRepository.AddAsync(product);

                    transaction.Commit();

                    response.Model = Mapper.Map<ProductModel>(product);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.SetError(ex, Logger);
                }
            }

            return response;
        }

        public async Task<ISingleResponse<ProductModel>> UpdateProductAsync(ProductModel updates)
        {
            Logger?.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new SingleResponse<ProductModel>();

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    await ProductRepository.UpdateAsync(Mapper.Map<Product>(updates));

                    transaction.Commit();
                    response.Model = updates;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.SetError(ex, Logger);
                }
            }

            return response;
        }

        public async Task<ISingleResponse<ProductModel>> RemoveProductAsync(int productID)
        {
            Logger?.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new SingleResponse<ProductModel>();

            try
            {
                // Retrieve product by id
                Product product = await ProductRepository.GetByIDAsync(productID);
                if (product == null)
                {
                    throw new DatabaseException("Product record not found.");
                }

                await ProductRepository.DeleteAsync(product);
                response.Model = Mapper.Map<ProductModel>(product);
            }
            catch (Exception ex)
            {
                response.SetError(ex, Logger);
            }

            return response;
        }
        #endregion Write Product
        #endregion Product

        #region Category

        public async Task<IListResponse<CategoryModel>> GetCategoriesAsync(int pageSize = 0, int pageNumber = 0)
        {
            Logger?.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new ListResponse<CategoryModel>();

            try
            {
                response.Model = await CategoryRepository.GetAll(pageSize, pageNumber).Select(o => Mapper.Map<CategoryModel>(o)).ToListAsync();
            }
            catch (Exception ex)
            {
                response.SetError(ex, Logger);
            }

            return response;
        }

        public async Task<IListResponse<CategoryModel>> GetCategoriesWithParentCategoriesAsync(int pageSize = 0, int pageNumber = 0)
        {
            Logger?.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new ListResponse<CategoryModel>();

            try
            {
                response.Model = await CategoryRepository.GetAllWithParentCategory(pageSize, pageNumber).Select(o => Mapper.Map<CategoryModel>(o)).ToListAsync();
            }
            catch (Exception ex)
            {
                response.SetError(ex, Logger);
            }

            return response;
        }

        public async Task<ISingleResponse<CategoryModel>> GetCategoryByIDAsync(int CategoryID)
        {
            Logger?.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new SingleResponse<CategoryModel>();

            try
            {
                response.Model = Mapper.Map<CategoryModel>(await CategoryRepository.GetByIDAsync(CategoryID));
            }
            catch (Exception ex)
            {
                response.SetError(ex, Logger);
            }

            return response;
        }

        public async Task<ISingleResponse<CategoryModel>> GetCategoryByIDWithParentCategoryAsync(int CategoryID)
        {
            Logger?.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new SingleResponse<CategoryModel>();

            try
            {
                response.Model = Mapper.Map<CategoryModel>(await CategoryRepository.GetByIDWithParentCategoryAsync(CategoryID));
            }
            catch (Exception ex)
            {
                response.SetError(ex, Logger);
            }

            return response;
        }

        public async Task<ISingleResponse<CategoryModel>> AddCategoryAsync(CategoryModel details)
        {
            Logger?.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new SingleResponse<CategoryModel>();

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var Category = Mapper.Map<Category>(details); ;

                    await CategoryRepository.AddAsync(Category);

                    transaction.Commit();

                    response.Model = Mapper.Map<CategoryModel>(Category);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return response;
        }


        public async Task<IListResponse<CategoryModel>> AddCategoriesAsync(IEnumerable<CategoryModel> details)
        {
            Logger?.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new ListResponse<CategoryModel>();

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    await CategoryRepository.AddAsync(details.Select(o => Mapper.Map<Category>(o)).ToAsyncEnumerable());

                    transaction.Commit();

                    response.Model = details;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return response;
        }

        public async Task<ISingleResponse<CategoryModel>> UpdateCategoryAsync(CategoryModel updates)
        {
            Logger?.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new SingleResponse<CategoryModel>();

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    await CategoryRepository.UpdateAsync(Mapper.Map<Category>(updates));

                    transaction.Commit();
                    response.Model = updates;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.SetError(ex, Logger);
                }
            }

            return response;
        }

        public async Task<ISingleResponse<CategoryModel>> RemoveCategoryAsync(int CategoryID)
        {
            Logger?.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new SingleResponse<CategoryModel>();

            try
            {
                // Retrieve Category by id
                Category Category = await CategoryRepository.GetByIDAsync(CategoryID);
                if (Category == null)
                {
                    throw new DatabaseException("Category record not found.");
                }

                await CategoryRepository.DeleteAsync(Category);
                response.Model = Mapper.Map<CategoryModel>(Category);
            }
            catch (Exception ex)
            {
                response.SetError(ex, Logger);
            }

            return response;
        }
        #endregion Category
    }
}
