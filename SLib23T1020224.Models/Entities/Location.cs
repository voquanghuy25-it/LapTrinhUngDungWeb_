namespace SLib23T1020224.Models.Entities;

/// <summary>
/// Vị trí lưu trữ sách trong thư viện (bảng <c>Locations</c>).
/// </summary>
public class Location
{
    /// <summary>
    /// Mã vị trí (khóa chính, tự tăng).
    /// </summary>
    public int LocationId { get; set; }

    /// <summary>
    /// Mã vị trí, duy nhất (tối đa 30 ký tự).
    /// </summary>
    public string LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// Tên vị trí (tối đa 150 ký tự).
    /// </summary>
    public string LocationName { get; set; } = string.Empty;

    /// <summary>
    /// Tầng.
    /// </summary>
    public string? Floor { get; set; }

    /// <summary>
    /// Phòng.
    /// </summary>
    public string? Room { get; set; }

    /// <summary>
    /// Kệ sách.
    /// </summary>
    public string? Shelf { get; set; }

    /// <summary>
    /// Mô tả vị trí.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Trạng thái sử dụng: <c>true</c> là đang sử dụng, <c>false</c> là ngưng sử dụng.
    /// </summary>
    public bool IsActive { get; set; }
}
