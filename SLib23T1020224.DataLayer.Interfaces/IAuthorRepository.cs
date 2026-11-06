using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.DataLayer.Interfaces;

/// <summary>
/// Định nghĩa các phép xử lý dữ liệu trên bảng <c>Authors</c> (tác giả).
/// </summary>
public interface IAuthorRepository : ICommonRepository<Author>
{
    /// <summary>
    /// Kiểm tra tác giả có đang được tham chiếu tại bảng <c>BookAuthors</c> (tác giả đã gắn với sách) hay không.
    /// </summary>
    /// <param name="id">Mã tác giả cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được tham chiếu; ngược lại <c>false</c>.</returns>
    Task<bool> InUseAsync(int id);
}
