using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia;

internal sealed class SkPaint : IDisposable
{
    public IntPtr Instance { get; private set; }
    
    public SkPaint()
    {
        Instance = API.questpdf_skia_paint_create();
        SkiaAPI.EnsureNotNull(Instance);
    }
    
    public void SetSolidColor(uint color)
    {
        API.questpdf_skia_paint_set_solid_color(Instance, color);
    }
    
    public void SetLinearGradient(SkPoint start, SkPoint end, uint[] colors)
    {
        API.questpdf_skia_paint_set_linear_gradient(Instance, in start, in end, colors.Length, colors);
    }
    
    public void SetStroke(float thickness)
    {
        API.questpdf_skia_paint_set_stroke(Instance, thickness);
    }
    
    public void SetDashedPathEffect(float[] intervals)
    {
        API.questpdf_skia_paint_set_dashed_path_effect(Instance, intervals.Length, intervals);
    }
    
    ~SkPaint()
    {
        Dispose();
    }
    
    public void Dispose()
    {
        if (Instance == IntPtr.Zero)
            return;
        
        API.questpdf_skia_paint_delete(Instance);
        Instance = IntPtr.Zero;
        GC.SuppressFinalize(this);
    }
    
    private static class API
    {
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr questpdf_skia_paint_create();
    
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void questpdf_skia_paint_delete(IntPtr paint);
        
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void questpdf_skia_paint_set_solid_color(IntPtr paint, uint color);
        
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void questpdf_skia_paint_set_linear_gradient(IntPtr paint, in SkPoint start, in SkPoint end, int colorsLength, uint[] colors);
        
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void questpdf_skia_paint_set_stroke(IntPtr paint, float thickness);    
        
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void questpdf_skia_paint_set_dashed_path_effect(IntPtr paint, int arrayLength, float[] intervals); 
    }
}