using System;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia.Text;

internal sealed class SkFontManager
{
    public IntPtr Instance { get; }
    
    public static SkFontManager Local { get; } = new(API.questpdf_skia_font_manager_create_local(AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory));
    public static SkFontManager Global { get; } = new(API.questpdf_skia_font_manager_create_global());

    private SkFontManager(IntPtr instance)
    {
        Instance = instance;
        SkiaAPI.EnsureNotNull(Instance);
    }
    
    public SkTypeface CreateTypeface(SkData data)
    {
        var instance = API.questpdf_skia_font_manager_create_typeface(Instance, data.Instance);
        return new SkTypeface(instance);
    }
    
    private static class API
    {
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr questpdf_skia_font_manager_create_local(string path);
        
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr questpdf_skia_font_manager_create_global();
        
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr questpdf_skia_font_manager_create_typeface(IntPtr fontManager, IntPtr fontData);
    }
}