#include "export.h"

#include <iostream>
#include "include/core/SkBitmap.h"
#include "include/core/SkCanvas.h"
#include "include/core/SkPaint.h"
#include "include/core/SkRect.h"
#include "include/core/SkStream.h"
#include "include/core/SkCanvas.h"
#include "include/core/SkString.h"
#include "include/codec/SkCodec.h"
#include "include/docs/SkPDFDocument.h"
#include "include/docs/SkPDFJpegHelpers.h"

extern "C" {

struct DateTime {
    int16_t timeZoneMinutes;
    uint16_t year;
    uint8_t month;
    uint8_t dayOfWeek;
    uint8_t day;
    uint8_t hour;
    uint8_t minute;
    uint8_t second;
};

struct PdfDocumentMetadata {
    char *title;
    char *author;
    char *subject;
    char *keywords;
    char *creator;
    char *producer;
    char *flang;

    DateTime creationDate;
    DateTime modificationDate;

    SkPDF::Metadata::PDFA_Conformance PDFAConformance;
    SkPDF::Metadata::PDFUA_Conformance PDFUAConformance;

    bool compressDocument;
    SkScalar rasterDPI;

    SkPDF::StructureElementNode* semanticNodeRoot;
};

}

SkPDF::DateTime map_datetime(DateTime datetime) {
    SkPDF::DateTime result;

    result.fTimeZoneMinutes = datetime.timeZoneMinutes;
    result.fYear = datetime.year;
    result.fMonth = datetime.month;
    result.fDayOfWeek = datetime.dayOfWeek;
    result.fDay = datetime.day;
    result.fHour = datetime.hour;
    result.fMinute = datetime.minute;
    result.fSecond = datetime.second;

    return result;
}

SkPDF::Metadata map_pdf_metadata(PdfDocumentMetadata metadata) {
    SkPDF::Metadata result;

    result.fTitle = metadata.title;
    result.fAuthor = metadata.author;
    result.fSubject = metadata.subject;
    result.fKeywords = metadata.keywords;
    result.fCreator = metadata.creator;
    result.fProducer = metadata.producer;
    result.fLang = metadata.flang;

    result.fCreation = map_datetime(metadata.creationDate);
    result.fModified = map_datetime(metadata.modificationDate);

    result.fPDFAConformance = metadata.PDFAConformance;
    result.fPDFUAConformance = metadata.PDFUAConformance;
    result.fCompressionLevel = metadata.compressDocument ? SkPDF::Metadata::CompressionLevel::LowButFast : SkPDF::Metadata::CompressionLevel::None;
    result.fRasterDPI = metadata.rasterDPI;
    result.fEncodingQuality = 101;

    result.jpegDecoder = SkPDF::JPEG::Decode;
    result.jpegEncoder = SkPDF::JPEG::Encode;

    result.fStructureElementTreeRoot = metadata.semanticNodeRoot;
    result.fOutline = SkPDF::Metadata::Outline::StructureElementHeaders;

    return result;
}

extern "C" {

QUEST_API SkDocument *pdf_document_create(SkDynamicMemoryWStream *stream, const PdfDocumentMetadata* metadata) {
    return SkPDF::MakeDocument(stream, map_pdf_metadata(*metadata)).release();
}

}