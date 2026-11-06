using System.Text.RegularExpressions;

namespace SLib23T1020224.BusinessLayer;

/// <summary>
/// Các hàm kiểm tra dữ liệu dùng chung cho các lớp nghiệp vụ (Service).
/// Quy ước chung: dữ liệu không hợp lệ thì ném <see cref="Exception"/> kèm thông báo lỗi.
/// </summary>
internal static class ValidationHelper
{
    /// <summary>
    /// Kiểm tra mã (khóa chính) hợp lệ: phải lớn hơn 0.
    /// </summary>
    /// <param name="id">Mã cần kiểm tra.</param>
    /// <param name="objectName">Tên đối tượng, dùng trong thông báo lỗi (ví dụ: "tác giả").</param>
    public static void ValidateId(int id, string objectName)
    {
        if (id <= 0)
            throw new Exception($"Mã {objectName} không hợp lệ.");
    }

    /// <summary>
    /// Kiểm tra và chuẩn hóa một chuỗi bắt buộc nhập: cắt khoảng trắng hai đầu, không được rỗng, không vượt quá độ dài tối đa.
    /// </summary>
    /// <param name="value">Giá trị cần kiểm tra.</param>
    /// <param name="fieldName">Tên trường, dùng trong thông báo lỗi (ví dụ: "Tên tác giả").</param>
    /// <param name="maxLength">Số ký tự tối đa cho phép.</param>
    /// <returns>Chuỗi đã được chuẩn hóa.</returns>
    public static string Required(string? value, string fieldName, int maxLength)
    {
        string text = (value ?? string.Empty).Trim();
        if (text.Length == 0)
            throw new Exception($"{fieldName} không được để trống.");
        if (text.Length > maxLength)
            throw new Exception($"{fieldName} không được vượt quá {maxLength} ký tự.");
        return text;
    }

    /// <summary>
    /// Kiểm tra và chuẩn hóa một chuỗi không bắt buộc: cắt khoảng trắng hai đầu, chuỗi rỗng được đổi thành <c>null</c>.
    /// </summary>
    /// <param name="value">Giá trị cần kiểm tra.</param>
    /// <param name="fieldName">Tên trường, dùng trong thông báo lỗi.</param>
    /// <param name="maxLength">Số ký tự tối đa cho phép.</param>
    /// <returns>Chuỗi đã chuẩn hóa hoặc <c>null</c> nếu không nhập.</returns>
    public static string? Optional(string? value, string fieldName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        string text = value.Trim();
        if (text.Length > maxLength)
            throw new Exception($"{fieldName} không được vượt quá {maxLength} ký tự.");
        return text;
    }

    /// <summary>
    /// Kiểm tra mã (code) chỉ gồm chữ cái, chữ số và các ký tự <c>. _ -</c>.
    /// </summary>
    /// <param name="code">Mã cần kiểm tra.</param>
    /// <param name="fieldName">Tên trường, dùng trong thông báo lỗi.</param>
    public static void ValidateCode(string code, string fieldName)
    {
        if (!Regex.IsMatch(code, @"^[A-Za-z0-9._-]+$"))
            throw new Exception($"{fieldName} chỉ được chứa chữ cái không dấu, chữ số và các ký tự . _ -");
    }

    /// <summary>
    /// Kiểm tra định dạng số điện thoại (6 đến 20 ký tự gồm chữ số, khoảng trắng và các ký tự <c>+ - ( ) .</c>).
    /// </summary>
    /// <param name="phone">Số điện thoại cần kiểm tra.</param>
    public static void ValidatePhone(string phone)
    {
        if (!Regex.IsMatch(phone, @"^[0-9+\-\s().]{6,20}$"))
            throw new Exception("Số điện thoại không hợp lệ.");
    }

    /// <summary>
    /// Kiểm tra định dạng địa chỉ email.
    /// </summary>
    /// <param name="email">Email cần kiểm tra.</param>
    public static void ValidateEmail(string email)
    {
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new Exception("Email không hợp lệ.");
    }

    /// <summary>
    /// Kiểm tra địa chỉ website: phải là đường dẫn đầy đủ bắt đầu bằng http:// hoặc https://.
    /// </summary>
    /// <param name="url">Địa chỉ website cần kiểm tra.</param>
    public static void ValidateWebsite(string url)
    {
        bool ok = Uri.TryCreate(url, UriKind.Absolute, out Uri? uri)
                  && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        if (!ok)
            throw new Exception("Website không hợp lệ (phải bắt đầu bằng http:// hoặc https://).");
    }
}
