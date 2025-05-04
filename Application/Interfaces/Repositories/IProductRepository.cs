using Domain.Entities.ProductRelated;

namespace Application.Interfaces.Repositories;
public interface IProductRepository
{
    Task<Product?> GetProductByIdAsync(int productId);
    Task<IEnumerable<Product>> GetProductsByShopAsync(int shopId);
    Task<int> CreateProductAsync(Product product); // it aslso get you the image url
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int productId);

    Task<ProductItem?> GetProductItemByIdAsync(int productItemId);
    Task<IEnumerable<ProductItem>> GetProductItemsByProductIdAsync(int productId);
    Task<int> AddProductItemAsync(ProductItem productItem); 
    Task<bool> UpdateProductItemAsync(ProductItem productItem); // it updates both product item and stock
    Task<bool> DeleteProductItemAsync(int itemId); // it deletes the product item and stock related to




    Task<int> AddProductImageAsync(ProductImage image); // it just add the image to the images table
    
    
    // it calls the AddProductImageAsync method and then add the image id to the product item
    Task<int> AddProductItemImageAsync(int productItemId, ProductImage image);

    Task<bool> DeleteProductImageAsync(int imageId); // just delete the image from images table
    Task<ProductImage> GetProductImageByIdAsync(int imageId);
    /* before calling this function (if you want to delete the image from product item)
        you have to update it first and put null in the image id column
        then call this function to delete the image from images table
     */


    Task<IEnumerable<Product>> SearchProductsAsync(ProductSearchCriteria criteria);

}