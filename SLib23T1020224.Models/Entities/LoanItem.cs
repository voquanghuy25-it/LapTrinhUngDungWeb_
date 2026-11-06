namespace SLib23T1020224.Models.Entities;

/// <summary>
/// Chi tiết phiếu mượn: mỗi dòng ứng với một bản sao sách được mượn (bảng <c>LoanItems</c>).
/// </summary>
public class LoanItem
{
    /// <summary>
    /// Mã chi tiết phiếu mượn (khóa chính, tự tăng).
    /// </summary>
    public int LoanItemId { get; set; }

    /// <summary>
    /// Mã phiếu mượn (khóa ngoại tới <see cref="Loan"/>).
    /// </summary>
    public int LoanId { get; set; }

    /// <summary>
    /// Mã bản sao sách được mượn (khóa ngoại tới <see cref="BookCopy"/>).
    /// </summary>
    public int BookCopyId { get; set; }

    /// <summary>
    /// Thời hạn phải trả sách.
    /// </summary>
    public DateTime DueAt { get; set; }

    /// <summary>
    /// Thời điểm trả sách thực tế; <c>null</c> khi chưa trả, bắt buộc có giá trị khi trạng thái là <c>Returned</c> hoặc <c>Lost</c>.
    /// </summary>
    public DateTime? ReturnedAt { get; set; }

    /// <summary>
    /// Trạng thái dòng mượn. Giá trị hợp lệ: <c>Borrowed</c>, <c>Returned</c>, <c>Lost</c> (mặc định <c>Borrowed</c>).
    /// </summary>
    public string ItemStatus { get; set; } = string.Empty;

    /// <summary>
    /// Tình trạng sách khi trả. Giá trị hợp lệ: <c>New</c>, <c>Good</c>, <c>Fair</c>, <c>Damaged</c>.
    /// </summary>
    public string? ReturnCondition { get; set; }

    /// <summary>
    /// Ghi chú.
    /// </summary>
    public string? Notes { get; set; }
}
