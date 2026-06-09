#include "export.h"

#include "include/core/SkStream.h"
#include "include/codec/SkCodec.h"
#include "include/codec/SkPngDecoder.h"
#include "include/codec/SkJpegDecoder.h"
#include "modules/svg/include/SkSVGDOM.h"
#include "modules/svg/include/SkSVGSVG.h"
#include "modules/svg/include/SkSVGRenderContext.h"
#include "modules/skresources/include/SkResources.h"

extern "C" {

QUEST_API skresources::ResourceProvider *questpdf_skia_resource_provider_create(char* resourcesPath, SkFontMgr *fontManager) {
    auto decodeStrategy = skresources::ImageDecodeStrategy::kPreDecode;

    auto fileProvider = skresources::FileResourceProvider::Make(SkString(resourcesPath), decodeStrategy);
    auto dataUriProvider = skresources::DataURIResourceProviderProxy::Make(std::move(fileProvider), decodeStrategy, sk_ref_sp(fontManager));

    return dataUriProvider.release();
}

}