namespace SLib23T1020224.Models.Entities;

/// <summary>
/// Đầu sách (thông tin thư mục) của thư viện (bảng <c>Books</c>).
/// </summary>
public class Book
{
    /// <summary>
    /// Mã đầu sách (khóa chính, tự tăng).
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Mã số sách tiêu chuẩn quốc tế ISBN.
    /// </summary>
    public string? ISBN { get; set; }

    /// <summary>
    /// Nhan đề chính của sách (tối đa 300 ký tự).
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Phụ đề của sách.
    /// </summary>
    public string? Subtitle { get; set; }

    /// <summary>
    /// Mã nhà xuất bản (khóa ngoại tới <see cref="Publisher"/>).
    /// </summary>
    public int? PublisherId { get; set; }

    /// <summary>
    /// Mã ngôn ngữ của sách (khóa ngoại tới <see cref="Language"/>).
    /// </summary>
    public int? LanguageId { get; set; }

    /// <summary>
    /// Năm xuất bản (từ 1000 đến 9999).
    /// </summary>
    public short? PublicationYear { get; set; }

    /// <summary>
    /// Lần xuất bản / phiên bản.
    /// </summary>
    public string? Edition { get; set; }

    /// <summary>
    /// Mô tả, tóm tắt nội dung sách.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Số trang của sách (nếu có thì phải lớn hơn 0).
    /// </summary>
    public int? PageCount { get; set; }

    /// <summary>
    /// Đường dẫn ảnh bìa sách.
    /// </summary>
    public string? CoverImagePath { get; set; }

    /// <summary>
    /// Trạng thái sử dụng: <c>true</c> là đang sử dụng, <c>false</c> là ngưng sử dụng.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Thời điểm tạo bản ghi.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Thời điểm cập nhật bản ghi gần nhất.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
