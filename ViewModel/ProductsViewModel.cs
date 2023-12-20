using MvvmHelpers;
using Peripteras.View;

namespace Peripteras.ViewModel;

public partial class ProductsViewModel : BaseViewModel
{
    private readonly ProductService productService;
    private readonly CartService cartService;

    private int lastProductIndex;


    // The products array always stores ALL products.
    // Search is performed on this.
    private List<Product> products;
    public List<Product> Products {  get { return products; } }
    // The filteredProducts array stores searched-for products
    private List<Product> filteredProducts;
    // The displayed products. We to add to this ObservableCollection from filteredProducts.
    public ObservableRangeCollection<Product> FilteredProducts { get; set; } = new();

    public ProductsViewModel(ProductService productService, CartService cartService)
    {
        Title = "Peripteras";
        this.productService = productService;
        this.cartService = cartService;

        Task.Run(async () => await GetProductsAsync());
       
    }

    

    [RelayCommand]
    private async Task Search(string query)
    {
        if (IsBusy) return;
        IsBusy = true;

        if (string.IsNullOrWhiteSpace(query))
        {
            filteredProducts = products;
        }
        else
        {
            filteredProducts = products.Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase))?.ToList();
                       
        }
        
        FilteredProducts.ReplaceRange(filteredProducts.Take(30));
        lastProductIndex = 29;

        IsBusy = false;
    }

    

    // Loads all products in memory from the json file.
    // Adds first 40 products in the FilteredProducts ObservableCollection. 
    private async Task GetProductsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            products = await productService.GetProducts();
            filteredProducts = products;
            
            FilteredProducts.Clear();          
            FilteredProducts.AddRange(products.Take(30));

            lastProductIndex = 29;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get products: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void LoadMoreProducts()
    {
        if (IsBusy) return;
        IsBusy = true;

        int range = 30;
        if (lastProductIndex + 30 >= filteredProducts.Count)
            range = filteredProducts.Count - lastProductIndex - 1;


        if (lastProductIndex < filteredProducts.Count)
            FilteredProducts.AddRange(filteredProducts.GetRange(lastProductIndex, range));
        
        
        lastProductIndex += 30;         
        
        IsBusy = false; 
    }

    [RelayCommand]
    private void UpdateCart(Product product)
    {
        if (product.IsInCart)
        {
            cartService.RemoveProduct(product);
            product.IsInCart = false;
        }
        else
        {
            cartService.AddProduct(product);
            product.IsInCart = true;
        }
    }

    [RelayCommand]
    private static void IncrementAmount(Product product) 
    {
        if (product.Amount >= 1 && product.Amount < 200)
        {
            product.Amount++;
        }
        else if (product.Amount < 1)
        {
            product.Amount += 0.1F;
            product.Amount = (float)Math.Round(product.Amount, 1);
        }
    }

    [RelayCommand]
    private static void DecrementAmount(Product product)
    {
        if (product.Amount >= 2)
        {
            product.Amount--;
        }
        else if (product.Amount >= 0.5)
        {
            product.Amount -= 0.5F;
            product.Amount = (float)Math.Round(product.Amount, 1);
        }
    }

    [RelayCommand]
    private async Task GoToCart()
    {
        if (cartService.Products.Count == 0) 
            return;

        await Shell.Current.GoToAsync(nameof(CartPage));
    }

    [RelayCommand]
    private async Task GoToAddProduct()
    {
        await Shell.Current.GoToAsync(nameof(AddProductPage));
    }

}








