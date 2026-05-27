#include "export.h"


extern "C" {

QUEST_API int get_questpdf_version() {
    return 15;
}

QUEST_API int check_compatibility_by_calculating_sum(int a, int b) {
    return a + b;
}

}
