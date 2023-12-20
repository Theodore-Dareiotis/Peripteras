using CommunityToolkit.Maui.Storage;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Colors = QuestPDF.Helpers.Colors;
using IContainer = QuestPDF.Infrastructure.IContainer;

namespace Peripteras.Services;

public class PDFservice
{
    private readonly CartService _cartService;
    private readonly IFileSaver _fileSaver;
    CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

    public PDFservice(CartService cartService, IFileSaver fileSaver)
    {
        _cartService = cartService;
        _fileSaver = fileSaver;
    }

    public async Task Generate()
    {

        //var filePath = Path.Combine(FileSystem.Current.CacheDirectory, "productsList.pdf");

        var model = _cartService.Products;
        var document = new ProductListDocument(model);
        
        using var stream = new MemoryStream();
        
        document.GeneratePdf(stream);
        stream.Position = 0;
        var fileSaverResult = await _fileSaver.SaveAsync("productsList.pdf", stream, default);
       
        if (fileSaverResult.IsSuccessful)
        {
            await Shell.Current.DisplayAlert("Success!", $"File saved successfully.", "OK");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error.", $"File couldn't be saved.", "OK");
        }
    }

}

public class ProductListDocument : IDocument
{
    private List<Product> Model { get; }

    public ProductListDocument(List<Product> model)
    {
        Model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);

                page.DefaultTextStyle(x => x.FontFamily("Arial"));
                

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
    }

    void ComposeHeader(IContainer container)
    {
        var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text("Παραγγελία").Style(titleStyle);

                column.Item().Text(text =>
                {
                    text.Span("Ημερομηνία Έκδοσης: ").SemiBold();
                    text.Span($"{DateTime.Now}");
                });
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(5);

            column.Item().Element(ComposeTable);
        });
    }

    void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25);
                columns.RelativeColumn(3);
                columns.RelativeColumn();
                columns.RelativeColumn();

            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("#");
                header.Cell().Element(CellStyle).Text("Προϊόν");
                header.Cell().Element(CellStyle).Text("Ποσότητα");
                header.Cell().Element(CellStyle).Text("Κωδικός");


                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                }
            });

            foreach (var product in Model)
            {
                table.Cell().Element(CellStyle).Text($"{Model.IndexOf(product) + 1}");
                table.Cell().Element(CellStyle).Text(product.Name);
                table.Cell().Element(CellStyle).Text($"{product.Amount}");
                table.Cell().Element(CellStyle).Text(product.Id);

                static IContainer CellStyle(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            }
        });
    }


}



