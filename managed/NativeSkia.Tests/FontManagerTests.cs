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
        Assert.That(typefaces, Has.Length.EqualTo(9));
        Assert.That(typefaces.Any(x => x.FamilyName == "Lato"));
    }

    [Test]
    public static void TypefaceShouldBeRegisteredUnderTypographicFamilyNameOnly()
    {
        using var typefaceProvider = new SkTypefaceProvider();
        RegisterFont(typefaceProvider, "Lato-Light.ttf");

        var typefaces = typefaceProvider.GetTypefaces();

        // the legacy per-style family name ("Lato Light") is intentionally not registered
        Assert.That(typefaces, Is.EqualTo(new[] { new FontInfo("Lato", "Lato-Light", 300, IsItalic: false, IsVariable: false) }));
    }

    [Test]
    public static void TypefaceRegisteredWithAliasShouldBeAvailableUnderAliasAndTypographicFamilyName()
    {
        using var typefaceProvider = new SkTypefaceProvider();
        RegisterFont(typefaceProvider, "Lato-Light.ttf", alias: "QuestPDF Alias Test");

        var typefaces = typefaceProvider.GetTypefaces();

        Assert.That(typefaces, Has.Length.EqualTo(2));
        Assert.That(typefaces, Does.Contain(new FontInfo("QuestPDF Alias Test", "Lato-Light", 300, IsItalic: false, IsVariable: false)));
        Assert.That(typefaces, Does.Contain(new FontInfo("Lato", "Lato-Light", 300, IsItalic: false, IsVariable: false)));
    }
}
