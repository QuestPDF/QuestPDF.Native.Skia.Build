#include "export.h"

#include "include/core/SkPicture.h"
#include "include/core/SkPictureRecorder.h"

extern "C" {

QUEST_API SkPictureRecorder *questpdf_skia_picture_recorder_create() {
    return new SkPictureRecorder();
}

QUEST_API SkCanvas *questpdf_skia_picture_recorder_begin_recording(SkPictureRecorder *pictureRecorder, float width, float height) {
    return pictureRecorder->beginRecording(width, height);
}

QUEST_API SkPicture *questpdf_skia_picture_recorder_end_recording(SkPictureRecorder *pictureRecorder) {
    return pictureRecorder->finishRecordingAsPicture().release();
}

QUEST_API void questpdf_skia_picture_recorder_delete(SkPictureRecorder *pictureRecorder) {
    delete pictureRecorder;
}

}