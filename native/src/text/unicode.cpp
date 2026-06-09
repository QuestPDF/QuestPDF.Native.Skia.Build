#include "../export.h"

#include "modules/skunicode/include/SkUnicode.h"
#include "modules/skunicode/include/SkUnicode_libgrapheme.h"

extern "C" {

QUEST_API SkUnicode *questpdf_skia_unicode_create() {
    return SkUnicodes::Libgrapheme::Make().release();
}

QUEST_API void questpdf_skia_unicode_unref(SkUnicode *unicode) {
    unicode->unref();
}

}  