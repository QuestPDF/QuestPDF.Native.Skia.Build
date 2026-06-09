#include "export.h"

#include "include/core/SkStream.h"
#include "include/codec/SkCodec.h"
#include "include/codec/SkPngDecoder.h"
#include "include/codec/SkJpegDecoder.h"
#include "modules/svg/include/SkSVGDOM.h"
#include "modules/svg/include/SkSVGSVG.h"
#include "modules/svg/include/SkSVGRenderContext.h"
#include "modules/skresources/include/SkResources.h"
#include "modules/skshaper/utils/FactoryHelpers.h"

extern "C" {

QUEST_API SkSVGDOM *questpdf_skia_svg_create(SkData *data, skresources::ResourceProvider *resourceProvider, SkFontMgr *fontManager) {
    auto svgStream = SkMemoryStream(sk_ref_sp(data));

    return SkSVGDOM::Builder()
        .setFontManager(sk_ref_sp(fontManager))
        .setTextShapingFactory(SkShapers::BestAvailable())
        .setResourceProvider(sk_ref_sp(resourceProvider))
        .make(svgStream)
        .release();
}

QUEST_API void questpdf_skia_svg_unref(SkSVGDOM *svg) {
    svg->unref();
}

enum SkSvgSizeUnit {
    kUnknown,
    kNumber,
    kPercentage,
    kPX,
    kCM,
    kMM,
    kIN,
    kPT,
    kPC,
};

struct SkSvgSize {
    float width;
    float height;

    SkSvgSizeUnit widthUnit;
    SkSvgSizeUnit heightUnit;
};

QUEST_API void questpdf_skia_svg_get_size(SkSVGDOM *svg, SkSvgSize *size, SkRect* viewBox) {
    auto convertUnit = [](SkSVGLength::Unit unit) -> SkSvgSizeUnit {
        switch (unit) {
            case SkSVGLength::Unit::kNumber: return SkSvgSizeUnit::kNumber;
            case SkSVGLength::Unit::kPercentage: return SkSvgSizeUnit::kPercentage;
            case SkSVGLength::Unit::kPX: return SkSvgSizeUnit::kPX;
            case SkSVGLength::Unit::kCM: return SkSvgSizeUnit::kCM;
            case SkSVGLength::Unit::kMM: return SkSvgSizeUnit::kMM;
            case SkSVGLength::Unit::kIN: return SkSvgSizeUnit::kIN;
            case SkSVGLength::Unit::kPT: return SkSvgSizeUnit::kPT;
            case SkSVGLength::Unit::kPC: return SkSvgSizeUnit::kPC;
            default: return SkSvgSizeUnit::kUnknown;
        }
    };

    size->width = svg->getRoot()->getWidth().value();
    size->widthUnit = convertUnit(svg->getRoot()->getWidth().unit());

    size->height = svg->getRoot()->getHeight().value();
    size->heightUnit = convertUnit(svg->getRoot()->getHeight().unit());

    auto inputViewBox = svg->getRoot()->getViewBox();

    if (inputViewBox.has_value())
        viewBox->setLTRB(inputViewBox->left(), inputViewBox->top(), inputViewBox->right(), inputViewBox->bottom());
}

}