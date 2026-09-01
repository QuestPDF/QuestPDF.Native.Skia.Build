using NUnit.Framework;
using QuestPDF.Skia;
using QuestPDF.Skia.Text;

namespace NativeSkia.Tests;

internal static class TestHelpers
{
    public static void ShouldHaveSize(this byte[] data, int sizeInBytes, int buffer = 0)
    {
        Assert.That(data.Length, Is.InRange(sizeInBytes - buffer, sizeInBytes + buffer));
    }
    
    public static void ShouldHaveSize(this SkData data, int sizeInBytes, int buffer = 0)
    {
        data.ToBytes().ShouldHaveSize(sizeInBytes, buffer);
    }

    public static void RegisterFont(SkTypefaceProvider typefaceProvider, string fileName, string? alias = null)
    {
        using var typefaceData = SkData.FromFile(Path.Combine(TestFixture.InputPath, fileName));
        typefaceProvider.AddTypefaceFromData(typefaceData, alias);
    }

    public static SkTypefaceProvider CreateTypefaceProvider()
    {
        var typefaceProvider = new SkTypefaceProvider();

        var executionPath = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
        var fontFilePaths = Directory.GetFiles(executionPath, "*.ttf", SearchOption.AllDirectories);

        foreach (var fileName in fontFilePaths)
        {
            using var typefaceData = SkData.FromFile(fileName);
            typefaceProvider.AddTypefaceFromData(typefaceData);
        }

        return typefaceProvider;
    }
}
