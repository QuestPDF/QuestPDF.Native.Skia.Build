#if defined(_WIN32)
#define QUEST_API questpdf_skia___declspec(dllexport)
#elif defined(__EMSCRIPTEN__)
#include <emscripten.h>
#define QUEST_API EMSCRIPTEN_KEEPALIVE questpdf_skia___attribute__((visibility("default")))
#else
#define QUEST_API
#endif
