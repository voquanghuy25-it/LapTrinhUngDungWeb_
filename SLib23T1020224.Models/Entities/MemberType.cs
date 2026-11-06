namespace SLib23T1020224.Models.Entities;

/// <summary>
/// Loại độc giả và quy định mượn sách tương ứng (bảng <c>MemberTypes</c>).
/// </summary>
public class MemberType
{
    /// <summary>
    /// Mã loại độc giả (khóa chính, tự tăng).
    /// </summary>
    public int MemberTypeId { get; set; }

    /// <summary>
    /// Tên loại độc giả, duy nhất (tối đa 100 ký tự).
    /// </summary>
    public string TypeName { get; set; } = string.Empty;

    /// <summary>
    /// Số sách tối đa được mượn cùng lúc (lớn hơn hoặc bằng 0, mặc định 3).
    /// </summary>
    public int MaxActiveLoans { get; set; }

    /// <summary>
    /// Số ngày được mượn cho mỗi lượt (lớn hơn 0, mặc định 14).
    /// </summary>
    public int LoanPeriodDays { get; set; }

    /// <summary>
    /// Số lần gia hạn tối đa (lớn hơn hoặc bằng 0, mặc định 0).
    /// </summary>
    public int MaxRenewals { get; set; }

    /// <summary>
    /// Trạng thái sử dụng: <c>true</c> là đang sử dụng, <c>false</c> là ngưng sử dụng.
    /// </summary>
    public bool IsActive { get; set; }
}
