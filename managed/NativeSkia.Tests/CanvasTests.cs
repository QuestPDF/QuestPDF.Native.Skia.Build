using NUnit.Framework;
using QuestPDF.Skia;

namespace NativeSkia.Tests;

public class CanvasTests
{
    [Test]
    public void DrawRectangle()
    {
        // prepare
        using var bitmap = new SkBitmap(400, 300);
        using var canvas = SkCanvas.CreateFromBitmap(bitmap);
        
        canvas.DrawFilledRectangle(new SkRect(0, 0, 400, 300), 0xFFFFFFFF);
        
        // blue filled rectangle
        canvas.DrawFilledRectangle(new SkRect(50, 50, 250, 250), 0xFF3F51B5);
        
        // pink stroked rectangle
        canvas.DrawStrokeRectangle(new SkRect(100, 100, 350, 200), 20, 0xFFE91E63);
        
        // save output
        using var pngData = bitmap.EncodeAsPng();
        TestFixture.SaveOutput("canvas_draw_rectangle.png", pngData);
        pngData.ShouldHaveSize(1_273);
    }
    
    [Test]
    public void DrawImage()
    {
        // prepare
        using var bitmap = new SkBitmap(400, 300);
        using var canvas = SkCanvas.CreateFromBitmap(bitmap);
        
        canvas.DrawFilledRectangle(new SkRect(0, 0, 400, 300), 0xFFFFFFFF);
        
        // test
        using var imageData = SkData.FromFile("Input/image.jpg");
        using var image = SkImage.FromData(imageData);
        
        canvas.Translate(40, 30);
        canvas.DrawImage(image, 320, 240);
        
        // save output
        using var pngData = bitmap.EncodeAsPng();
        TestFixture.SaveOutput("canvas_draw_image.png", pngData);
        pngData.ShouldHaveSize(81_150, buffer: 150);
    }
    
    [Test]
    public void Translate()
    {
        // prepare
        using var bitmap = new SkBitmap(300, 300);
        using var canvas = SkCanvas.CreateFromBitmap(bitmap);
        
        canvas.DrawFilledRectangle(new SkRect(0, 0, 400, 300), 0xFFFFFFFF);
        
        // test
        canvas.Translate(50, 50);
        canvas.DrawFilledRectangle(new SkRect(0, 0, 100, 100), 0xFF673AB7);
        
        canvas.Translate(50, 50);
        canvas.DrawFilledRectangle(new SkRect(0, 0, 100, 100), 0xFF3F51B5);
        
        canvas.Translate(50, 50);
        canvas.DrawFilledRectangle(new SkRect(0, 0, 100, 100), 0xFF2196F3);
        
        // save output
        using var pngData = bitmap.EncodeAsPng();
        TestFixture.SaveOutput("canvas_translate.png", pngData);
        pngData.ShouldHaveSize(1_191);
    }
    
    [Test]
    public void Rotate()
    {
        // prepare
        using var bitmap = new SkBitmap(400, 400);
        using var canvas = SkCanvas.CreateFromBitmap(bitmap);
        
        canvas.DrawFilledRectangle(new SkRect(0, 0, 400, 400), 0xFFFFFFFF);
        
        // test
        canvas.Translate(200, 200);
        canvas.DrawFilledRectangle(new SkRect(-100, -100, 100, 100), 0xFF673AB7);
        
        canvas.Rotate(30);
        canvas.DrawFilledRectangle(new SkRect(-100, -100, 100, 100), 0xFF3F51B5);
        
        canvas.Rotate(30);
        canvas.DrawFilledRectangle(new SkRect(-100, -100, 100, 100), 0xFF2196F3);
        
        // save output
        using var pngData = bitmap.EncodeAsPng();
        TestFixture.SaveOutput("canvas_rotate.png", pngData);
        pngData.ShouldHaveSize(11_060);
    }
    
    [Test]
    public void Scale()
    {
        // prepare
        using var bitmap = new SkBitmap(300, 300);
        using var canvas = SkCanvas.CreateFromBitmap(bitmap);
        
        canvas.DrawFilledRectangle(new SkRect(0, 0, 300, 300), 0xFFFFFFFF);
        
        // test
        canvas.Translate(50, 50);
        canvas.DrawFilledRectangle(new SkRect(0, 0, 200, 200), 0xFF673AB7);
        
        canvas.Scale(0.5f, 0.75f);
        canvas.DrawFilledRectangle(new SkRect(0, 0, 200, 200), 0xFF3F51B5);
        
        canvas.Scale(0.75f, 0.5f);
        canvas.DrawFilledRectangle(new SkRect(0, 0, 200, 200), 0xFF2196F3);
        
        // save output
        using var pngData = bitmap.EncodeAsPng();
        TestFixture.SaveOutput("canvas_scale.png", pngData);
        pngData.ShouldHaveSize(1_141);
    }
    
