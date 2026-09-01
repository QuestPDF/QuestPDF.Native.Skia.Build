using System.Runtime.InteropServices;

namespace QuestPDF.Skia;

internal sealed class SkResourceProvider
{
    public IntPtr Instance { get; private set; }
    
    public static SkResourceProvider Local { get; } = new();
    
    private SkResourceProvider()
    {
        var resourcesPath = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
        
        Instance = API.questpdf_skia_resource_provider_create(resourcesPath);
        SkiaAPI.EnsureNotNull(Instance);
    }
    
    private static class API
    {
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr questpdf_skia_resource_provider_create(string resourcesPath);
    }
}