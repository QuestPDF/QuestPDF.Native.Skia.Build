#include "../export.h"

#include <set>
#include <string>
#include <vector>

#include "include/core/SkData.h"
#include "include/core/SkFontMgr.h"
#include "include/core/SkFontScanner.h"
#include "include/core/SkStream.h"
#include "include/core/SkTypeface.h"
#include "include/ports/SkFontMgr_empty.h"
#include "include/ports/SkFontScanner_FreeType.h"
#include "modules/skparagraph/include/ParagraphBuilder.h"
#include "modules/skparagraph/include/TypefaceFontProvider.h"

static std::string toLowerAscii(const SkString& value) {
    std::string result(value.c_str(), value.size());

    for (auto& character : result) {
        if (character >= 'A' && character <= 'Z')
            character += 'a' - 'A';
    }

    return result;
}

static void registerTypefaceUnderAllFamilyNames(skia::textlayout::TypefaceFontProvider* typefaceFontProvider, SkTypeface* typeface, const char* alias) {
    std::set<std::string> registeredFamilyNames;

    auto registerFamilyName = [&](const SkString& familyName) {
        if (familyName.isEmpty())
            return;

        if (!registeredFamilyNames.insert(toLowerAscii(familyName)).second)
            return;

        typefaceFontProvider->registerTypeface(sk_ref_sp(typeface), familyName);
    };

    if (alias != nullptr)
        registerFamilyName(SkString(alias));

    SkString backendFamilyName;
    typeface->getFamilyName(&backendFamilyName);
    registerFamilyName(backendFamilyName);

    sk_sp<SkTypeface::LocalizedStrings> familyNames(typeface->createFamilyNameIterator());

    if (familyNames == nullptr)
        return;

    SkTypeface::LocalizedString familyName;

    while (familyNames->next(&familyName))
        registerFamilyName(familyName.fString);
}

static std::vector<sk_sp<SkTypeface>> createTypefacesFromData(SkData* data) {
    static const sk_sp<SkFontMgr> fontManager = SkFontMgr_New_Custom_Empty();
    static const std::unique_ptr<SkFontScanner> fontScanner = SkFontScanner_Make_FreeType();

    std::vector<sk_sp<SkTypeface>> typefaces;

    SkMemoryStream stream(sk_ref_sp(data));
    int faceCount = 0;

    if (!fontScanner->scanFile(&stream, &faceCount))
        return typefaces;

    for (int index = 0; index < faceCount; index++) {
        auto typeface = fontManager->makeFromData(sk_ref_sp(data), index);

        if (typeface != nullptr)
            typefaces.push_back(std::move(typeface));
    }

    return typefaces;
}

extern "C" {

QUEST_API skia::textlayout::TypefaceFontProvider *questpdf_skia_typeface_font_provider_create() {
    return sk_make_sp<skia::textlayout::TypefaceFontProvider>().release();
}

QUEST_API int questpdf_skia_typeface_font_provider_add_typefaces_from_data(skia::textlayout::TypefaceFontProvider *typefaceFontProvider, SkData* data, char *alias) {
    int registeredTypefaces = 0;

    for (auto& typeface : createTypefacesFromData(data)) {
        registerTypefaceUnderAllFamilyNames(typefaceFontProvider, typeface.get(), alias);
        registeredTypefaces++;
    }

    return registeredTypefaces;
}

QUEST_API SkFontMgr *questpdf_skia_typeface_font_provider_as_font_manager(skia::textlayout::TypefaceFontProvider *typefaceFontProvider) {
    return typefaceFontProvider;
}

QUEST_API void questpdf_skia_typeface_font_provider_unref(skia::textlayout::TypefaceFontProvider *typefaceFontProvider) {
    typefaceFontProvider->unref();
}

}
