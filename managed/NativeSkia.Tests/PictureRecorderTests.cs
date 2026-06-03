using NUnit.Framework;
using QuestPDF.Skia;

namespace NativeSkia.Tests;

public class PictureRecorderTests
{
    [Test]
    public void PictureWorkflow()
    {
        // generate picture
        using var recorder = new SkPictureRecorder();
        using var recorderCanvas = recorder.BeginRecording(400, 300);
        
        recorderCanvas.DrawFilledRectangle(new SkRect(0, 0, 400, 300), 0xFFFFFFFF);
        recorderCanvas.DrawFilledRectangle(new SkRect(50, 50, 250, 150), 0xFF673AB7);
        recorderCanvas.DrawFilledRectangle(new SkRect(100, 100, 300, 200), 0xFF3F51B5);
        recorderCanvas.DrawFilledRectangle(new SkRect(150, 150, 350, 250), 0xFF2196F3);
        
        using var picture = recorder.EndRecording();
        
        // serialization test
        using var serializedPictureData = picture.Serialize();
        Assert.That(serializedPictureData.ToBytes().Length, Is.GreaterThan(0));
        
        var serializedPictureBytes = serializedPictureData.ToBytes();
        using var deserializedPictureData = SkData.FromBinary(serializedPictureBytes);
        
        using var deserializedPicture = SkPicture.Deserialize(deserializedPictureData);
        
        // draw picture
        using var bitmap = new SkBitmap(400, 300);
        using var outputBitmapCanvas = SkCanvas.CreateFromBitmap(bitmap);
        
        outputBitmapCanvas.DrawPicture(deserializedPicture);
        
        // save output
        using var pngData = bitmap.EncodeAsPng();
        TestFixture.SaveOutput("picture.png", pngData);
        pngData.ShouldHaveSize(1_269);
    }
}