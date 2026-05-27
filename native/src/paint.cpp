#include <vector>
#include "include/core/SkPathEffect.h"
#include "include/effects/SkDashPathEffect.h"
#include "include/effects/SkGradient.h"
#include "include/core/SkTileMode.h"
#include "include/core/SkPaint.h"

#include "export.h"

extern "C" {

    QUEST_API SkPaint *paint_create() {
        const auto paint = new SkPaint();
        paint->setAntiAlias(true);
        return paint;
    }

    QUEST_API void paint_delete(const SkPaint* paint) {
        delete paint;
    }

    QUEST_API void paint_set_solid_color(SkPaint* paint, SkColor color) {
        paint->setColor(color);
    }

    QUEST_API void paint_set_linear_gradient(SkPaint* paint, const SkPoint* start, const SkPoint* end, int colorsLength, SkColor* colors) {
        // SkGradient only accepts SkColor4f
        std::vector<SkColor4f> colors4f(colorsLength);

        for (int i = 0; i < colorsLength; i++)
            colors4f[i] = SkColor4f::FromColor(colors[i]);

        // define gradient
        const SkPoint points[2] = { *start, *end };
        const auto gradient = SkGradient({SkSpan(colors4f.data(), colorsLength), {}, SkTileMode::kClamp}, { });
        const auto shader = SkShaders::LinearGradient(points, gradient);

        paint->setShader(shader);
    }

    QUEST_API void paint_set_stroke(SkPaint* paint, const float thickness) {
        paint->setStroke(true);
        paint->setStrokeWidth(thickness);
    }

    QUEST_API void paint_set_dashed_path_effect(SkPaint* paint, const int length, const SkScalar* intervals) {
        const auto intervalsSpan = SkSpan(intervals, length);
        const auto effect = SkDashPathEffect::Make(intervalsSpan, 0);
        paint->setPathEffect(effect);
    }
}