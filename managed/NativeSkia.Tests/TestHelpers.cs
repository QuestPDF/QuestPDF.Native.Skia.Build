using NUnit.Framework;
using QuestPDF.Skia;

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
}