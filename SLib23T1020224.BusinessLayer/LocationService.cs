using SLib23T1020224.DataLayer.Interfaces;
using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.BusinessLayer;

/// <summary>
/// Cung cấp các chức năng nghiệp vụ liên quan đến vị trí lưu trữ (<see cref="Location"/>):
/// kiểm tra tính hợp lệ của dữ liệu và điều phối các thao tác xuống tầng dữ liệu.
/// Quy ước chung: khi dữ liệu hoặc thao tác không hợp lệ thì ném <see cref="Exception"/> kèm thông báo lỗi.
/// </summary>
public class LocationService
{
    private readonly ILocationRepository _repository;

    /// <summary>
    /// Khởi tạo dịch vụ nghiệp vụ vị trí lưu trữ.
    /// </summary>
    /// <param name="repository">Đối tượng xử lý dữ liệu vị trí lưu trữ ở tầng dữ liệu.</param>
    /// <exception cref="Exception">Ném ra khi <paramref name="repository"/> là <c>null</c>.</exception>
    public LocationService(ILocationRepository repository)
    {
        _repository = repository
            ?? throw new Exception("Không có đối tượng xử lý dữ liệu vị trí lưu trữ.");
    }

    /// <summary>
    /// Lấy danh sách toàn bộ vị trí lưu trữ.
    /// </summary>
    /// <returns>Danh sách vị trí lưu trữ (rỗng nếu chưa có dữ liệu).</returns>
    public async Task<IEnumerable<Location>> ListAsync()
    {
        return await _repository.ListAsync();
    }

    /// <summary>
    /// Lấy thông tin một vị trí lưu trữ theo mã.
    /// </summary>
    /// <param name="id">Mã vị trí lưu trữ (phải lớn hơn 0).</param>
    /// <returns>Đối tượng tìm được; <c>null</c> nếu không tồn tại.</returns>
    /// <exception cref="Exception">Ném ra khi mã không hợp lệ.</exception>
    public async Task<Location?> GetAsync(int id)
    {
        ValidationHelper.ValidateId(id, "vị trí lưu trữ");
        return await _repository.GetAsync(id);
    }

    /// <summary>
    /// Bổ sung một vị trí lưu trữ mới.
    /// </summary>
    /// <param name="data">Thông tin cần bổ sung (mã do hệ thống tự sinh).</param>
    /// <returns>Mã của bản ghi vừa được bổ sung.</returns>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ hoặc không bổ sung được.</exception>
    public async Task<int> AddAsync(Location data)
    {
        await ValidateAsync(data);

        int id = await _repository.AddAsync(data);
        if (id <= 0)
            throw new Exception("Không thể bổ sung vị trí lưu trữ.");

        return id;
    }

    /// <summary>
    /// Cập nhật thông tin một vị trí lưu trữ đã có.
    /// </summary>
    /// <param name="data">Thông tin mới (xác định qua <see cref="Location.LocationId"/>).</param>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ, bản ghi không tồn tại hoặc không cập nhật được.</exception>
    public async Task UpdateAsync(Location data)
    {
        await ValidateAsync(data);
        ValidationHelper.ValidateId(data.LocationId, "vị trí lưu trữ");

        var existing = await _repository.GetAsync(data.LocationId);
        if (existing == null)
            throw new Exception("Vị trí lưu trữ không tồn tại.");

        bool success = await _repository.UpdateAsync(data);
        if (!success)
            throw new Exception("Không thể cập nhật thông tin vị trí lưu trữ.");
    }

    /// <summary>
    /// Xóa một vị trí lưu trữ. Chỉ được xóa khi bản ghi tồn tại và chưa được dữ liệu khác sử dụng.
    /// </summary>
    /// <param name="id">Mã cần xóa.</param>
    /// <exception cref="Exception">Ném ra khi mã không hợp lệ, bản ghi không tồn tại, đang được sử dụng hoặc không xóa được.</exception>
    public async Task DeleteAsync(int id)
    {
        ValidationHelper.ValidateId(id, "vị trí lưu trữ");

        var existing = await _repository.GetAsync(id);
        if (existing == null)
            throw new Exception("Vị trí lưu trữ không tồn tại.");

        if (await _repository.InUseAsync(id))
            throw new Exception("Vị trí lưu trữ đang có bản sao sách nên không thể xóa.");

        bool success = await _repository.DeleteAsync(id);
        if (!success)
            throw new Exception("Không thể xóa vị trí lưu trữ.");
    }

    /// <summary>
    /// Kiểm tra một vị trí lưu trữ có đang được dữ liệu khác sử dụng hay không.
    /// </summary>
    /// <param name="id">Mã cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được sử dụng; ngược lại <c>false</c>.</returns>
    /// <exception cref="Exception">Ném ra khi mã không hợp lệ.</exception>
    public async Task<bool> InUseAsync(int id)
    {
        ValidationHelper.ValidateId(id, "vị trí lưu trữ");
        return await _repository.InUseAsync(id);
    }

    /// <summary>
    /// Kiểm tra tính hợp lệ của dữ liệu vị trí lưu trữ, chuẩn hóa các trường chuỗi
    /// (mã vị trí được đổi sang chữ in hoa) và kiểm tra mã vị trí không bị trùng.
    /// </summary>
    /// <param name="data">Dữ liệu cần kiểm tra.</param>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ.</exception>
    private async Task ValidateAsync(Location data)
    {
        if (data == null)
            throw new Exception("Dữ liệu vị trí lưu trữ không được để trống.");

        data.LocationCode = ValidationHelper.Required(data.LocationCode, "Mã vị trí", 30).ToUpperInvariant();
        ValidationHelper.ValidateCode(data.LocationCode, "Mã vị trí");

        data.LocationName = ValidationHelper.Required(data.LocationName, "Tên vị trí", 150);
        data.Floor = ValidationHelper.Optional(data.Floor, "Tầng", 30);
        data.Room = ValidationHelper.Optional(data.Room, "Phòng", 50);
        data.Shelf = ValidationHelper.Optional(data.Shelf, "Kệ", 50);
        data.Description = ValidationHelper.Optional(data.Description, "Mô tả", 300);

        // Mã vị trí phải duy nhất (khớp ràng buộc UQ_Locations_Code)
        var all = await _repository.ListAsync();
        bool duplicated = all.Any(x => x.LocationId != data.LocationId
            && string.Equals(x.LocationCode, data.LocationCode, StringComparison.OrdinalIgnoreCase));
        if (duplicated)
            throw new Exception("Mã vị trí đã tồn tại.");
    }
}
