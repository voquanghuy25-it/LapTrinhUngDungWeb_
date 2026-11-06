namespace SLib23T1020224.Models.Entities;

/// <summary>
/// Nhân viên (thủ thư/quản trị) sử dụng hệ thống (bảng <c>Employees</c>).
/// </summary>
public class Employee
{
    /// <summary>
    /// Mã nhân viên (khóa chính, tự tăng).
    /// </summary>
    public int EmployeeId { get; set; }

    /// <summary>
    /// Tên đệm và tên của nhân viên.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Họ của nhân viên.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Ngày sinh.
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Địa chỉ email của nhân viên (tối đa 50 ký tự).
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Số điện thoại.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Địa chỉ liên hệ.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Mật khẩu đăng nhập. Nên lưu dạng đã băm (hash), không lưu mật khẩu thô.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Trạng thái làm việc: <c>true</c> là đang làm việc, <c>false</c> là đã nghỉ.
    /// </summary>
    public bool IsWorking { get; set; }

    /// <summary>
    /// Vai trò / phân quyền của nhân viên trong hệ thống.
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// Tên đăng nhập.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Ngày bắt đầu làm việc.
    /// </summary>
    public DateTime? HireDate { get; set; }
}
