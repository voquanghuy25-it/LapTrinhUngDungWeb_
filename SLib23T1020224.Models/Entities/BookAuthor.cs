namespace SLib23T1020224.Models.Entities;

/// <summary>
/// Liên kết nhiều-nhiều giữa sách và tác giả (bảng <c>BookAuthors</c>).
/// </summary>
public class BookAuthor
{
    /// <summary>
    /// Mã sách (khóa chính, khóa ngoại tới <see cref="Book"/>).
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Mã tác giả (khóa chính, khóa ngoại tới <see cref="Author"/>).
    /// </summary>
    public int AuthorId { get; set; }

    /// <summary>
    /// Thứ tự của tác giả trong sách (phải lớn hơn 0, mặc định là 1).
    /// </summary>
    public short AuthorOrder { get; set; }

    /// <summary>
    /// Vai trò đóng góp của tác giả (ví dụ: tác giả chính, biên dịch, hiệu đính).
    /// </summary>
    public string? Contribution { get; set; }
}
