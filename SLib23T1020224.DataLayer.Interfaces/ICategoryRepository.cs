using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.DataLayer.Interfaces;

/// <summary>
/// Định nghĩa các phép xử lý dữ liệu trên bảng <c>Categories</c> (thể loại).
/// </summary>
public interface ICategoryRepository : ICommonRepository<Category>
{
    /// <summary>
    /// Kiểm tra thể loại có đang được tham chiếu tại bảng <c>BookCategories</c> (thể loại đã gắn với sách) và chính bảng <c>Categories</c> (thể loại đang là cha của thể loại khác) hay không.
    /// </summary>
    /// <param name="id">Mã thể loại cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được tham chiếu; ngược lại <c>false</c>.</returns>
    Task<bool> InUseAsync(int id);
}

