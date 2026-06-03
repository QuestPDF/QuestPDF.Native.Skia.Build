using NUnit.Framework;
using QuestPDF.Skia;

namespace NativeSkia.Tests;

[SetUpFixture]
public class TestFixture
{
    public const string OutputPath = "Output";
    public const string InputPath = "Input";
    
    [OneTimeSetUp]
    public void SetUp()
    {
        if (Directory.Exists(OutputPath))
            Directory.Delete(OutputPath, true);
        
        Directory.CreateDirectory(OutputPath);
    }
    
    internal static void SaveOutput(string path, SkData data)
    {
        var content = data.ToBytes();
        Assert.That(content.Length, Is.GreaterThan(0));
        SaveOutput(path, content);
    }

    public static void SaveOutput(string path, byte[] data)
    {
        path = Path.Combine(OutputPath, path);
        File.WriteAllBytes(path, data);
    }
}