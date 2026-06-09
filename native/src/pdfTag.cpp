#include <iostream>

#include "export.h"

#include <memory>
#include <vector>
#include "include/core/SkCanvas.h"
#include "include/docs/SkPDFDocument.h"

extern "C" {

QUEST_API SkPDF::StructureElementNode* questpdf_skia_pdf_structure_element_create(int nodeId, char* type, char* alt, char* lang) {
    auto element = new SkPDF::StructureElementNode();
    element->fNodeId = nodeId;

    element->fTypeString = type;
    element->fAlt = alt;
    element->fLang = lang;

    return element;
}

QUEST_API void questpdf_skia_pdf_structure_element_set_children(SkPDF::StructureElementNode* parent, SkPDF::StructureElementNode** children, int childCount) {
    parent->fChildVector = std::vector<std::unique_ptr<SkPDF::StructureElementNode>>(children, children + childCount);
}

QUEST_API void questpdf_skia_pdf_structure_element_add_attribute_text(SkPDF::StructureElementNode* node, const char* owner, const char* name, const char* value) {
    node->fAttributes.appendName(owner, name, value);
}

QUEST_API void questpdf_skia_pdf_structure_element_add_attribute_integer(SkPDF::StructureElementNode* node, const char* owner, const char* name, const int value) {
    node->fAttributes.appendInt(owner, name, value);
}

QUEST_API void questpdf_skia_pdf_structure_element_add_attribute_float(SkPDF::StructureElementNode* node, const char* owner, const char* name, const float value) {
    node->fAttributes.appendFloat(owner, name, value);
}

QUEST_API void questpdf_skia_pdf_structure_element_add_attribute_float_array(SkPDF::StructureElementNode* node, const char* owner, const char* name, const float* array, int arrayLength) {
    const auto value = std::vector<float>(array, array + arrayLength);
    node->fAttributes.appendFloatArray(owner, name, value);
}

QUEST_API void questpdf_skia_pdf_structure_element_add_attribute_node_ids(SkPDF::StructureElementNode* node, const char* owner, const char* name, const int* array, int arrayLength) {
    const auto value = std::vector<int>(array, array + arrayLength);
    node->fAttributes.appendNodeIdArray(owner, name, value);
}

QUEST_API void questpdf_skia_pdf_structure_element_delete(const SkPDF::StructureElementNode* element) {
    delete element;
}

}
