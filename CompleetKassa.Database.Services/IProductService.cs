using System.Collections.Generic;
using System.Threading.Tasks;
using CompleetKassa.Database.Core.Services;
using CompleetKassa.Database.Core.Services.ResponseTypes;
using CompleetKassa.Models;

namespace CompleetKassa.Database.Services
{
    public interface IProductService : IService
    {
        #region Product
        #region Read Product
        Task<IListResponse<ProductModel>> GetProductsAsync(int pageSize = 0, int pageNumber = 0);
        Task<IListResponse<ProductModel>> GetProductsWithDetailsAsync(int pageSize = 0, int pageNumber = 0);
        Task<ISingleResponse<ProductModel>> GetProductByIDAsync(int productID);
        Task<ISingleResponse<ProductModel>> GetProductByIDWithDetailsAsync(int productID);
        #endregion Read Product

        #region Write Product
        Task<ISingleResponse<ProductModel>> UpdateProductAsync(ProductModel updates);
        Task<ISingleResponse<ProductModel>> AddProductAsync(ProductModel details);
        Task<ISingleResponse<ProductModel>> RemoveProductAsync(int productID);
        #endregion Write Product
        #endregion Product

        #region Category
        Task<IListResponse<CategoryModel>> GetCategoriesAsync(int pageSize = 0, int pageNumber = 0);
        Task<IListResponse<CategoryModel>> GetCategoriesWithParentCategoriesAsync(int pageSize = 0, int pageNumber = 0);

        Task<ISingleResponse<CategoryModel>> GetCategoryByIDAsync(int categoryID);
        Task<ISingleResponse<CategoryModel>> GetCategoryByIDWithParentCategoryAsync(int categoryID);

        Task<ISingleResponse<CategoryModel>> UpdateCategoryAsync(CategoryModel updates);

        Task<ISingleResponse<CategoryModel>> AddCategoryAsync(CategoryModel details);

        Task<IListResponse<CategoryModel>> AddCategoriesAsync(IEnumerable<CategoryModel> details);

        Task<ISingleResponse<CategoryModel>> RemoveCategoryAsync(int categoryID);
        #endregion Category
    }
}