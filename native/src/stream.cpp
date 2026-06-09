#include "export.h"
#include "include/core/SkStream.h"


typedef void (*WriteStreamCallback)(const void* data, int size);

class SK_API SkEventWStream final : public SkWStream {
public:
    static constexpr size_t kBufferSize = 8192;

    explicit SkEventWStream(WriteStreamCallback callback)
        : fBytesWritten(0)
        , fCallback(callback)
        , fBufferPos(0) {
        fBuffer = new uint8_t[kBufferSize];
    }

    ~SkEventWStream() override {
        flush();
        delete[] fBuffer;
    }

    SkEventWStream(const SkEventWStream&) = delete;
    SkEventWStream& operator=(const SkEventWStream&) = delete;

    bool write(const void* data, size_t dataSize) override {
        if (!fCallback) return false;

        fBytesWritten += dataSize;

        // if data won't fit in buffer, flush first
        if (fBufferPos > 0 && fBufferPos + dataSize > kBufferSize) {
            flush();
        }

        // if data is too large for buffer, send directly
        if (dataSize > kBufferSize) {
            if (fBufferPos > 0)
                flush();

            fCallback(data, dataSize);
            return true;
        }

        // buffer the data
        const auto src = static_cast<const uint8_t*>(data);
        memcpy(fBuffer + fBufferPos, src, dataSize);
        fBufferPos += dataSize;

        return true;
    }

    void flush() override {
        if (fBufferPos > 0 && fCallback && fBuffer) {
            fCallback(fBuffer, fBufferPos);
            fBufferPos = 0;
        }
    }

    size_t bytesWritten() const override {
        return fBytesWritten;
    }

private:
    size_t fBytesWritten;
    WriteStreamCallback fCallback;
    int fBufferPos;
    uint8_t* fBuffer = nullptr;
};


extern "C" {

QUEST_API SkEventWStream *questpdf_skia_write_stream_create(WriteStreamCallback callback) {
    return new SkEventWStream(callback);
}

QUEST_API void questpdf_skia_write_stream_flush(SkEventWStream *stream) {
    stream->flush();
}

QUEST_API void questpdf_skia_write_stream_delete(SkEventWStream *stream) {
    delete stream;
}

}