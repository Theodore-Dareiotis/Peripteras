using Microsoft.Maui.Storage;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Peripteras.Services;

public class ProductService
{
    //List<Product> productsList = new();


    public async Task<List<Product>> GetProducts()
    {
        //if (productsList?.Count > 0)
         //   return productsList;
        
        string productsFile = Path.Combine(FileSystem.Current.AppDataDirectory, "products.json");

        if (!File.Exists(productsFile)) 
            await CopyFileToAppDataDirectory("products.json");
        
        using FileStream openStream = File.OpenRead(productsFile);

        var products = await JsonSerializer.DeserializeAsync(openStream, ProductContext.Default.ListProduct);
   
        return products;
    }

    public static async Task CopyFileToAppDataDirectory(string filename)
    {
        string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, filename);
        
        using Stream inputStream = await FileSystem.Current.OpenAppPackageFileAsync(filename);
        using FileStream outputStream = File.Create(targetFile);
        await inputStream.CopyToAsync(outputStream);
        
    }

    public async Task AddProduct(Product product)
    {        
        var products = await GetProducts();
        products.Add(product);
        string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, "products.json");

        var options = new JsonSerializerOptions();
        options.Converters.Add(new IgnorePropertyConverter());
        
        string json = JsonSerializer.Serialize(products, options);
        File.WriteAllText(targetFile, json);

    }
}

public class IgnorePropertyConverter : JsonConverter<Product>
{
    public override Product Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
    public override void Write(Utf8JsonWriter writer, Product value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString(nameof(Product.Id), value.Id);
        writer.WriteString(nameof(Product.Name), value.Name);                
        writer.WriteNumber(nameof(Product.Price), value.Price);
        
        
        writer.WriteEndObject();
    }
}