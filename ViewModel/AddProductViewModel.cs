namespace Peripteras.ViewModel;
public partial class AddProductViewModel : BaseViewModel
{
    readonly ProductService _productService;
    readonly ProductsViewModel _productsViewModel;

    [ObservableProperty]
    string name;
    [ObservableProperty]
    string id;


    public AddProductViewModel(ProductService productService, ProductsViewModel productsViewModel)
    {
        _productService = productService;
        _productsViewModel = productsViewModel;
    }

    [RelayCommand]
    private async Task AddNewProduct()
    {
        if (IsBusy) return;

        Product newProduct = new()
        {
            Amount = 1,
            Id = Id,
            Name = Name,
            Price = 0,
            IsInCart = false
        };


        try
        {
            IsBusy = false;
            await _productService.AddProduct(newProduct);

        }
        catch (Exception ex) 
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to write to file products.json: {ex.Message}", "OK");
        }
        finally
        {
           IsBusy = true;
        }

        _productsViewModel.Products.Add(newProduct);
        _productsViewModel.FilteredProducts.Add(newProduct);
        await Shell.Current.GoToAsync("..");
    }

   

    [RelayCommand]
    private static async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}


