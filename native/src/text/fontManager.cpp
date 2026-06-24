#include "../export.h"

#include "include/ports/SkFontMgr_directory.h"
#include "modules/skparagraph/include/ParagraphBuilder.h"
#include "modules/skparagraph/include/TypefaceFontProvider.h"

#ifdef __APPLE__

#include "include/ports/SkFontMgr_mac_ct.h"

sk_sp<SkFontMgr> font_manager_create_default_internal() {
    return SkFontMgr_New_CoreText(nullptr);
}

#endif

#ifdef _WIN32

#include "include/ports/SkTypeface_win.h"

sk_sp<SkFontMgr> font_manager_create_default_internal() {
    return SkFontMgr_New_DirectWrite();
}

#endif

#ifdef __linux__

#include "include/ports/SkFontMgr_directory.h"

sk_sp<SkFontMgr> font_manager_create_default_internal() {
    return SkFontMgr_New_Custom_Directory("/usr/share/fonts/");
}

#endif

extern "C" {

QUEST_API SkFontMgr *questpdf_skia_font_manager_create_local(char* path) {
    return SkFontMgr_New_Custom_Directory(path).release();
}

QUEST_API SkFontMgr *questpdf_skia_font_manager_create_global() {
    return font_manager_create_default_internal().release();
}

QUEST_API SkTypeface *questpdf_skia_font_manager_create_typeface(SkFontMgr* fontManager, SkData* fontData) {
    return fontManager->makeFromData(sk_ref_sp(fontData)).release();
}

}
