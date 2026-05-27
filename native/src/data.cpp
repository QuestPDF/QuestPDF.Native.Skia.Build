#include "export.h"

#include "include/core/SkBitmap.h"
#include "include/core/SkRect.h"
#include "include/core/SkData.h"
#include "include/codec/SkCodec.h"


extern "C" {

QUEST_API SkData *data_create_from_file(const char *path) {
    return SkData::MakeFromFileName(path).release();
}

QUEST_API SkData *data_create_from_binary(const uint8_t *data, int dataLength) {
    return SkData::MakeWithCopy(data, dataLength).release();
}

struct GetBytesFromDataResult {
    int length;
    const uint8_t *bytes;
};

QUEST_API void data_get_bytes(SkData *data, GetBytesFromDataResult* out_result) {
    GetBytesFromDataResult& result = *out_result;
    result.length = data->size();
    result.bytes = data->bytes();
}

QUEST_API void data_unref(SkData *data) {
    data->unref();
}

}