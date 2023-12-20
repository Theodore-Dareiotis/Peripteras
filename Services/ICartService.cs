namespace Peripteras.Services;

public interface ICartService
{
    List<Product> Products { get; }

    void AddProduct(Product product);
    void RemoveProduct(Product product);
    void ClearCart();
}
    
    

