using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.DataLayer.Interfaces;

/// <summary>
/// Định nghĩa các phép xử lý dữ liệu trên bảng <c>MemberTypes</c> (loại độc giả).
/// </summary>
public interface IMemberTypeRepository : ICommonRepository<MemberType>
{
    /// <summary>
    /// Kiểm tra loại độc giả có đang được tham chiếu tại bảng <c>Members</c> (loại độc giả đã có độc giả) hay không.
    /// </summary>
    /// <param name="id">Mã loại độc giả cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được tham chiếu; ngược lại <c>false</c>.</returns>
    Task<bool> InUseAsync(int id);
}
