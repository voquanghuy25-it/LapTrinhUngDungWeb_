using SLib23T1020224.DataLayer.Interfaces;
using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.BusinessLayer;

/// <summary>
/// Cung cấp các chức năng nghiệp vụ liên quan đến tác giả (<see cref="Author"/>):
/// kiểm tra tính hợp lệ của dữ liệu và điều phối các thao tác xuống tầng dữ liệu.
/// Quy ước chung: khi dữ liệu hoặc thao tác không hợp lệ thì ném <see cref="Exception"/> kèm thông báo lỗi.
/// </summary>
public class AuthorService
{
    /// <summary>
    /// Số ký tự tối đa của tên tác giả (theo cột <c>AuthorName</c> trong CSDL).
    /// </summary>
    private const int AuthorNameMaxLength = 200;

    private readonly IAuthorRepository _authorRepository;

    /// <summary>
    /// Khởi tạo dịch vụ nghiệp vụ tác giả.
    /// </summary>
    /// <param name="authorRepository">Đối tượng xử lý dữ liệu tác giả ở tầng dữ liệu.</param>
    /// <exception cref="Exception">Ném ra khi <paramref name="authorRepository"/> là <c>null</c>.</exception>
    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository
            ?? throw new Exception("Không có đối tượng xử lý dữ liệu tác giả.");
    }

    /// <summary>
    /// Lấy danh sách toàn bộ tác giả.
    /// </summary>
    /// <returns>Danh sách tác giả (rỗng nếu chưa có dữ liệu).</returns>
    public async Task<IEnumerable<Author>> ListAsync()
    {
        return await _authorRepository.ListAsync();
    }

    /// <summary>
    /// Lấy thông tin một tác giả theo mã.
    /// </summary>
    /// <param name="id">Mã tác giả (phải lớn hơn 0).</param>
    /// <returns>Tác giả tìm được; <c>null</c> nếu không tồn tại.</returns>
    /// <exception cref="Exception">Ném ra khi mã tác giả không hợp lệ.</exception>
    public async Task<Author?> GetAsync(int id)
    {
        ValidateId(id);
        return await _authorRepository.GetAsync(id);
    }

    /// <summary>
    /// Bổ sung một tác giả mới.
    /// </summary>
    /// <param name="data">Thông tin tác giả cần bổ sung (mã tác giả do hệ thống tự sinh).</param>
    /// <returns>Mã của tác giả vừa được bổ sung.</returns>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ hoặc không bổ sung được.</exception>
    public async Task<int> AddAsync(Author data)
    {
        ValidateAuthor(data);

        int id = await _authorRepository.AddAsync(data);
        if (id <= 0)
            throw new Exception("Không thể bổ sung tác giả.");

        return id;
    }

    /// <summary>
    /// Cập nhật thông tin một tác giả đã có.
    /// </summary>
    /// <param name="data">Thông tin mới của tác giả (xác định qua <see cref="Author.AuthorId"/>).</param>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ, tác giả không tồn tại hoặc không cập nhật được.</exception>
    public async Task UpdateAsync(Author data)
    {
        ValidateAuthor(data);
        ValidateId(data.AuthorId);

        var existing = await _authorRepository.GetAsync(data.AuthorId);
        if (existing == null)
            throw new Exception("Tác giả không tồn tại.");

        bool success = await _authorRepository.UpdateAsync(data);
        if (!success)
            throw new Exception("Không thể cập nhật thông tin tác giả.");
    }

    /// <summary>
    /// Xóa một tác giả. Chỉ được xóa khi tác giả tồn tại và chưa được sách nào sử dụng.
    /// </summary>
    /// <param name="id">Mã tác giả cần xóa.</param>
    /// <exception cref="Exception">Ném ra khi mã không hợp lệ, tác giả không tồn tại, đang được sử dụng hoặc không xóa được.</exception>
    public async Task DeleteAsync(int id)
    {
        ValidateId(id);

        var existing = await _authorRepository.GetAsync(id);
        if (existing == null)
            throw new Exception("Tác giả không tồn tại.");

        if (await _authorRepository.InUseAsync(id))
            throw new Exception("Tác giả đang được sử dụng trong dữ liệu sách nên không thể xóa.");

        bool success = await _authorRepository.DeleteAsync(id);
        if (!success)
            throw new Exception("Không thể xóa tác giả.");
    }

    /// <summary>
    /// Kiểm tra một tác giả có đang được dữ liệu khác (sách) sử dụng hay không.
    /// </summary>
    /// <param name="id">Mã tác giả cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được sử dụng; ngược lại <c>false</c>.</returns>
    /// <exception cref="Exception">Ném ra khi mã tác giả không hợp lệ.</exception>
    public async Task<bool> InUseAsync(int id)
    {
        ValidateId(id);
        return await _authorRepository.InUseAsync(id);
    }

    /// <summary>
    /// Kiểm tra mã tác giả hợp lệ (phải lớn hơn 0).
    /// </summary>
    private static void ValidateId(int id)
    {
        if (id <= 0)
            throw new Exception("Mã tác giả không hợp lệ.");
    }

    /// <summary>
    /// Kiểm tra tính hợp lệ của dữ liệu tác giả và chuẩn hóa các trường chuỗi
    /// (cắt khoảng trắng thừa hai đầu; tiểu sử rỗng được đổi thành <c>null</c>).
    /// </summary>
    /// <param name="data">Dữ liệu tác giả cần kiểm tra.</param>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ.</exception>
    private static void ValidateAuthor(Author data)
    {
        if (data == null)
            throw new Exception("Dữ liệu tác giả không được để trống.");

        // Chuẩn hóa chuỗi
        data.AuthorName = (data.AuthorName ?? string.Empty).Trim();
        data.Biography = string.IsNullOrWhiteSpace(data.Biography) ? null : data.Biography.Trim();

        // Tên tác giả: bắt buộc, tối đa 200 ký tự
        if (data.AuthorName.Length == 0)
            throw new Exception("Tên tác giả không được để trống.");
        if (data.AuthorName.Length > AuthorNameMaxLength)
            throw new Exception($"Tên tác giả không được vượt quá {AuthorNameMaxLength} ký tự.");

        // Ngày sinh, ngày mất: không được ở tương lai
        DateTime today = DateTime.Today;
        if (data.BirthDate.HasValue && data.BirthDate.Value.Date > today)
            throw new Exception("Ngày sinh không được lớn hơn ngày hiện tại.");
        if (data.DeathDate.HasValue && data.DeathDate.Value.Date > today)
            throw new Exception("Ngày mất không được lớn hơn ngày hiện tại.");

        // Ngày mất phải sau hoặc bằng ngày sinh (khớp ràng buộc CK_Authors_Dates)
        if (data.BirthDate.HasValue && data.DeathDate.HasValue
            && data.DeathDate.Value.Date < data.BirthDate.Value.Date)
            throw new Exception("Ngày mất phải lớn hơn hoặc bằng ngày sinh.");
    }
}
