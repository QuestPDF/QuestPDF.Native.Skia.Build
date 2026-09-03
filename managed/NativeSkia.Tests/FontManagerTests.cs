using NUnit.Framework;
using QuestPDF.Skia.Text;
using static NativeSkia.Tests.TestHelpers;

namespace NativeSkia.Tests;

public static class FontManagerTests
{
    [Test]
    public static void GlobalFontManagerShouldHaveRegisteredFonts()
    {
        var typefaces = SkFontManager.Global.GetTypefaces();
        Assert.That(typefaces, Is.Not.Empty);
    }
    
    [Test]
    public static void FreshTypefaceProviderShouldNotHaveRegisteredFonts()
    {
        using var typefaceProvider = new SkTypefaceProvider();
        var typefaces = typefaceProvider.GetTypefaces();
        Assert.That(typefaces, Is.Empty);
    }
    
    [Test]
    public static void ConfiguredTypefaceProviderShouldHaveRegisteredFonts()
    {
        using var typefaceProvider = CreateTypefaceProvider();
        var typefaces = typefaceProvider.GetTypefaces();
        Assert.That(typefaces, Has.Length.EqualTo(10));
        Assert.That(typefaces.Any(x => x.FamilyName == "Lato"));
    }

    [Test]
    public static void TypefaceRegisteredWithAliasShouldBeAvailableUnderAliasAndDeclaredNames()
    {
        using var typefaceProvider = new SkTypefaceProvider();
        RegisterFont(typefaceProvider, "Lato-Light.ttf", alias: "QuestPDF Alias Test");

        var typefaces = typefaceProvider.GetTypefaces();

        Assert.That(typefaces, Has.Length.EqualTo(3));
        Assert.That(typefaces, Does.Contain(new FontInfo("QuestPDF Alias Test", "Lato-Light", 300, IsItalic: false, IsVariable: false)));
        Assert.That(typefaces, Does.Contain(new FontInfo("Lato", "Lato-Light", 300, IsItalic: false, IsVariable: false)));
        Assert.That(typefaces, Does.Contain(new FontInfo("Lato Light", "Lato-Light", 300, IsItalic: false, IsVariable: false)));
    }

    [Test]
    public static void TypefaceRegisteredWithAliasEqualToDeclaredNameShouldNotBeDuplicated()
    {
        using var typefaceProvider = new SkTypefaceProvider();

        // family names are matched case-insensitively, so this alias duplicates the declared "Lato" name
        RegisterFont(typefaceProvider, "Lato-Light.ttf", alias: "LATO");

        var typefaces = typefaceProvider.GetTypefaces();

        Assert.That(typefaces, Has.Length.EqualTo(2));
        Assert.That(typefaces.Count(x => x.FamilyName.Equals("Lato", StringComparison.OrdinalIgnoreCase)), Is.EqualTo(1));
        Assert.That(typefaces, Does.Contain(new FontInfo("Lato Light", "Lato-Light", 300, IsItalic: false, IsVariable: false)));
    }
}
