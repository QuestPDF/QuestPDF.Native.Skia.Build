#include "export.h"


extern "C" {

QUEST_API int questpdf_get_compatibility_version() {
    return 16;
}

QUEST_API int questpdf_check_compatibility_by_calculating_sum(int a, int b) {
    return a + b;
}

// obsolete
QUEST_API int questpdf_skia_get_questpdf_version() {
    return questpdf_get_compatibility_version();
}

}
