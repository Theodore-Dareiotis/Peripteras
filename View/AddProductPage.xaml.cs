namespace Peripteras.View;

public partial class AddProductPage : ContentPage
{
	public AddProductPage(AddProductViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

}