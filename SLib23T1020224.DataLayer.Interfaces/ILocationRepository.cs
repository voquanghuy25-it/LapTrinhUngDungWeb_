using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.DataLayer.Interfaces;

/// <summary>
/// Định nghĩa các phép xử lý dữ liệu trên bảng <c>Locations</c> (vị trí lưu trữ).
/// </summary>
public interface ILocationRepository : ICommonRepository<Location>
{
    /// <summary>
    /// Kiểm tra vị trí lưu trữ có đang được tham chiếu tại bảng <c>BookCopies</c> (vị trí đang có bản sao sách) hay không.
    /// </summary>
    /// <param name="id">Mã vị trí lưu trữ cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được tham chiếu; ngược lại <c>false</c>.</returns>
    Task<bool> InUseAsync(int id);
}
