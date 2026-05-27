#include "../export.h"

#include "modules/skparagraph/include/TextStyle.h"

extern "C" {

struct TextStyleFontFeature {
    char* name;
    int value;
};

struct TextStyleConfiguration {
    static const int FONT_FAMILIES_LENGTH = 16;

    SkScalar fontSize;
    SkFontStyle::Weight fontWeight;
    bool isItalic;

    char* fontFamilies[FONT_FAMILIES_LENGTH];
    TextStyleFontFeature fontFeatures[FONT_FAMILIES_LENGTH];

    SkColor foregroundColor;
    SkColor backgroundColor;

    SkColor decorationColor;
    SkScalar decorationThickness;
    skia::textlayout::TextDecoration decorationType;
    skia::textlayout::TextDecorationMode decorationMode;
    skia::textlayout::TextDecorationStyle decorationStyle;

    SkScalar lineHeight;
    SkScalar letterSpacing;
    SkScalar wordSpacing;
    SkScalar baselineOffset;
};

QUEST_API skia::textlayout::TextStyle *text_style_create(const TextStyleConfiguration* configurationPtr) {
    const TextStyleConfiguration& configuration = *configurationPtr;

    auto textStyle = new skia::textlayout::TextStyle();

    textStyle->setFontSize(configuration.fontSize);

    SkFontStyle fontStyle(configuration.fontWeight, SkFontStyle::Width::kNormal_Width, configuration.isItalic ? SkFontStyle::Slant::kItalic_Slant : SkFontStyle::Slant::kUpright_Slant);
    textStyle->setFontStyle(fontStyle);

    // set font families
    std::vector<SkString> fontFamilies;

    for (auto & fontFamily : configuration.fontFamilies) {
        if (fontFamily == nullptr)
            continue;

        fontFamilies.emplace_back(fontFamily);
    }

    textStyle->setFontFamilies(fontFamilies);
    // end font families

    if (configuration.foregroundColor != 0) {
        SkPaint paint;
        paint.setColor(configuration.foregroundColor);
        textStyle->setForegroundPaint(paint);
    }

    if (configuration.backgroundColor != 0) {
        SkPaint paint;
        paint.setColor(configuration.backgroundColor);
        textStyle->setBackgroundPaint(paint);
    }

    textStyle->setDecorationColor(configuration.decorationColor);
    textStyle->setDecoration(configuration.decorationType);
    textStyle->setDecorationMode(configuration.decorationMode);
    textStyle->setDecorationStyle(configuration.decorationStyle);
    textStyle->setDecorationThicknessMultiplier(configuration.decorationThickness);

    // when lineHeight is 0, use the default line height from the font metrics
    if (configuration.lineHeight != 0) {
        textStyle->setHeight(configuration.lineHeight);
        textStyle->setHeightOverride(true);
    }

    textStyle->setHalfLeading(true);
    textStyle->setLetterSpacing(configuration.letterSpacing);
    textStyle->setWordSpacing(configuration.wordSpacing);
    textStyle->setBaselineShift(configuration.baselineOffset);

    textStyle->setFontHinting(SkFontHinting::kNone);

    for (auto & fontFeature : configuration.fontFeatures) {
        if (fontFeature.name == nullptr)
            continue;

        textStyle->addFontFeature(SkString(fontFeature.name), fontFeature.value);
    }

    return textStyle;
}

QUEST_API void text_style_delete(skia::textlayout::TextStyle *textStyle) {
    delete textStyle;
}

}
