using System.Text.Json.Serialization;

namespace Peripteras.Model;

public partial class Product : ObservableObject
{
    public string Id { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }

    [ObservableProperty]
    float amount = 1;

    [ObservableProperty]
    bool isInCart = false;
}

[JsonSerializable(typeof(List<Product>))]
internal sealed partial class ProductContext : JsonSerializerContext
{

}
