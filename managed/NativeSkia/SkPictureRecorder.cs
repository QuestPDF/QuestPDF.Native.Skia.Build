using System.Runtime.InteropServices;

namespace QuestPDF.Skia;

internal sealed class SkPictureRecorder : IDisposable
{
    public IntPtr Instance { get; private set; }
    
    public SkPictureRecorder()
    {
        Instance = API.questpdf_skia_picture_recorder_create();
        SkiaAPI.EnsureNotNull(Instance);
    }

    public SkCanvas BeginRecording(float width, float height)
    {
        var canvasInstance = API.questpdf_skia_picture_recorder_begin_recording(Instance, width, height);
        return new SkCanvas(canvasInstance, disposeNativeObject: false);
    }
    
    public SkPicture EndRecording()
    {
        var pictureInstance = API.questpdf_skia_picture_recorder_end_recording(Instance);
        return new SkPicture(pictureInstance);
    }
    
    ~SkPictureRecorder()
    {
        Dispose();
    }
    
    public void Dispose()
    {
        if (Instance == IntPtr.Zero)
            return;
        
        API.questpdf_skia_picture_recorder_delete(Instance);
        Instance = IntPtr.Zero;
    }
    
    private static class API
    {
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr questpdf_skia_picture_recorder_create();
        
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr questpdf_skia_picture_recorder_begin_recording(IntPtr pictureRecorder, float width, float height);
        
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr questpdf_skia_picture_recorder_end_recording(IntPtr pictureRecorder);
        
        [DllImport(SkiaAPI.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void questpdf_skia_picture_recorder_delete(IntPtr pictureRecorder);
    }
}