#include "export.h"

#include <iostream>
#include "include/core/SkBitmap.h"
#include "include/core/SkCanvas.h"
#include "include/core/SkStream.h"
#include "include/codec/SkCodec.h"
#include "include/docs/SkPDFDocument.h"

extern "C" {

QUEST_API SkCanvas *questpdf_skia_document_begin_page(SkDocument *document, float width, float height) {
    return document->beginPage(width, height);
}

QUEST_API void questpdf_skia_document_end_page(SkDocument *document) {
    document->endPage();
}

QUEST_API void questpdf_skia_document_close(SkDocument *document) {
    document->close();
}

QUEST_API  void questpdf_skia_document_unref(SkDocument *document) {
    document->unref();
}

}