    [Test]
    public void Save_Restore()
    {
        // prepare
        using var bitmap = new SkBitmap(500, 200);
        using var canvas = SkCanvas.CreateFromBitmap(bitmap);
        
        canvas.DrawFilledRectangle(new SkRect(0, 0, 500, 200), 0xFFFFFFFF);
        
        // test
        canvas.Save();
        canvas.Translate(50, 50);
        canvas.DrawFilledRectangle(new SkRect(0, 0, 100, 100), 0xFF673AB7);
        canvas.Restore();
        
        canvas.Save();
        canvas.Translate(200, 50);
        canvas.DrawFilledRectangle(new SkRect(0, 0, 100, 100), 0xFF3F51B5);
        canvas.Restore();
        
        canvas.Save();
        canvas.Translate(350, 50);
        canvas.DrawFilledRectangle(new SkRect(0, 0, 100, 100), 0xFF2196F3);
        canvas.Restore();
        
        // save output
        using var pngData = bitmap.EncodeAsPng();
        TestFixture.SaveOutput("canvas_scale.png", pngData);
        pngData.ShouldHaveSize(973);
    }

    [Test]
    public void DrawPath()
    {
        const string icon = "M7.5,2C5.71,3.15 4.5,5.18 4.5,7.5C4.5,9.82 5.71,11.85 7.53,13C4.46,13 2,10.54 2,7.5A5.5,5.5 0 0,1 7.5,2M19.07,3.5L20.5,4.93L4.93,20.5L3.5,19.07L19.07,3.5M12.89,5.93L11.41,5L9.97,6L10.39,4.3L9,3.24L10.75,3.12L11.33,1.47L12,3.1L13.73,3.13L12.38,4.26L12.89,5.93M9.59,9.54L8.43,8.81L7.31,9.59L7.65,8.27L6.56,7.44L7.92,7.35L8.37,6.06L8.88,7.33L10.24,7.36L9.19,8.23L9.59,9.54M19,13.5A5.5,5.5 0 0,1 13.5,19C12.28,19 11.15,18.6 10.24,17.93L17.93,10.24C18.6,11.15 19,12.28 19,13.5M14.6,20.08L17.37,18.93L17.13,22.28L14.6,20.08M18.93,17.38L20.08,14.61L22.28,17.15L18.93,17.38M20.08,12.42L18.94,9.64L22.28,9.88L20.08,12.42M9.63,18.93L12.4,20.08L9.87,22.27L9.63,18.93Z";
        
        // prepare
        using var bitmap = new SkBitmap(300, 300);
        using var canvas = SkCanvas.CreateFromBitmap(bitmap);
        
        canvas.DrawFilledRectangle(new SkRect(0, 0, 300, 300), 0xFFFFFFFF);
        
        // draw text
        canvas.Translate(30, 30);
        canvas.Scale(10, 10);
        canvas.DrawSvgPath(icon, 0xFF3F51B5);

        // save output
        using var pngData = bitmap.EncodeAsPng();
        TestFixture.SaveOutput("canvas_svg_path.png", pngData);
        pngData.ShouldHaveSize(10_354);
    }
    
    [Test]
    public void DrawOverflowArea()
    {
        // prepare
        using var bitmap = new SkBitmap(350, 400);
        using var canvas = SkCanvas.CreateFromBitmap(bitmap);
        
        canvas.DrawFilledRectangle(new SkRect(0, 0, 350, 400), 0xFFFFFFFF);
        
        // draw area
        var availableSpace = new SkRect(0, 0, 200, 250);
        var neededSpace = new SkRect(0, 0, 250, 300);
        
        canvas.Translate(50, 50);
        canvas.DrawFilledRectangle(availableSpace, 0xFF81C784);
        
        canvas.Save();
        canvas.ClipOverflowArea(availableSpace, neededSpace);
        canvas.DrawOverflowArea(neededSpace);
        canvas.Restore();
        
        canvas.DrawStrokeRectangle(neededSpace, 2f, 0xFFF44336);
        
        // save output
        using var pngData = bitmap.EncodeAsPng();
        TestFixture.SaveOutput("canvas_overflow_area.png", pngData);
        pngData.ShouldHaveSize(15_075);
    }
    
