namespace SLib23T1020224.Models.Entities;

/// <summary>
/// Ngôn ngữ của sách (bảng <c>Languages</c>).
/// </summary>
public class Language
{
    /// <summary>
    /// Mã ngôn ngữ (khóa chính, tự tăng).
    /// </summary>
    public int LanguageId { get; set; }

    /// <summary>
    /// Mã ngôn ngữ, duy nhất (ví dụ: <c>vi</c>, <c>en</c>).
    /// </summary>
    public string LanguageCode { get; set; } = string.Empty;

    /// <summary>
    /// Tên ngôn ngữ (tối đa 100 ký tự).
    /// </summary>
    public string LanguageName { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái sử dụng: <c>true</c> là đang sử dụng, <c>false</c> là ngưng sử dụng.
    /// </summary>
    public bool IsActive { get; set; }
}
