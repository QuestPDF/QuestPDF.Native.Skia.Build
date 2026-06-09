#include "../export.h"

#include "modules/skparagraph/include/ParagraphBuilder.h"
#include "modules/skparagraph/include/TypefaceFontProvider.h"

#include "include/ports/SkTypeface_win.h"

extern "C" {

QUEST_API skia::textlayout::TypefaceFontProvider *questpdf_skia_typeface_font_provider_create() {
    return sk_make_sp<skia::textlayout::TypefaceFontProvider>().release();
}

QUEST_API void questpdf_skia_typeface_font_provider_add_typeface(skia::textlayout::TypefaceFontProvider *typefaceFontProvider, SkTypeface* typeface) {
    typefaceFontProvider->registerTypeface(sk_ref_sp(typeface));
}

QUEST_API void questpdf_skia_typeface_font_provider_add_typeface_with_custom_alias(skia::textlayout::TypefaceFontProvider *typefaceFontProvider, SkTypeface* typeface, char *alias) {
    typefaceFontProvider->registerTypeface(sk_ref_sp(typeface), SkString(alias));
}

QUEST_API void questpdf_skia_typeface_font_provider_unref(skia::textlayout::TypefaceFontProvider *typefaceFontProvider) {
    typefaceFontProvider->unref();
}

}
