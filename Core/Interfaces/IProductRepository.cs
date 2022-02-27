using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIDAsync(int id);
        Task<IReadOnlyList<Product>> GetProductsAsync();  //Being Specific Avout what is returning

        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();  //Being Specific Avout what is returning

        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();  //Being Specific Avout what is returning



    }
}