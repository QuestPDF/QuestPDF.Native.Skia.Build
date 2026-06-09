#if defined(_WIN32)
    #define QUEST_API __declspec(dllexport)
#else
    #define QUEST_API __attribute__((visibility("default")))
#endif
