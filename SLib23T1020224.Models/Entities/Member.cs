namespace SLib23T1020224.Models.Entities;

/// <summary>
/// Độc giả (thành viên) của thư viện (bảng <c>Members</c>).
/// </summary>
public class Member
{
    /// <summary>
    /// Mã độc giả (khóa chính, tự tăng).
    /// </summary>
    public int MemberId { get; set; }

    /// <summary>
    /// Mã thẻ độc giả, duy nhất (tối đa 30 ký tự).
    /// </summary>
    public string MemberCode { get; set; } = string.Empty;

    /// <summary>
    /// Mã loại độc giả (khóa ngoại tới <see cref="MemberType"/>).
    /// </summary>
    public int MemberTypeId { get; set; }

    /// <summary>
    /// Họ và tên độc giả (tối đa 200 ký tự).
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Ngày sinh.
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Giới tính. Giá trị hợp lệ: <c>Male</c>, <c>Female</c>, <c>Other</c>.
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// Địa chỉ email.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Số điện thoại.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Địa chỉ liên hệ.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Cơ quan / đơn vị / trường học của độc giả.
    /// </summary>
    public string? Organization { get; set; }

    /// <summary>
    /// Thời điểm đăng ký thẻ.
    /// </summary>
    public DateTime RegisteredAt { get; set; }

    /// <summary>
    /// Ngày hết hạn thẻ (nếu có thì phải lớn hơn hoặc bằng ngày đăng ký).
    /// </summary>
    public DateTime? ExpiredAt { get; set; }

    /// <summary>
    /// Trạng thái thẻ. Giá trị hợp lệ: <c>Active</c>, <c>Suspended</c>, <c>Expired</c>, <c>Closed</c> (mặc định <c>Active</c>).
    /// </summary>
    public string MemberStatus { get; set; } = string.Empty;

    /// <summary>
    /// Ghi chú.
    /// </summary>
    public string? Notes { get; set; }
}
