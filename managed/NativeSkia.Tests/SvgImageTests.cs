using NUnit.Framework;
using QuestPDF.Skia;
using QuestPDF.Skia.Text;

namespace NativeSkia.Tests;

public class SvgImageTests
{
    [Test]
    public void Load()
    {
        var svgContent = File.ReadAllText("Input/icon.svg");
        using var svg = new SkSvgImage(svgContent, SkResourceProvider.Local, SkFontManager.Global);

        Assert.That(svg.Instance, Is.Not.EqualTo(IntPtr.Zero));

        Assert.That(svg.Size.Width, Is.EqualTo(75.3201294f));
        Assert.That(svg.Size.WidthUnit, Is.EqualTo(SkSvgImageSize.Unit.Millimeters));

        Assert.That(svg.Size.Height, Is.EqualTo(92.6041641f));
        Assert.That(svg.Size.HeightUnit, Is.EqualTo(SkSvgImageSize.Unit.Millimeters));

        Assert.That(svg.ViewBox, Is.EqualTo(new SkRect(0, 0, 76f, 93f)));
    }

    [Test]
    public void Svg()
    {
        // read SVG
        var svgContent = File.ReadAllText("Input/icon.svg");
        using var svg = new SkSvgImage(svgContent, SkResourceProvider.Local, SkFontManager.Global);
        
        // create document
        using var memoryStream = new MemoryStream();
        using var skiaStream = new SkWriteStream(memoryStream);
        using var pdf = SkPdfDocument.Create(skiaStream, new SkPdfDocumentMetadata() { CompressDocument = true });
        
        // draw svg in a pdf document
        using var pageCanvas = pdf.BeginPage(800, 600);
        pageCanvas.DrawSvg(svg, 400, 600);
        
        pdf.EndPage();
        pdf.Close();
        skiaStream.Flush();

        var documentData = memoryStream.ToArray();
        TestFixture.SaveOutput("document_svg.pdf", documentData);
        documentData.ShouldHaveSize(3_260);
    }

    [Test]
    public void RenderTextWithRegisteredFonts()
    {
        using var typefaceProvider = new SkTypefaceProvider();
        
        using var typefaceData = SkData.FromFile(Path.Combine(TestFixture.InputPath, "Lato-Regular.ttf" ));
        typefaceProvider.AddTypefaceFromData(typefaceData);
        
        const string svgContent =
            """
            <svg xmlns="http://www.w3.org/2000/svg" width="300" height="100">
                <text x="10" y="50" font-family="Times New Roman" font-size="20">Hello World</text>
            </svg>
            """;
        
        using var svg = new SkSvgImage(svgContent, SkResourceProvider.Local, typefaceProvider);
        
        using var memoryStream = new MemoryStream();
        using var skiaStream = new SkWriteStream(memoryStream);
        using var pdf = SkPdfDocument.Create(skiaStream, new SkPdfDocumentMetadata());
        
        using var pageCanvas = pdf.BeginPage(300, 100);
        pageCanvas.DrawSvg(svg, 300, 100);
        
        pdf.EndPage();
        pdf.Close();
        skiaStream.Flush();

        var documentContent = System.Text.Encoding.Latin1.GetString(memoryStream.ToArray());
        Assert.That(documentContent, Does.Contain("Lato"));
    }
}
