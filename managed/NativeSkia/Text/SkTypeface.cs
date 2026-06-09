using System.Runtime.InteropServices;

namespace QuestPDF.Skia.Text;

public sealed class SkTypeface : IDisposable
{
    public IntPtr Instance { get; private set; }

    public SkTypeface(IntPtr instance)
    {
        Instance = instance;
        SkiaAPI.EnsureNotNull(Instance);
    }
    
    ~SkTypeface()
    {
        Dispose();
    }
    
    public void Dispose()
    {
        if (Instance == IntPtr.Zero)
            return;
        
        API.questpdf_skia_typeface_unref(Instance);
        Instance = IntPtr.Zero;
    }
    
    private static class API
    {
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void questpdf_skia_typeface_unref(IntPtr typeface);
    }
}