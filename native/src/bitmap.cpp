#include "export.h"

#include "include/core/SkBitmap.h"
#include "include/core/SkImage.h"
#include "include/core/SkRect.h"
#include "include/core/SkStream.h"
#include "include/encode/SkJpegEncoder.h"
#include "include/encode/SkPngEncoder.h"
#include "include/encode/SkWebpEncoder.h"


extern "C" {

QUEST_API SkBitmap *questpdf_skia_bitmap_create(int width, int height) {
    auto bitmap = new SkBitmap();
    bitmap->allocN32Pixels(width, height);
    return bitmap;
}

QUEST_API void questpdf_skia_bitmap_delete(SkBitmap *bitmap) {
    delete bitmap;
}

QUEST_API SkBitmap *questpdf_skia_bitmap_load_from_image(SkImage *image) {
    auto targetBitmap = new SkBitmap();
    targetBitmap->allocPixels(image->imageInfo());
    image->readPixels(targetBitmap->pixmap(), 0, 0);
    return targetBitmap;
}

QUEST_API SkData *questpdf_skia_bitmap_encode_as_jpg(SkBitmap *bitmap, int quality) {
    SkJpegEncoder::Options encodingOptions;
    encodingOptions.fQuality = quality;

    SkDynamicMemoryWStream stream;
    SkJpegEncoder::Encode(&stream, bitmap->pixmap(), encodingOptions);

    return stream.detachAsData().release();
}

QUEST_API SkData *questpdf_skia_bitmap_encode_as_png(SkBitmap *bitmap) {
    SkPngEncoder::Options encodingOptions;

    SkDynamicMemoryWStream stream;
    SkPngEncoder::Encode(&stream, bitmap->pixmap(), encodingOptions);

    return stream.detachAsData().release();
}

QUEST_API SkData *questpdf_skia_bitmap_encode_as_webp(SkBitmap *bitmap, int quality) {
    SkWebpEncoder::Options encodingOptions;
    encodingOptions.fCompression = SkWebpEncoder::Compression::kLossy;
    encodingOptions.fQuality = static_cast<float>(quality);

    SkDynamicMemoryWStream stream;
    SkWebpEncoder::Encode(&stream, bitmap->pixmap(), encodingOptions);

    return stream.detachAsData().release();
}

}