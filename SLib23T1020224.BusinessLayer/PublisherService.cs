using SLib23T1020224.DataLayer.Interfaces;
using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.BusinessLayer;

/// <summary>
/// Cung cấp các chức năng nghiệp vụ liên quan đến nhà xuất bản (<see cref="Publisher"/>):
/// kiểm tra tính hợp lệ của dữ liệu và điều phối các thao tác xuống tầng dữ liệu.
/// Quy ước chung: khi dữ liệu hoặc thao tác không hợp lệ thì ném <see cref="Exception"/> kèm thông báo lỗi.
/// </summary>
public class PublisherService
{
    private readonly IPublisherRepository _repository;

    /// <summary>
    /// Khởi tạo dịch vụ nghiệp vụ nhà xuất bản.
    /// </summary>
    /// <param name="repository">Đối tượng xử lý dữ liệu nhà xuất bản ở tầng dữ liệu.</param>
    /// <exception cref="Exception">Ném ra khi <paramref name="repository"/> là <c>null</c>.</exception>
    public PublisherService(IPublisherRepository repository)
    {
        _repository = repository
            ?? throw new Exception("Không có đối tượng xử lý dữ liệu nhà xuất bản.");
    }

    /// <summary>
    /// Lấy danh sách toàn bộ nhà xuất bản.
    /// </summary>
    /// <returns>Danh sách nhà xuất bản (rỗng nếu chưa có dữ liệu).</returns>
    public async Task<IEnumerable<Publisher>> ListAsync()
    {
        return await _repository.ListAsync();
    }

    /// <summary>
    /// Lấy thông tin một nhà xuất bản theo mã.
    /// </summary>
    /// <param name="id">Mã nhà xuất bản (phải lớn hơn 0).</param>
    /// <returns>Đối tượng tìm được; <c>null</c> nếu không tồn tại.</returns>
    /// <exception cref="Exception">Ném ra khi mã không hợp lệ.</exception>
    public async Task<Publisher?> GetAsync(int id)
    {
        ValidationHelper.ValidateId(id, "nhà xuất bản");
        return await _repository.GetAsync(id);
    }

    /// <summary>
    /// Bổ sung một nhà xuất bản mới.
    /// </summary>
    /// <param name="data">Thông tin cần bổ sung (mã do hệ thống tự sinh).</param>
    /// <returns>Mã của bản ghi vừa được bổ sung.</returns>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ hoặc không bổ sung được.</exception>
    public async Task<int> AddAsync(Publisher data)
    {
        Validate(data);

        int id = await _repository.AddAsync(data);
        if (id <= 0)
            throw new Exception("Không thể bổ sung nhà xuất bản.");

        return id;
    }

    /// <summary>
    /// Cập nhật thông tin một nhà xuất bản đã có.
    /// </summary>
    /// <param name="data">Thông tin mới (xác định qua <see cref="Publisher.PublisherId"/>).</param>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ, bản ghi không tồn tại hoặc không cập nhật được.</exception>
    public async Task UpdateAsync(Publisher data)
    {
        Validate(data);
        ValidationHelper.ValidateId(data.PublisherId, "nhà xuất bản");

        var existing = await _repository.GetAsync(data.PublisherId);
        if (existing == null)
            throw new Exception("Nhà xuất bản không tồn tại.");

        bool success = await _repository.UpdateAsync(data);
        if (!success)
            throw new Exception("Không thể cập nhật thông tin nhà xuất bản.");
    }

    /// <summary>
    /// Xóa một nhà xuất bản. Chỉ được xóa khi bản ghi tồn tại và chưa được dữ liệu khác sử dụng.
    /// </summary>
    /// <param name="id">Mã cần xóa.</param>
    /// <exception cref="Exception">Ném ra khi mã không hợp lệ, bản ghi không tồn tại, đang được sử dụng hoặc không xóa được.</exception>
    public async Task DeleteAsync(int id)
    {
        ValidationHelper.ValidateId(id, "nhà xuất bản");

        var existing = await _repository.GetAsync(id);
        if (existing == null)
            throw new Exception("Nhà xuất bản không tồn tại.");

        if (await _repository.InUseAsync(id))
            throw new Exception("Nhà xuất bản đã có sách nên không thể xóa.");

        bool success = await _repository.DeleteAsync(id);
        if (!success)
            throw new Exception("Không thể xóa nhà xuất bản.");
    }

    /// <summary>
    /// Kiểm tra một nhà xuất bản có đang được dữ liệu khác sử dụng hay không.
    /// </summary>
    /// <param name="id">Mã cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được sử dụng; ngược lại <c>false</c>.</returns>
    /// <exception cref="Exception">Ném ra khi mã không hợp lệ.</exception>
    public async Task<bool> InUseAsync(int id)
    {
        ValidationHelper.ValidateId(id, "nhà xuất bản");
        return await _repository.InUseAsync(id);
    }

    /// <summary>
    /// Kiểm tra tính hợp lệ của dữ liệu nhà xuất bản và chuẩn hóa các trường chuỗi.
    /// </summary>
    /// <param name="data">Dữ liệu cần kiểm tra.</param>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ.</exception>
    private static void Validate(Publisher data)
    {
        if (data == null)
            throw new Exception("Dữ liệu nhà xuất bản không được để trống.");

        data.PublisherName = ValidationHelper.Required(data.PublisherName, "Tên nhà xuất bản", 200);
        data.Address = ValidationHelper.Optional(data.Address, "Địa chỉ", 300);

        data.Phone = ValidationHelper.Optional(data.Phone, "Số điện thoại", 20);
        if (data.Phone != null)
            ValidationHelper.ValidatePhone(data.Phone);

        data.Email = ValidationHelper.Optional(data.Email, "Email", 254);
        if (data.Email != null)
            ValidationHelper.ValidateEmail(data.Email);

        data.Website = ValidationHelper.Optional(data.Website, "Website", 300);
        if (data.Website != null)
            ValidationHelper.ValidateWebsite(data.Website);
    }
}
