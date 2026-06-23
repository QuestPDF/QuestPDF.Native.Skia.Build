# configure CMAKE
cmake_minimum_required(VERSION 3.15)
set(CMAKE_CXX_STANDARD 20)
set(CMAKE_MSVC_RUNTIME_LIBRARY "MultiThreaded")


project(QuestPdfSkia)


# include all code source files
file(GLOB_RECURSE SOURCES "src/*.cpp")


# build project as a native shared library
add_library(QuestPdfSkia SHARED ${SOURCES})


# hide all internal symbols by default
set_target_properties(QuestPdfSkia PROPERTIES
    CXX_VISIBILITY_PRESET hidden
    C_VISIBILITY_PRESET hidden
    VISIBILITY_INLINES_HIDDEN ON
)

# disable RTTI; compile with per-function/per-data sections
# so the linker can discard unreferenced code
if (MSVC)
    target_compile_options(QuestPdfSkia PRIVATE /GR- /Gy /Gw)
else()
    target_compile_options(QuestPdfSkia PRIVATE -fno-rtti -ffunction-sections -fdata-sections)
endif()


# include Skia library code
set(SKIA_DIR ${SKIA_DIR})
include_directories(${SKIA_DIR}/)


# MacOS configuration
if(${CMAKE_SYSTEM_NAME} MATCHES "Darwin")
    file(GLOB SKIA_LIBRARY "${SKIA_DIR}/out/release/*.a")
    find_library(COREFOUNDATION_LIBRARY CoreFoundation)
    find_library(COREGRAPHICS_LIBRARY CoreGraphics)
    find_library(CORETEXT_LIBRARY CoreText)
    target_link_libraries(QuestPdfSkia PRIVATE ${SKIA_LIBRARY} ${CORETEXT_LIBRARY} ${COREGRAPHICS_LIBRARY} ${COREFOUNDATION_LIBRARY})

    target_link_options(QuestPdfSkia PRIVATE
        -Wl,-dead_strip
        -Wl,-exported_symbol,_questpdf_skia_*
    )

    # reduce output package size
    add_custom_command(TARGET QuestPdfSkia POST_BUILD COMMAND ${CMAKE_STRIP} -x $<TARGET_FILE:QuestPdfSkia>)
endif()


# Linux configuration
if(${CMAKE_SYSTEM_NAME} MATCHES "Linux")
    file(GLOB SKIA_LIBRARY "${SKIA_DIR}/out/release/*.a")
    list(SORT SKIA_LIBRARY)

    set(THREADS_PREFER_PTHREAD_FLAG TRUE)
    find_package(Threads REQUIRED)
    
    target_link_libraries(QuestPdfSkia PRIVATE
        -Wl,--start-group
        ${SKIA_LIBRARY}
        -Wl,--end-group
        Threads::Threads
    )

    target_link_options(QuestPdfSkia PRIVATE
        -fuse-ld=lld
        -Wl,-z,defs
        -Wl,--no-undefined
        -Wl,--exclude-libs,ALL
        -Wl,--gc-sections
        -Wl,--icf=safe
    )

    # reduce output package size
    add_custom_command(TARGET QuestPdfSkia POST_BUILD COMMAND ${CMAKE_STRIP} -x $<TARGET_FILE:QuestPdfSkia>)
endif()


# Windows configuration
if(${CMAKE_SYSTEM_NAME} MATCHES "Windows")
    file(GLOB SKIA_LIBRARY "${SKIA_DIR}/out/release/*.lib")
    target_link_libraries(QuestPdfSkia PRIVATE ${SKIA_LIBRARY})

    target_link_libraries(QuestPdfSkia PRIVATE dwrite.lib fontsub.lib)
    target_link_options(QuestPdfSkia PRIVATE /OPT:REF /OPT:ICF)
endif()
