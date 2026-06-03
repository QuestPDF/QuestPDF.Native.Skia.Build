using NUnit.Framework;
using QuestPDF.Skia;

namespace NativeSkia.Tests;

public class ImageTests
{
    [Test]
    public void GeneratePlaceholder()
    {
        using var image = SkImage.GeneratePlaceholder(400, 300, 0xFF64B5F6, 0xFFBA68C8);
        Assert.That(image.Width, Is.EqualTo(400));
        Assert.That(image.Height, Is.EqualTo(300));
        Assert.That(image.EncodedDataSize, Is.EqualTo(1659));
        
        var content = image.GetEncodedData();
        content.ShouldHaveSize(1_659);
        
        // blue (top left)
        // purple (bottom right)
        TestFixture.SaveOutput("placeholder.png", content);
    }
    
    [Test]
    public void Resize_Opaque()
    {
        using var sampleImageData = SkData.FromFile("Input/opaque-image.jpg");
        using var sampleImage = SkImage.FromData(sampleImageData);
        
        Assert.That(sampleImage.Width, Is.EqualTo(800));
        Assert.That(sampleImage.Height, Is.EqualTo(600));

        // don't scale images to if target resolution is bigger than original
        using var biggerImage = sampleImage.ResizeAndCompress(40_000, 30_000, 90, true);
        using var biggerImageData = biggerImage.GetEncodedData();
        
        Assert.That(biggerImage.Width, Is.EqualTo(800));
        Assert.That(biggerImage.Height, Is.EqualTo(600));
        biggerImageData.ShouldHaveSize(67_817);
        
        // scale images to if target resolution is smaller than original
        using var smallerImage = sampleImage.ResizeAndCompress(80, 60, 90, true);
        using var smallerImageData = smallerImage.GetEncodedData();
        
        Assert.That(smallerImage.Width, Is.EqualTo(80));
        Assert.That(smallerImage.Height, Is.EqualTo(60));
        smallerImageData.ShouldHaveSize(2_707);
    }
    
    [Test]
    public void ResizeAndCompress_Opaque()
    {
        using var sampleImageData = SkData.FromFile("Input/opaque-image.jpg");
        using var sampleImage = SkImage.FromData(sampleImageData);
        
        // high quality
        using var highQualityImage = sampleImage.ResizeAndCompress(400, 300, 90, false);
        using var highQualityImageData = highQualityImage.GetEncodedData();
        highQualityImageData.ShouldHaveSize(33_970);
        TestFixture.SaveOutput("image_resize_high_quality.jpg", highQualityImageData);
        
        // low quality
        using var lowQualityImage = sampleImage.ResizeAndCompress(400, 300, 10, true);
        using var lowQualityImageData = lowQualityImage.GetEncodedData();
        lowQualityImageData.ShouldHaveSize(4_495);
        TestFixture.SaveOutput("image_resize_low_quality.jpg", lowQualityImageData);
    }
    
    [Test]
    public void ResizeAndCompress_Transparent()
    {
        using var sampleImageData = SkData.FromFile("Input/transparent-image.png");
        using var sampleImage = SkImage.FromData(sampleImageData);
        
        Assert.That(sampleImage.Width, Is.EqualTo(800));
        Assert.That(sampleImage.Height, Is.EqualTo(600));

        using var resizedImage = sampleImage.ResizeAndCompress(200, 150, 90, true);
        using var resizedImageData = resizedImage.GetEncodedData();
        
        Assert.That(resizedImage.Width, Is.EqualTo(200));
        Assert.That(resizedImage.Height, Is.EqualTo(150));
        resizedImageData.ShouldHaveSize(7_230, buffer: 10);
        
        TestFixture.SaveOutput("image_transparent_resized.png", resizedImageData);
        // check if image is transparent
    }
}
