using System;
using System.Runtime.InteropServices;
using System.Text;

namespace QuestPDF.Skia.Text;

internal sealed class SkFontManager
{
    public IntPtr Instance { get; }
    
    public static SkFontManager Global { get; } = new(API.questpdf_skia_font_manager_create_global());

    private SkFontManager(IntPtr instance)
    {
        Instance = instance;
        SkiaAPI.EnsureNotNull(Instance);
    }
    
    public FontInfo[] GetTypefaces() => GetTypefaces(Instance);

    public FontInfo[] GetTypefacesWithGlyph(int codepoint) => GetTypefacesWithGlyph(Instance, codepoint);
    
    internal static FontInfo[] GetTypefaces(IntPtr fontManagerInstance)
    {
        API.questpdf_skia_font_manager_get_typefaces(fontManagerInstance, out var array, out var arrayLength);
        return MapFontInfos(array, arrayLength);
    }

    internal static FontInfo[] GetTypefacesWithGlyph(IntPtr fontManagerInstance, int codepoint)
    {
        API.questpdf_skia_font_manager_get_typefaces_with_glyph(fontManagerInstance, codepoint, out var array, out var arrayLength);
        return MapFontInfos(array, arrayLength);
    }

    private static FontInfo[] MapFontInfos(IntPtr array, int arrayLength)
    {
        try
        {
            var result = new FontInfo[arrayLength];
            var size = Marshal.SizeOf<API.SkFontInfo>();

            for (var i = 0; i < arrayLength; i++)
            {
                var description = Marshal.PtrToStructure<API.SkFontInfo>(IntPtr.Add(array, i * size));

                result[i] = new FontInfo(
                    FamilyName: DecodeString(description.FamilyName),
                    PostScriptName: DecodeString(description.PostScriptName),
                    Weight: description.Weight,
                    IsItalic: description.IsItalic,
                    IsVariable: description.IsVariable);
            }

            return result;
        }
        finally
        {
            API.questpdf_skia_font_manager_delete_typefaces(array);
        }
        
        // decodes a NUL-terminated UTF-8 string stored in a fixed-size buffer
        static string DecodeString(byte[] buffer)
        {
            var length = Array.IndexOf(buffer, (byte)0);

            if (length < 0)
                length = buffer.Length;

            return Encoding.UTF8.GetString(buffer, 0, length);
        }
    }
    
    private static class API
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct SkFontInfo
        {
            public const int StringBufferLength = 256;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = StringBufferLength)] public byte[] FamilyName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = StringBufferLength)] public byte[] PostScriptName;
            public int Weight;
            [MarshalAs(UnmanagedType.U1)] public bool IsItalic;
            [MarshalAs(UnmanagedType.U1)] public bool IsVariable;
        }
        
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr questpdf_skia_font_manager_create_global();
        
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void questpdf_skia_font_manager_get_typefaces(IntPtr fontManager, out IntPtr array, out int arrayLength);

        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void questpdf_skia_font_manager_get_typefaces_with_glyph(IntPtr fontManager, int codepoint, out IntPtr array, out int arrayLength);

        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void questpdf_skia_font_manager_delete_typefaces(IntPtr array);
    }
}