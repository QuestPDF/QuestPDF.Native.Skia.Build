#include "export.h"


extern "C" {

QUEST_API int questpdf_skia_get_compatibility_version() {
    return 27;
}

QUEST_API int questpdf_get_compatibility_version() {
    return questpdf_skia_get_compatibility_version();
}

}