    [Test]
    public void GetMatrix()
    {
        // prepare
        using var bitmap = new SkBitmap(350, 400);
        using var canvas = SkCanvas.CreateFromBitmap(bitmap);
        
        // perform transformations
        canvas.Translate(15, 25);
        canvas.Scale(2, 3);
        canvas.Translate(35, 45);
        canvas.Scale(4, 5);
        
        // assert
        var currentMatrix = canvas.GetCurrentMatrix();

        var expectedValue = new SkCanvasMatrix()
        {
            ScaleX = 8,
            ScaleY = 15,
            TranslateX = 85,
            TranslateY = 160,
            Perspective3 = 1
        };
        
        Assert.That(currentMatrix, Is.EqualTo(expectedValue));
    }
    
    [Test]
    public void SetMatrix()
    {
        // prepare
        using var bitmap = new SkBitmap(350, 400);
        using var canvas = SkCanvas.CreateFromBitmap(bitmap);

        var newValue = new SkCanvasMatrix
        {
            ScaleX = 2,
            ScaleY = 3,
            TranslateX = 85,
            TranslateY = 160,
            Perspective3 = 1
        };
        
        // perform transformations
        canvas.SetCurrentMatrix(newValue);
        canvas.Translate(10, 20);
        
        // assert
        var expectedValue = newValue with { TranslateX = 105, TranslateY = 220 };
        var currentMatrix = canvas.GetCurrentMatrix();
        
        Assert.That(currentMatrix, Is.EqualTo(expectedValue));
    }
    
    [Test]
    public void DrawLine()
    {
        // prepare
        using var bitmap = new SkBitmap(400, 300);
        using var canvas = SkCanvas.CreateFromBitmap(bitmap);
        
        canvas.DrawFilledRectangle(new SkRect(0, 0, 400, 400), 0xFFFFFFFF);
        canvas.Translate(25, 25);

        // draw
        using var paintThin = new SkPaint();
        paintThin.SetSolidColor(0xFF2196F3);
        paintThin.SetStroke(4);
        canvas.DrawLine(new SkPoint(0, 0), new SkPoint(350, 100), paintThin);
        
        using var paintThick = new SkPaint();
        paintThick.SetSolidColor(0xFF4CAF50);
        paintThick.SetStroke(12);
        canvas.DrawLine(new SkPoint(0, 50), new SkPoint(350, 150), paintThick);
        
        using var paintDashed = new SkPaint();
        paintDashed.SetSolidColor(0xFFFFC107);
        paintDashed.SetStroke(8);
        paintDashed.SetDashedPathEffect([2, 2, 4, 4, 8, 8]);
        canvas.DrawLine(new SkPoint(0, 100), new SkPoint(350, 200), paintDashed);
        
        using var paintGradient = new SkPaint();
        paintGradient.SetStroke(8);
        paintGradient.SetLinearGradient(new SkPoint(0, 150), new SkPoint(350, 250), [0xFF2196F3, 0xFF4CAF50, 0xFFFFC107]);
        canvas.DrawLine(new SkPoint(0, 150), new SkPoint(350, 250), paintGradient);
        
        // assert
        using var pngData = bitmap.EncodeAsPng();
        TestFixture.SaveOutput("canvas_line.png", pngData);
        pngData.ShouldHaveSize(12_711);
    }
    
    [Test]
    public void DrawRoundedRectangle()
    {
        // prepare
        using var bitmap = new SkBitmap(300, 300);
        using var canvas = SkCanvas.CreateFromBitmap(bitmap);
        
        canvas.DrawFilledRectangle(new SkRect(0, 0, 300, 300), 0xFFFFFFFF);
        canvas.Translate(50, 50);
        
        // draw
        using var boxPaint = new SkPaint(); 
        boxPaint.SetLinearGradient(new SkPoint(0, 0), new SkPoint(200, 200), [0xFF2196F3, 0xFF4CAF50, 0xFFFFC107]);

        var roundedRect = new SkRoundedRect
        {
            Rect = new SkRect(0, 0, 200, 200),
            TopLeftRadius = new SkPoint(0, 0),
            TopRightRadius = new SkPoint(10, 10),
            BottomLeftRadius = new SkPoint(20, 20),
            BottomRightRadius = new SkPoint(40, 40)
        };

        canvas.Save();
        canvas.ClipRoundedRectangle(roundedRect);
        canvas.DrawRectangle(roundedRect.Rect, boxPaint);
        canvas.Restore();

        // assert
        using var pngData = bitmap.EncodeAsPng();
        TestFixture.SaveOutput("canvas_rounded_rectangle.png", pngData);
        pngData.ShouldHaveSize(3_986, buffer: 250);
    }
    
