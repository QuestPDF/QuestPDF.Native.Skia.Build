#include "../export.h"

#include "modules/skparagraph/include/ParagraphBuilder.h"
#include "modules/skparagraph/include/TypefaceFontProvider.h"

extern "C" {

QUEST_API skia::textlayout::FontCollection *questpdf_skia_font_collection_create(SkFontMgr *fontManager, skia::textlayout::TypefaceFontProvider *typefaceFontProvider) {
    auto fontCollection = new skia::textlayout::FontCollection();

    fontCollection->setAssetFontManager(sk_ref_sp(typefaceFontProvider));
    fontCollection->setDefaultFontManager(sk_ref_sp(fontManager));
    fontCollection->enableFontFallback();
    fontCollection->getParagraphCache()->turnOn(false);

    return fontCollection;
}

QUEST_API void questpdf_skia_font_collection_unref(skia::textlayout::FontCollection *fontCollection) {
    fontCollection->unref();
}

}
