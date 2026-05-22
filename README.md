This project integrates [Skia](https://skia.org) and [QuestPDF](https://www.questpdf.com). It consists of two layers:
- Native unmanaged code - C++ wrapper for Skia that exposes and/or composes Skia APIs necessary for QuestPDF,
- Managed dotnet code - low-level wrapper using P/Invoke calls to access native code.

---

Recommended actions to build locally for MacOS:

```sh
git config --global merge.conflictstyle zdiff3
git clone https://github.com/QuestPDF/QuestPDF.Native.Skia skia --filter=blob:none
cd skia
bash build_local_macos.sh
```
