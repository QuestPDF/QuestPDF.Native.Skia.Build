#include "../export.h"

#include "modules/skparagraph/include/ParagraphStyle.h"
#include "modules/skparagraph/include/ParagraphBuilder.h"
#include "modules/skparagraph/include/TextStyle.h"

extern "C" {

QUEST_API void questpdf_skia_paragraph_plan_layout(skia::textlayout::Paragraph *paragraph, float availableWidth) {
    paragraph->layout(availableWidth);
}

QUEST_API void questpdf_skia_paragraph_get_size(skia::textlayout::Paragraph *paragraph, float* totalWidth, float* totalHeight) {
    *totalWidth = paragraph->getLongestLine();
    *totalHeight = paragraph->getHeight();
}

struct LineExtent {
    float top;
    float bottom;
};

QUEST_API void questpdf_skia_paragraph_get_line_extents(skia::textlayout::Paragraph *paragraph, LineExtent **array, int *arrayLength) {
    std::vector<skia::textlayout::LineMetrics> lineMetrics;
    paragraph->getLineMetrics(lineMetrics);

    *arrayLength = lineMetrics.size();
    *array = new LineExtent[*arrayLength];

    for (int i = 0; i < *arrayLength; ++i) {
        const auto& metrics = lineMetrics[i];
        (*array)[i].top    = static_cast<float>(metrics.fBaseline - metrics.fAscent);
        (*array)[i].bottom = static_cast<float>(metrics.fBaseline + metrics.fDescent);
    }
}

QUEST_API void questpdf_skia_paragraph_delete_line_extents(LineExtent *array) {
    delete[] array;
}

QUEST_API void questpdf_skia_paragraph_get_unresolved_codepoints(skia::textlayout::Paragraph *paragraph, SkUnichar **outputArray, int *outputArrayLength) {
    const auto codepoints = paragraph->unresolvedCodepoints();

    *outputArrayLength = codepoints.size();
    *outputArray = new int[*outputArrayLength];

    int index = 0;

    for (const auto &codepoint: codepoints)
        (*outputArray)[index++] = codepoint;
}

QUEST_API void questpdf_skia_paragraph_delete_unresolved_codepoints(SkUnichar *array) {
    delete[] array;
}

QUEST_API void questpdf_skia_paragraph_get_placeholder_positions(skia::textlayout::Paragraph *paragraph, SkRect **outputArray, int *outputArrayLength) {
    const auto placeholders = paragraph->getRectsForPlaceholders();

    *outputArrayLength = placeholders.size();
    *outputArray = new SkRect[*outputArrayLength];

    for (int i = 0; i < *outputArrayLength; ++i)
        (*outputArray)[i] = placeholders[i].rect;
}

QUEST_API void questpdf_skia_paragraph_delete_positions(SkRect *array) {
    delete[] array;
}

QUEST_API void questpdf_skia_paragraph_get_text_range_positions(skia::textlayout::Paragraph *paragraph, int rangeStart, int rangeEnd, SkRect **outputArray, int *outputArrayLength) {
    const auto placeholders = paragraph->getRectsForRange(rangeStart, rangeEnd, skia::textlayout::RectHeightStyle::kTight, skia::textlayout::RectWidthStyle::kTight);

    *outputArrayLength = placeholders.size();
    *outputArray = new SkRect[*outputArrayLength];

    for (int i = 0; i < *outputArrayLength; ++i)
        (*outputArray)[i] = placeholders[i].rect;
}

QUEST_API void questpdf_skia_paragraph_delete(skia::textlayout::Paragraph *paragraph) {
    delete paragraph;
}

}