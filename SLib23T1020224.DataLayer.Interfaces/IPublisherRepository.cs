using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.DataLayer.Interfaces;

/// <summary>
/// Định nghĩa các phép xử lý dữ liệu trên bảng <c>Publishers</c> (nhà xuất bản).
/// </summary>
public interface IPublisherRepository : ICommonRepository<Publisher>
{
    /// <summary>
    /// Kiểm tra nhà xuất bản có đang được tham chiếu tại bảng <c>Books</c> (nhà xuất bản đã có sách) hay không.
    /// </summary>
    /// <param name="id">Mã nhà xuất bản cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được tham chiếu; ngược lại <c>false</c>.</returns>
    Task<bool> InUseAsync(int id);
}
