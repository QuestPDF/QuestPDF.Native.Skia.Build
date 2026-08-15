#include "export.h"

#include "include/core/SkStream.h"
#include "include/core/SkPicture.h"


extern "C" {

QUEST_API void questpdf_skia_picture_unref(SkPicture *picture) {
    picture->unref();
}

}