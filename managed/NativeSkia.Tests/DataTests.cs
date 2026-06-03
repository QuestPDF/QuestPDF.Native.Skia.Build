using NUnit.Framework;
using QuestPDF.Skia;

namespace NativeSkia.Tests;

public class DataTests
{
    [Test]
    public void CreateFromFile()
    {
        var path = Path.Combine(TestFixture.InputPath, "image.jpg");
     
        // read file via C# API
        var expectedContent = File.ReadAllBytes(path);
        Assert.That(expectedContent.Length, Is.EqualTo(28_357));
        
        // // read file via NativeSkia API
        using var data = SkData.FromFile(path);
        var actualContent = data.ToBytes();
        
        // compare
        Assert.That(actualContent, Is.EqualTo(expectedContent));
    }
    
    [Test]
    public void CreateFromBinary()
    {
        var path = Path.Combine(TestFixture.InputPath, "image.jpg");
     
        // read file via C# API
        var expectedContent = File.ReadAllBytes(path);
        Assert.That(expectedContent.Length, Is.EqualTo(28_357));
        
        // read file via NativeSkia API
        using var data = SkData.FromBinary(expectedContent);
        var actualContent = data.ToBytes();
        
        // compare
        Assert.That(actualContent, Is.EqualTo(expectedContent));
    }
}