namespace QuestPDF.Skia.Text;

/// <summary>
/// Describes a typeface available in a font manager or a typeface provider.
/// </summary>
/// <param name="FamilyName">Family name under which the typeface can be requested. A typeface available under several names is described once per name.</param>
/// <param name="PostScriptName">PostScript name of the face, e.g. "Lato-Light". Identifies the face and appears as the font name in the PDF output.</param>
/// <param name="Weight">Weight from 100 (thin) to 900 (black); 400 is normal, 700 is bold. For variable fonts, the weight of the default instance.</param>
/// <param name="IsItalic">True for italic and oblique typefaces.</param>
/// <param name="IsVariable">True for variable fonts, whose weight (and possibly other properties) can vary along their axes.</param>
internal sealed record FontInfo(
    string FamilyName, 
    string PostScriptName, 
    int Weight, 
    bool IsItalic, 
    bool IsVariable);
