#include "export.h"

#include "include/core/SkStream.h"
#include "include/core/SkRect.h"
#include "include/core/SkCanvas.h"
#include "include/svg/SkSVGCanvas.h"

extern "C" {

QUEST_API SkCanvas *svg_create_canvas(const SkRect* bounds, SkWStream *stream) {
    return SkSVGCanvas::Make(*bounds, stream).release();
}

}