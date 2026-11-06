namespace SLib23T1020224.Models.Entities;

/// <summary>
/// Bản sao vật lý của một đầu sách trong thư viện, là đơn vị được cho mượn (bảng <c>BookCopies</c>).
/// </summary>
public class BookCopy
{
    /// <summary>
    /// Mã bản sao sách (khóa chính, tự tăng).
    /// </summary>
    public int BookCopyId { get; set; }

    /// <summary>
    /// Mã đầu sách mà bản sao này thuộc về (khóa ngoại tới <see cref="Book"/>).
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Mã vạch của bản sao, duy nhất trong hệ thống (tối đa 50 ký tự).
    /// </summary>
    public string Barcode { get; set; } = string.Empty;

    /// <summary>
    /// Mã vị trí lưu trữ hiện tại (khóa ngoại tới <see cref="Location"/>).
    /// </summary>
    public int? LocationId { get; set; }

    /// <summary>
    /// Ngày nhập bản sao vào thư viện.
    /// </summary>
    public DateTime? AcquisitionDate { get; set; }

    /// <summary>
    /// Giá nhập của bản sao (nếu có thì phải lớn hơn hoặc bằng 0).
    /// </summary>
    public decimal? AcquisitionPrice { get; set; }

    /// <summary>
    /// Tình trạng vật lý của sách. Giá trị hợp lệ: <c>New</c>, <c>Good</c>, <c>Fair</c>, <c>Damaged</c> (mặc định <c>Good</c>).
    /// </summary>
    public string CopyCondition { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái của bản sao. Giá trị hợp lệ: <c>Available</c>, <c>OnLoan</c>, <c>Reserved</c>, <c>Lost</c>, <c>Withdrawn</c> (mặc định <c>Available</c>).
    /// </summary>
    public string CopyStatus { get; set; } = string.Empty;

    /// <summary>
    /// Ghi chú về bản sao.
    /// </summary>
    public string? Notes { get; set; }
}
