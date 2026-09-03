#include "../export.h"

#include <algorithm>
#include <cstring>
#include <vector>

#include "include/core/SkFontStyle.h"
#include "include/core/SkString.h"
#include "include/core/SkTypeface.h"
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

struct SkFontInfo {
    char familyName[256]; // UTF-8, NUL-terminated
    char postScriptName[256]; // UTF-8, NUL-terminated
    int weight;
    bool isItalic;
    bool isVariable;
};

static void copyString(char *destination, size_t destinationSize, const SkString &source) {
    const size_t length = std::min(source.size(), destinationSize - 1);
    memcpy(destination, source.c_str(), length);
    destination[length] = '\0';
}

static SkFontInfo mapTypefaceToFontInfo(const SkString &familyName, SkTypeface *typeface) {
    SkString postScriptName;
    typeface->getPostScriptName(&postScriptName);

    const SkFontStyle fontStyle = typeface->fontStyle();

    SkFontInfo description{};
    copyString(description.familyName, sizeof(description.familyName), familyName);
    copyString(description.postScriptName, sizeof(description.postScriptName), postScriptName);
    description.weight = fontStyle.weight();
    description.isItalic = fontStyle.slant() != SkFontStyle::kUpright_Slant;
    description.isVariable = typeface->getVariationDesignParameters({}) > 0;

    return description;
}

static std::vector<SkFontInfo> getTypefaces(SkFontMgr *fontManager) {
    std::vector<SkFontInfo> descriptions;

    const int familyCount = fontManager->countFamilies();

    for (int familyIndex = 0; familyIndex < familyCount; familyIndex++) {
        SkString familyName;
        fontManager->getFamilyName(familyIndex, &familyName);

        sk_sp<SkFontStyleSet> styleSet = fontManager->createStyleSet(familyIndex);

        if (styleSet == nullptr)
            continue;

        const int styleCount = styleSet->count();

        for (int styleIndex = 0; styleIndex < styleCount; styleIndex++) {
            sk_sp<SkTypeface> typeface = styleSet->createTypeface(styleIndex);

            if (typeface != nullptr)
                descriptions.push_back(mapTypefaceToFontInfo(familyName, typeface.get()));
        }
    }

    return descriptions;
}

static void copyToOutputArray(const std::vector<SkFontInfo> &descriptions, SkFontInfo **array, int *arrayLength) {
    *arrayLength = descriptions.size();
    *array = new SkFontInfo[*arrayLength];
    std::copy(descriptions.begin(), descriptions.end(), *array);
}

extern "C" {

QUEST_API SkFontMgr *questpdf_skia_font_manager_create_global() {
    return font_manager_create_default_internal().release();
}

QUEST_API void questpdf_skia_font_manager_get_typefaces(SkFontMgr *fontManager, SkFontInfo **array, int *arrayLength) {
    const auto descriptions = getTypefaces(fontManager);
    copyToOutputArray(descriptions, array, arrayLength);
}

QUEST_API void questpdf_skia_font_manager_delete_typefaces(SkFontInfo *array) {
    delete[] array;
}

}
