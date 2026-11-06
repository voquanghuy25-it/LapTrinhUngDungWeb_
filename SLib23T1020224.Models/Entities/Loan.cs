namespace SLib23T1020224.Models.Entities;

/// <summary>
/// Phiếu mượn sách của độc giả (bảng <c>Loans</c>).
/// </summary>
public class Loan
{
    /// <summary>
    /// Mã phiếu mượn (khóa chính, tự tăng).
    /// </summary>
    public int LoanId { get; set; }

    /// <summary>
    /// Mã phiếu mượn hiển thị, duy nhất (tối đa 30 ký tự).
    /// </summary>
    public string LoanCode { get; set; } = string.Empty;

    /// <summary>
    /// Mã độc giả mượn sách (khóa ngoại tới <see cref="Member"/>).
    /// </summary>
    public int MemberId { get; set; }

    /// <summary>
    /// Mã nhân viên lập phiếu (khóa ngoại tới <see cref="Employee"/>).
    /// </summary>
    public int? EmployeeId { get; set; }

    /// <summary>
    /// Thời điểm lập phiếu mượn.
    /// </summary>
    public DateTime BorrowedAt { get; set; }

    /// <summary>
    /// Trạng thái phiếu mượn. Giá trị hợp lệ: <c>Active</c>, <c>Completed</c>, <c>Cancelled</c> (mặc định <c>Active</c>).
    /// </summary>
    public string LoanStatus { get; set; } = string.Empty;

    /// <summary>
    /// Ghi chú.
    /// </summary>
    public string? Notes { get; set; }
}
