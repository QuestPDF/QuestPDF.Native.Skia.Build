# configure CMAKE
cmake_minimum_required(VERSION 3.15)
set(CMAKE_CXX_STANDARD 20)
set(CMAKE_MSVC_RUNTIME_LIBRARY "MultiThreaded")


project(QuestPdfSkia)


# enable IPO to achieve better performance, reduce artifact size
include(CheckIPOSupported)
check_ipo_supported(RESULT IPO_IS_SUPPORTED)

if(IPO_IS_SUPPORTED)
    set(CMAKE_INTERPROCEDURAL_OPTIMIZATION TRUE)
endif()


# include all code source files
file(GLOB_RECURSE SOURCES "src/*.cpp")


# select between running as application (main.cpp) or producing the library
add_library(QuestPdfSkia SHARED ${SOURCES})
#add_executable(QuestPdfSkia ${SOURCES})


# built with -fno-rtti
if (MSVC)
    target_compile_options(QuestPdfSkia PRIVATE /GR-)
else()
    target_compile_options(QuestPdfSkia PRIVATE -fno-rtti)
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

    # reduce output package size
    add_custom_command(TARGET QuestPdfSkia POST_BUILD COMMAND ${CMAKE_STRIP} -x $<TARGET_FILE:QuestPdfSkia>)
endif()


# Linux configuration
if(${CMAKE_SYSTEM_NAME} MATCHES "Linux")
    file(GLOB SKIA_LIBRARY "${SKIA_DIR}/out/release/*.a")
    list(SORT SKIA_LIBRARY)

    target_link_libraries(QuestPdfSkia PRIVATE
        -Wl,--start-group
        ${SKIA_LIBRARY}
        -Wl,--end-group
    )

    target_link_options(QuestPdfSkia PRIVATE
        -Wl,-z,defs
        -Wl,--no-undefined
    )

    # reduce output package size
    add_custom_command(TARGET QuestPdfSkia POST_BUILD COMMAND ${CMAKE_STRIP} -x $<TARGET_FILE:QuestPdfSkia>)
endif()


# Windows configuration
if(${CMAKE_SYSTEM_NAME} MATCHES "Windows")
    if(CMAKE_GENERATOR_PLATFORM MATCHES "Win32")
        set(WIN_ARCH "x86")
    else()
        set(WIN_ARCH "x64")
    endif()

    file(GLOB SKIA_LIBRARY "${SKIA_DIR}/out/release/*.lib")
    target_link_libraries(QuestPdfSkia PRIVATE ${SKIA_LIBRARY})

    set(WINDOWS_SDK_PATH "C:/Program Files (x86)/Windows Kits/10/Lib/10.0.26100.0/um/${WIN_ARCH}")
    target_link_libraries(QuestPdfSkia PRIVATE "${WINDOWS_SDK_PATH}/fontsub.lib")
    target_link_libraries(QuestPdfSkia PRIVATE "${WINDOWS_SDK_PATH}/dwrite.lib")
endif()


# WebAssembly configuration
if(${CMAKE_SYSTEM_NAME} MATCHES "Emscripten")
    set_target_properties(QuestPdfSkia PROPERTIES
        OUTPUT_NAME "libQuestPdfSkia"
        SUFFIX ".wasm"
        LINK_FLAGS "-Os -s SIDE_MODULE=1 -s STANDALONE_WASM --no-entry"
    )

    file(GLOB SKIA_LIBRARY "${SKIA_DIR}/out/release/*.a")
    target_link_libraries(QuestPdfSkia PRIVATE ${SKIA_LIBRARY})
endif()