    [Test]
    public void DrawComplexBorder()
    {
        // prepare
        using var bitmap = new SkBitmap(300, 300);
        using var canvas = SkCanvas.CreateFromBitmap(bitmap);
        
        canvas.DrawFilledRectangle(new SkRect(0, 0, 300, 300), 0xFFFFFFFF);
        canvas.Translate(50, 50);
        
        // draw
        using var boxPaint = new SkPaint(); 
        boxPaint.SetLinearGradient(new SkPoint(0, 0), new SkPoint(200, 200), [0xFF2196F3, 0xFF4CAF50, 0xFFFFC107]);

        var outerRect = new SkRoundedRect
        {
            Rect = new SkRect(0, 0, 200, 200),
            TopLeftRadius = new SkPoint(0, 0),
            TopRightRadius = new SkPoint(10, 10),
            BottomLeftRadius = new SkPoint(20, 20),
            BottomRightRadius = new SkPoint(40, 40)
        };
        
        var innerRect = new SkRoundedRect
        {
            Rect = new SkRect(10, 10, 190, 190),
            TopLeftRadius = new SkPoint(0, 0),
            TopRightRadius = new SkPoint(0, 0),
            BottomLeftRadius = new SkPoint(10, 10),
            BottomRightRadius = new SkPoint(30, 30)
        };

        canvas.DrawComplexBorder(innerRect, outerRect, boxPaint);

        // assert
        using var pngData = bitmap.EncodeAsPng();
        TestFixture.SaveOutput("canvas_complex_border.png", pngData);
        pngData.ShouldHaveSize(4_454, buffer: 50);
    }
    
    [Test]
    public void DrawShadow()
    {
        // prepare
        using var bitmap = new SkBitmap(800, 400);
        using var canvas = SkCanvas.CreateFromBitmap(bitmap);
        
        canvas.DrawFilledRectangle(new SkRect(0, 0, 800, 400), 0xFFFFFFFF);
        
        // draw
        using var boxPaint = new SkPaint(); 
        boxPaint.SetSolidColor(0xFF795548);

        var roundedRect = new SkRoundedRect
        {
            Rect = new SkRect(0, 0, 200, 200),
            TopLeftRadius = new SkPoint(0, 0),
            TopRightRadius = new SkPoint(10, 10),
            BottomLeftRadius = new SkPoint(20, 20),
            BottomRightRadius = new SkPoint(40, 40)
        };
        
        // rectangle 1
        canvas.Translate(100, 100);
        canvas.DrawShadow(roundedRect, new SkBoxShadow
        {
            Color = 0xFF000000,
            OffsetX = 5,
            OffsetY = 5,
            Blur = 0
        });
        
        canvas.Save();
        canvas.ClipRoundedRectangle(roundedRect);
        canvas.DrawRectangle(roundedRect.Rect, boxPaint);
        canvas.Restore();
        
        // rectangle 2
        canvas.Translate(400, 0);
        canvas.DrawShadow(roundedRect, new SkBoxShadow
        {
            Color = 0xFF000000,
            OffsetX = 0,
            OffsetY = 0,
            Blur = 10
        });
        
        canvas.Save();
        canvas.ClipRoundedRectangle(roundedRect);
        canvas.DrawRectangle(roundedRect.Rect, boxPaint);
        canvas.Restore();
        
        // assert
        using var pngData = bitmap.EncodeAsPng();
        TestFixture.SaveOutput("canvas_shadow.png", pngData);
        pngData.ShouldHaveSize(7_948);
    }
    
    [Test]
    public void ClipRectangle()
    {
        // prepare
        using var bitmap = new SkBitmap(400, 300);
        using var canvas = SkCanvas.CreateFromBitmap(bitmap);
        
        canvas.DrawFilledRectangle(new SkRect(0, 0, 400, 300), 0xFFFFFFFF);
        
        // draw
        var roundedRect = new SkRoundedRect
        {
            Rect = new SkRect(25, 25, 375, 275),
            TopLeftRadius = new SkPoint(0, 0),
            TopRightRadius = new SkPoint(10, 10),
            BottomLeftRadius = new SkPoint(20, 20),
            BottomRightRadius = new SkPoint(40, 40)
        };

        var outerRect = new SkRect(100, 0, 300, 300);
        
        using var imageData = SkData.FromFile("Input/image.jpg");
        using var image = SkImage.FromData(imageData);
        
        canvas.Save();
        canvas.ClipRoundedRectangle(roundedRect);  
        canvas.DrawImage(image, 400, 300);
        canvas.Restore();
        
        canvas.Save();
        canvas.ClipRectangle(outerRect);
        canvas.DrawImage(image, 400, 300);
        canvas.Restore();
         
        // assert
        using var pngData = bitmap.EncodeAsPng();
        TestFixture.SaveOutput("canvas_clip_rectangle.png", pngData);
        pngData.ShouldHaveSize(98_674, buffer: 10);
    }
}
