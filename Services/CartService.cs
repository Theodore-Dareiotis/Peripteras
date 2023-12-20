namespace Peripteras.Services;

public class CartService : ICartService
{
    public List<Product> Products { get; } = new();

    public void AddProduct(Product product)
    {
        var cartProduct = Products.FirstOrDefault(p => p.Id == product.Id);

        if (cartProduct != null)
            return;
        
        Products.Add(product);
        

    }

    public void RemoveProduct(Product product)
    {
        Products.Remove(product);
    }

    public void ClearCart()
    {
        Products.Clear();
    }

        

}
    
    

