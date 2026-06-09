#include "export.h"

#include "include/core/SkStream.h"
#include "include/core/SkPicture.h"


extern "C" {

QUEST_API SkData *questpdf_skia_picture_serialize(SkPicture *picture) {
    return picture->serialize().release();
}

QUEST_API SkPicture *questpdf_skia_picture_deserialize(SkData *data) {
    return SkPicture::MakeFromData(data, nullptr).release();
}

QUEST_API void questpdf_skia_picture_unref(SkPicture *picture) {
    picture->unref();
}
}