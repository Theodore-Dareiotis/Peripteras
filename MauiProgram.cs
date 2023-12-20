using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Peripteras.View;
using QuestPDF.Infrastructure;
using CommunityToolkit.Maui.Storage;

namespace Peripteras
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fontello.ttf", "Icons");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
            builder.Services.AddSingleton<ProductService>();
            builder.Services.AddSingleton<CartService>();
            builder.Services.AddSingleton<PDFservice>();
            builder.Services.AddSingleton<ProductsViewModel>();
            builder.Services.AddTransient<CartViewModel>();
            builder.Services.AddTransient<CartPage>();
            builder.Services.AddTransient<AddProductViewModel>();
            builder.Services.AddTransient<AddProductPage>();
            builder.Services.AddSingleton<MainPage>();

            QuestPDF.Settings.License = LicenseType.Community;

            return builder.Build();

        }
    }
}
