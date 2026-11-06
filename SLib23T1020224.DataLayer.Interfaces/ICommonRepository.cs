namespace SLib23T1020224.DataLayer.Interfaces;

/// <summary>
/// Định nghĩa các phép xử lý dữ liệu cơ bản (xem danh sách, lấy một dòng, thêm, sửa, xóa) dùng chung cho các bảng dữ liệu.
/// </summary>
/// <typeparam name="T">Kiểu dữ liệu (Entity) biểu diễn một dòng của bảng cần xử lý.</typeparam>
public interface ICommonRepository<T> where T : class
{
    /// <summary>
    /// Lấy danh sách toàn bộ dữ liệu.
    /// </summary>
    /// <returns>Danh sách dữ liệu.</returns>
    Task<IEnumerable<T>> ListAsync();

    /// <summary>
    /// Lấy thông tin của một dòng dữ liệu theo mã.
    /// </summary>
    /// <param name="id">Mã của dòng dữ liệu cần lấy.</param>
    /// <returns>Dữ liệu tìm được; <c>null</c> nếu không tồn tại.</returns>
    Task<T?> GetAsync(int id);

    /// <summary>
    /// Bổ sung một dòng dữ liệu mới.
    /// </summary>
    /// <param name="data">Dữ liệu cần bổ sung.</param>
    /// <returns>Mã của dòng dữ liệu vừa được bổ sung.</returns>
    Task<int> AddAsync(T data);

    /// <summary>
    /// Cập nhật một dòng dữ liệu.
    /// </summary>
    /// <param name="data">Dữ liệu cần cập nhật (xác định dòng cần sửa thông qua mã).</param>
    /// <returns><c>true</c> nếu cập nhật thành công; ngược lại <c>false</c>.</returns>
    Task<bool> UpdateAsync(T data);

    /// <summary>
    /// Xóa một dòng dữ liệu theo mã.
    /// </summary>
    /// <param name="id">Mã của dòng dữ liệu cần xóa.</param>
    /// <returns><c>true</c> nếu xóa thành công; ngược lại <c>false</c>.</returns>
    Task<bool> DeleteAsync(int id);
}
