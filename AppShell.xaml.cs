using Peripteras.View;

namespace Peripteras;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));
        Routing.RegisterRoute(nameof(AddProductPage), typeof(AddProductPage));
    }
}
