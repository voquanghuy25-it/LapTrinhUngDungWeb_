using SLib23T1020224.DataLayer.Interfaces;
using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.BusinessLayer;

/// <summary>
/// Cung cấp các chức năng nghiệp vụ liên quan đến loại độc giả (<see cref="MemberType"/>):
/// kiểm tra tính hợp lệ của dữ liệu và điều phối các thao tác xuống tầng dữ liệu.
/// Quy ước chung: khi dữ liệu hoặc thao tác không hợp lệ thì ném <see cref="Exception"/> kèm thông báo lỗi.
/// </summary>
public class MemberTypeService
{
    private readonly IMemberTypeRepository _repository;

    /// <summary>
    /// Khởi tạo dịch vụ nghiệp vụ loại độc giả.
    /// </summary>
    /// <param name="repository">Đối tượng xử lý dữ liệu loại độc giả ở tầng dữ liệu.</param>
    /// <exception cref="Exception">Ném ra khi <paramref name="repository"/> là <c>null</c>.</exception>
    public MemberTypeService(IMemberTypeRepository repository)
    {
        _repository = repository
            ?? throw new Exception("Không có đối tượng xử lý dữ liệu loại độc giả.");
    }

    /// <summary>
    /// Lấy danh sách toàn bộ loại độc giả.
    /// </summary>
    /// <returns>Danh sách loại độc giả (rỗng nếu chưa có dữ liệu).</returns>
    public async Task<IEnumerable<MemberType>> ListAsync()
    {
        return await _repository.ListAsync();
    }

    /// <summary>
    /// Lấy thông tin một loại độc giả theo mã.
    /// </summary>
    /// <param name="id">Mã loại độc giả (phải lớn hơn 0).</param>
    /// <returns>Đối tượng tìm được; <c>null</c> nếu không tồn tại.</returns>
    /// <exception cref="Exception">Ném ra khi mã không hợp lệ.</exception>
    public async Task<MemberType?> GetAsync(int id)
    {
        ValidationHelper.ValidateId(id, "loại độc giả");
        return await _repository.GetAsync(id);
    }

    /// <summary>
    /// Bổ sung một loại độc giả mới.
    /// </summary>
    /// <param name="data">Thông tin cần bổ sung (mã do hệ thống tự sinh).</param>
    /// <returns>Mã của bản ghi vừa được bổ sung.</returns>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ hoặc không bổ sung được.</exception>
    public async Task<int> AddAsync(MemberType data)
    {
        await ValidateAsync(data);

        int id = await _repository.AddAsync(data);
        if (id <= 0)
            throw new Exception("Không thể bổ sung loại độc giả.");

        return id;
    }

    /// <summary>
    /// Cập nhật thông tin một loại độc giả đã có.
    /// </summary>
    /// <param name="data">Thông tin mới (xác định qua <see cref="MemberType.MemberTypeId"/>).</param>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ, bản ghi không tồn tại hoặc không cập nhật được.</exception>
    public async Task UpdateAsync(MemberType data)
    {
        await ValidateAsync(data);
        ValidationHelper.ValidateId(data.MemberTypeId, "loại độc giả");

        var existing = await _repository.GetAsync(data.MemberTypeId);
        if (existing == null)
            throw new Exception("Loại độc giả không tồn tại.");

        bool success = await _repository.UpdateAsync(data);
        if (!success)
            throw new Exception("Không thể cập nhật thông tin loại độc giả.");
    }

    /// <summary>
    /// Xóa một loại độc giả. Chỉ được xóa khi bản ghi tồn tại và chưa được dữ liệu khác sử dụng.
    /// </summary>
    /// <param name="id">Mã cần xóa.</param>
    /// <exception cref="Exception">Ném ra khi mã không hợp lệ, bản ghi không tồn tại, đang được sử dụng hoặc không xóa được.</exception>
    public async Task DeleteAsync(int id)
    {
        ValidationHelper.ValidateId(id, "loại độc giả");

        var existing = await _repository.GetAsync(id);
        if (existing == null)
            throw new Exception("Loại độc giả không tồn tại.");

        if (await _repository.InUseAsync(id))
            throw new Exception("Loại độc giả đã có độc giả nên không thể xóa.");

        bool success = await _repository.DeleteAsync(id);
        if (!success)
            throw new Exception("Không thể xóa loại độc giả.");
    }

    /// <summary>
    /// Kiểm tra một loại độc giả có đang được dữ liệu khác sử dụng hay không.
    /// </summary>
    /// <param name="id">Mã cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được sử dụng; ngược lại <c>false</c>.</returns>
    /// <exception cref="Exception">Ném ra khi mã không hợp lệ.</exception>
    public async Task<bool> InUseAsync(int id)
    {
        ValidationHelper.ValidateId(id, "loại độc giả");
        return await _repository.InUseAsync(id);
    }

    /// <summary>
    /// Kiểm tra tính hợp lệ của dữ liệu loại độc giả: tên bắt buộc và không trùng,
    /// số sách tối đa &gt;= 0, số ngày mượn &gt; 0, số lần gia hạn &gt;= 0 (khớp các ràng buộc CHECK trong CSDL).
    /// </summary>
    /// <param name="data">Dữ liệu cần kiểm tra.</param>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ.</exception>
    private async Task ValidateAsync(MemberType data)
    {
        if (data == null)
            throw new Exception("Dữ liệu loại độc giả không được để trống.");

        data.TypeName = ValidationHelper.Required(data.TypeName, "Tên loại độc giả", 100);

        if (data.MaxActiveLoans < 0)
            throw new Exception("Số sách được mượn tối đa không được nhỏ hơn 0.");
        if (data.LoanPeriodDays <= 0)
            throw new Exception("Số ngày mượn phải lớn hơn 0.");
        if (data.MaxRenewals < 0)
            throw new Exception("Số lần gia hạn tối đa không được nhỏ hơn 0.");

        // Tên loại độc giả phải duy nhất (khớp ràng buộc UQ_MemberTypes_TypeName)
        var all = await _repository.ListAsync();
        bool duplicated = all.Any(x => x.MemberTypeId != data.MemberTypeId
            && string.Equals(x.TypeName, data.TypeName, StringComparison.OrdinalIgnoreCase));
        if (duplicated)
            throw new Exception("Tên loại độc giả đã tồn tại.");
    }
}
