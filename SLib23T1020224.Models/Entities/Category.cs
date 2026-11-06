namespace SLib23T1020224.Models.Entities;

/// <summary>
/// Thể loại sách, hỗ trợ cấu trúc phân cấp cha-con (bảng <c>Categories</c>).
/// </summary>
public class Category
{
    /// <summary>
    /// Mã thể loại (khóa chính, tự tăng).
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Tên thể loại (tối đa 150 ký tự).
    /// </summary>
    public string CategoryName { get; set; } = string.Empty;

    /// <summary>
    /// Mã thể loại cha (khóa ngoại tự tham chiếu); <c>null</c> nếu là thể loại gốc, không được trùng với mã của chính nó.
    /// </summary>
    public int? ParentCategoryId { get; set; }

    /// <summary>
    /// Mô tả thể loại.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Trạng thái sử dụng: <c>true</c> là đang sử dụng, <c>false</c> là ngưng sử dụng.
    /// </summary>
    public bool IsActive { get; set; }
}
