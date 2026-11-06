using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.DataLayer.Interfaces;

/// <summary>
/// Định nghĩa các phép xử lý dữ liệu trên bảng <c>Languages</c> (ngôn ngữ).
/// </summary>
public interface ILanguageRepository : ICommonRepository<Language>
{
    /// <summary>
    /// Kiểm tra ngôn ngữ có đang được tham chiếu tại bảng <c>Books</c> (ngôn ngữ đã có sách) hay không.
    /// </summary>
    /// <param name="id">Mã ngôn ngữ cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được tham chiếu; ngược lại <c>false</c>.</returns>
    Task<bool> InUseAsync(int id);
}

