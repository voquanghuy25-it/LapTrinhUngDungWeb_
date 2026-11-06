namespace SLib23T1020224.Models.Entities;

/// <summary>
/// Liên kết nhiều-nhiều giữa sách và thể loại (bảng <c>BookCategories</c>).
/// </summary>
public class BookCategory
{
    /// <summary>
    /// Mã sách (khóa chính, khóa ngoại tới <see cref="Book"/>).
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Mã thể loại (khóa chính, khóa ngoại tới <see cref="Category"/>).
    /// </summary>
    public int CategoryId { get; set; }
}
