using NUnit.Framework;
using QuestPDF.Skia;
using QuestPDF.Skia.Text;

namespace NativeSkia.Tests;

public class SvgCanvasTests
{
    [Test]
    public void Output()
    {
        // read bitmap
        using var imageData = SkData.FromFile("Input/image.jpg");
        using var image = SkImage.FromData(imageData);
        
        // load SVG
        var svgContent = File.ReadAllText("Input/icon.svg");
        using var svgImage = new SkSvgImage(svgContent, SkResourceProvider.Local, SkFontManager.Global);
        
        // draw svg in a pdf document
        using var memoryStream = new MemoryStream();
        using var skiaStream = new SkWriteStream(memoryStream);
        using var svgCanvas = SkSvgCanvas.CreateSvg(800, 600, skiaStream);
        
        svgCanvas.DrawImage(image, 800, 600);
        svgCanvas.DrawSvg(svgImage, 800, 600);
        
        svgCanvas.Dispose();
        skiaStream.Flush();
        
        // save
        var svgData = memoryStream.ToArray();
        TestFixture.SaveOutput("canvas_svg.svg", svgData);
        svgData.ShouldHaveSize(367_118);
    }
}