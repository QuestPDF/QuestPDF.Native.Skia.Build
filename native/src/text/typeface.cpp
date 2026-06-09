#include "../export.h"

#include "modules/skparagraph/include/ParagraphBuilder.h"
#include "modules/skparagraph/include/TypefaceFontProvider.h"

extern "C" {

QUEST_API void questpdf_skia_typeface_unref(SkTypeface *typeface) {
    typeface->unref();
}

}
