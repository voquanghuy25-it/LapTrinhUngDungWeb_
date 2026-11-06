namespace SLib23T1020224.Models.Entities;

/// <summary>
/// Nhà xuất bản (bảng <c>Publishers</c>).
/// </summary>
public class Publisher
{
    /// <summary>
    /// Mã nhà xuất bản (khóa chính, tự tăng).
    /// </summary>
    public int PublisherId { get; set; }

    /// <summary>
    /// Tên nhà xuất bản (tối đa 200 ký tự).
    /// </summary>
    public string PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// Địa chỉ nhà xuất bản.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Số điện thoại.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Địa chỉ email.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Địa chỉ website.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Trạng thái sử dụng: <c>true</c> là đang sử dụng, <c>false</c> là ngưng sử dụng.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Thời điểm tạo bản ghi.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
