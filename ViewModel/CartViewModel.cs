namespace Peripteras.ViewModel;

public partial class CartViewModel : BaseViewModel
{
    private readonly CartService _cartService;
    private readonly PDFservice _pdfService;
    
    public ObservableCollection<Product> CartProducts { get; } = new();


    public CartViewModel(CartService cartService, PDFservice pdfService)
    {
        _cartService = cartService;
        _pdfService = pdfService;

        var products = _cartService.Products;
        foreach (var product in products)
        {
            CartProducts.Add(product);
        }
    }

    [RelayCommand]
    private void RemoveProduct(Product product)
    {
        CartProducts.Remove(product);
        _cartService.RemoveProduct(product);
        product.IsInCart = false;
    }

    [RelayCommand]
    private async Task GeneratePDF()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;            
            await _pdfService.Generate();
           
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to create PDF: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    



}
