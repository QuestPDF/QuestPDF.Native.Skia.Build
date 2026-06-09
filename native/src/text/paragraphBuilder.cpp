#include "../export.h"

#include "modules/skparagraph/include/ParagraphStyle.h"
#include "modules/skparagraph/include/ParagraphBuilder.h"
#include "modules/skparagraph/include/TextStyle.h"
#include "modules/skunicode/include/SkUnicode.h"

extern "C" {

struct ParagraphStyleConfiguration {
    skia::textlayout::TextAlign alignment;
    skia::textlayout::TextDirection direction;
    int maxLinesVisible;
    char* lineClampEllipsis;
};

QUEST_API skia::textlayout::ParagraphBuilder *questpdf_skia_paragraph_builder_create(const ParagraphStyleConfiguration* configurationPtr, SkUnicode *unicode, skia::textlayout::FontCollection *fontCollection) {
    const ParagraphStyleConfiguration& configuration = *configurationPtr;

    skia::textlayout::ParagraphStyle paragraphStyle = skia::textlayout::ParagraphStyle();
    paragraphStyle.setTextAlign(configuration.alignment);
    paragraphStyle.setTextDirection(configuration.direction);
    paragraphStyle.setReplaceTabCharacters(true);
    paragraphStyle.turnHintingOff();
    paragraphStyle.setApplyRoundingHack(false);

    if (configuration.maxLinesVisible > 0) {
        paragraphStyle.setEllipsis(SkString(configuration.lineClampEllipsis));
        paragraphStyle.setMaxLines(configuration.maxLinesVisible);
    } else {
        const int infiniteLines = std::numeric_limits<int>::max();
        paragraphStyle.setMaxLines(infiniteLines);
    }

    auto paragraphBuilder = skia::textlayout::ParagraphBuilder::make(paragraphStyle, sk_ref_sp(fontCollection), sk_ref_sp(unicode));
    return paragraphBuilder.release();
}

QUEST_API void questpdf_skia_paragraph_builder_add_text(skia::textlayout::ParagraphBuilder *paragraphBuilder, char *text, skia::textlayout::TextStyle *style) {
    paragraphBuilder->pushStyle(*style);
    paragraphBuilder->addText(text);
    paragraphBuilder->pop();
}

QUEST_API void questpdf_skia_paragraph_builder_add_placeholder(skia::textlayout::ParagraphBuilder *paragraphBuilder, const skia::textlayout::PlaceholderStyle* style) {
    paragraphBuilder->addPlaceholder(*style);
}

QUEST_API skia::textlayout::Paragraph *questpdf_skia_paragraph_builder_create_paragraph(skia::textlayout::ParagraphBuilder *paragraphBuilder) {
    auto paragraph = paragraphBuilder->Build();
    return paragraph.release();
}

QUEST_API void questpdf_skia_paragraph_builder_reset(skia::textlayout::ParagraphBuilder *paragraphBuilder) {
    paragraphBuilder->Reset();
}

QUEST_API void questpdf_skia_paragraph_builder_delete(skia::textlayout::ParagraphBuilder *paragraphBuilder) {
    delete paragraphBuilder;
}

}  