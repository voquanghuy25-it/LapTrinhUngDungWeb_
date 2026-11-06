namespace SLib23T1020224.Models.Entities;

/// <summary>
/// Tác giả của sách (bảng <c>Authors</c>).
/// </summary>
public class Author
{
    /// <summary>
    /// Mã tác giả (khóa chính, tự tăng).
    /// </summary>
    public int AuthorId { get; set; }

    /// <summary>
    /// Tên tác giả (tối đa 200 ký tự).
    /// </summary>
    public string AuthorName { get; set; } = string.Empty;

    /// <summary>
    /// Tiểu sử tác giả.
    /// </summary>
    public string? Biography { get; set; }

    /// <summary>
    /// Ngày sinh của tác giả.
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Ngày mất của tác giả (nếu có, phải lớn hơn hoặc bằng ngày sinh).
    /// </summary>
    public DateTime? DeathDate { get; set; }

    /// <summary>
    /// Trạng thái sử dụng: <c>true</c> là đang sử dụng, <c>false</c> là ngưng sử dụng.
    /// </summary>
    public bool IsActive { get; set; }
}
