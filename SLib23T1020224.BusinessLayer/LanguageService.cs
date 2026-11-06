using SLib23T1020224.DataLayer.Interfaces;
using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.BusinessLayer;

/// <summary>
/// Cung cấp các chức năng nghiệp vụ liên quan đến ngôn ngữ (<see cref="Language"/>):
/// kiểm tra tính hợp lệ của dữ liệu và điều phối các thao tác xuống tầng dữ liệu.
/// Quy ước chung: khi dữ liệu hoặc thao tác không hợp lệ thì ném <see cref="Exception"/> kèm thông báo lỗi.
/// </summary>
public class LanguageService
{
    private readonly ILanguageRepository _repository;

    /// <summary>
    /// Khởi tạo dịch vụ nghiệp vụ ngôn ngữ.
    /// </summary>
    /// <param name="repository">Đối tượng xử lý dữ liệu ngôn ngữ ở tầng dữ liệu.</param>
    /// <exception cref="Exception">Ném ra khi <paramref name="repository"/> là <c>null</c>.</exception>
    public LanguageService(ILanguageRepository repository)
    {
        _repository = repository
            ?? throw new Exception("Không có đối tượng xử lý dữ liệu ngôn ngữ.");
    }

    /// <summary>
    /// Lấy danh sách toàn bộ ngôn ngữ.
    /// </summary>
    /// <returns>Danh sách ngôn ngữ (rỗng nếu chưa có dữ liệu).</returns>
    public async Task<IEnumerable<Language>> ListAsync()
    {
        return await _repository.ListAsync();
    }

    /// <summary>
    /// Lấy thông tin một ngôn ngữ theo mã.
    /// </summary>
    /// <param name="id">Mã ngôn ngữ (phải lớn hơn 0).</param>
    /// <returns>Đối tượng tìm được; <c>null</c> nếu không tồn tại.</returns>
    /// <exception cref="Exception">Ném ra khi mã không hợp lệ.</exception>
    public async Task<Language?> GetAsync(int id)
    {
        ValidationHelper.ValidateId(id, "ngôn ngữ");
        return await _repository.GetAsync(id);
    }

    /// <summary>
    /// Bổ sung một ngôn ngữ mới.
    /// </summary>
    /// <param name="data">Thông tin cần bổ sung (mã do hệ thống tự sinh).</param>
    /// <returns>Mã của bản ghi vừa được bổ sung.</returns>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ hoặc không bổ sung được.</exception>
    public async Task<int> AddAsync(Language data)
    {
        await ValidateAsync(data);

        int id = await _repository.AddAsync(data);
        if (id <= 0)
            throw new Exception("Không thể bổ sung ngôn ngữ.");

        return id;
    }

    /// <summary>
    /// Cập nhật thông tin một ngôn ngữ đã có.
    /// </summary>
    /// <param name="data">Thông tin mới (xác định qua <see cref="Language.LanguageId"/>).</param>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ, bản ghi không tồn tại hoặc không cập nhật được.</exception>
    public async Task UpdateAsync(Language data)
    {
        await ValidateAsync(data);
        ValidationHelper.ValidateId(data.LanguageId, "ngôn ngữ");

        var existing = await _repository.GetAsync(data.LanguageId);
        if (existing == null)
            throw new Exception("Ngôn ngữ không tồn tại.");

        bool success = await _repository.UpdateAsync(data);
        if (!success)
            throw new Exception("Không thể cập nhật thông tin ngôn ngữ.");
    }

    /// <summary>
    /// Xóa một ngôn ngữ. Chỉ được xóa khi bản ghi tồn tại và chưa được dữ liệu khác sử dụng.
    /// </summary>
    /// <param name="id">Mã cần xóa.</param>
    /// <exception cref="Exception">Ném ra khi mã không hợp lệ, bản ghi không tồn tại, đang được sử dụng hoặc không xóa được.</exception>
    public async Task DeleteAsync(int id)
    {
        ValidationHelper.ValidateId(id, "ngôn ngữ");

        var existing = await _repository.GetAsync(id);
        if (existing == null)
            throw new Exception("Ngôn ngữ không tồn tại.");

        if (await _repository.InUseAsync(id))
            throw new Exception("Ngôn ngữ đã có sách nên không thể xóa.");

        bool success = await _repository.DeleteAsync(id);
        if (!success)
            throw new Exception("Không thể xóa ngôn ngữ.");
    }

    /// <summary>
    /// Kiểm tra một ngôn ngữ có đang được dữ liệu khác sử dụng hay không.
    /// </summary>
    /// <param name="id">Mã cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được sử dụng; ngược lại <c>false</c>.</returns>
    /// <exception cref="Exception">Ném ra khi mã không hợp lệ.</exception>
    public async Task<bool> InUseAsync(int id)
    {
        ValidationHelper.ValidateId(id, "ngôn ngữ");
        return await _repository.InUseAsync(id);
    }

    /// <summary>
    /// Kiểm tra tính hợp lệ của dữ liệu ngôn ngữ, chuẩn hóa các trường chuỗi
    /// (mã ngôn ngữ được đổi sang chữ thường) và kiểm tra mã ngôn ngữ không bị trùng.
    /// </summary>
    /// <param name="data">Dữ liệu cần kiểm tra.</param>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ.</exception>
    private async Task ValidateAsync(Language data)
    {
        if (data == null)
            throw new Exception("Dữ liệu ngôn ngữ không được để trống.");

        data.LanguageCode = ValidationHelper.Required(data.LanguageCode, "Mã ngôn ngữ", 10).ToLowerInvariant();
        ValidationHelper.ValidateCode(data.LanguageCode, "Mã ngôn ngữ");

        data.LanguageName = ValidationHelper.Required(data.LanguageName, "Tên ngôn ngữ", 100);

        // Mã ngôn ngữ phải duy nhất (khớp ràng buộc UQ_Languages_Code)
        var all = await _repository.ListAsync();
        bool duplicated = all.Any(x => x.LanguageId != data.LanguageId
            && string.Equals(x.LanguageCode, data.LanguageCode, StringComparison.OrdinalIgnoreCase));
        if (duplicated)
            throw new Exception("Mã ngôn ngữ đã tồn tại.");
    }
}
