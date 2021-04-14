using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Api.Products.Models;

namespace ECommerce.Api.Products
{
    public interface IProductsProvider
    {
        Task<(bool IsSuccess, IEnumerable<Product> Products, string ErrorMessage)> GetProductsAsync();
        Task<(bool IsSuccess, Product Product, string ErrorMessage)> GetProductAsync(int id);
    }
}