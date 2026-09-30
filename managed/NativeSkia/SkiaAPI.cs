using System;

namespace QuestPDF.Skia;

internal static class SkiaAPI
{
    public const string LibraryName = "questpdf_skia";
    
    public static void EnsureNotNull(IntPtr instance)
    {
        if (instance == IntPtr.Zero)
            throw new InvalidOperationException($"QuestPDF cannot instantiate native object.");
    }
}