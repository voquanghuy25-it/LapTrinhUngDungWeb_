using SLib23T1020224.DataLayer.Interfaces;
using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.BusinessLayer;

/// <summary>
/// Cung cấp các chức năng nghiệp vụ liên quan đến thể loại (<see cref="Category"/>):
/// kiểm tra tính hợp lệ của dữ liệu và điều phối các thao tác xuống tầng dữ liệu.
/// Quy ước chung: khi dữ liệu hoặc thao tác không hợp lệ thì ném <see cref="Exception"/> kèm thông báo lỗi.
/// </summary>
public class CategoryService
{
    private readonly ICategoryRepository _repository;

    /// <summary>
    /// Khởi tạo dịch vụ nghiệp vụ thể loại.
    /// </summary>
    /// <param name="repository">Đối tượng xử lý dữ liệu thể loại ở tầng dữ liệu.</param>
    /// <exception cref="Exception">Ném ra khi <paramref name="repository"/> là <c>null</c>.</exception>
    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository
            ?? throw new Exception("Không có đối tượng xử lý dữ liệu thể loại.");
    }

    /// <summary>
    /// Lấy danh sách toàn bộ thể loại.
    /// </summary>
    /// <returns>Danh sách thể loại (rỗng nếu chưa có dữ liệu).</returns>
    public async Task<IEnumerable<Category>> ListAsync()
    {
        return await _repository.ListAsync();
    }

    /// <summary>
    /// Lấy thông tin một thể loại theo mã.
    /// </summary>
    /// <param name="id">Mã thể loại (phải lớn hơn 0).</param>
    /// <returns>Đối tượng tìm được; <c>null</c> nếu không tồn tại.</returns>
    /// <exception cref="Exception">Ném ra khi mã không hợp lệ.</exception>
    public async Task<Category?> GetAsync(int id)
    {
        ValidationHelper.ValidateId(id, "thể loại");
        return await _repository.GetAsync(id);
    }

    /// <summary>
    /// Bổ sung một thể loại mới.
    /// </summary>
    /// <param name="data">Thông tin cần bổ sung (mã do hệ thống tự sinh).</param>
    /// <returns>Mã của bản ghi vừa được bổ sung.</returns>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ hoặc không bổ sung được.</exception>
    public async Task<int> AddAsync(Category data)
    {
        await ValidateAsync(data);

        int id = await _repository.AddAsync(data);
        if (id <= 0)
            throw new Exception("Không thể bổ sung thể loại.");

        return id;
    }

    /// <summary>
    /// Cập nhật thông tin một thể loại đã có.
    /// </summary>
    /// <param name="data">Thông tin mới (xác định qua <see cref="Category.CategoryId"/>).</param>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ, bản ghi không tồn tại hoặc không cập nhật được.</exception>
    public async Task UpdateAsync(Category data)
    {
        await ValidateAsync(data);
        ValidationHelper.ValidateId(data.CategoryId, "thể loại");

        var existing = await _repository.GetAsync(data.CategoryId);
        if (existing == null)
            throw new Exception("Thể loại không tồn tại.");

        bool success = await _repository.UpdateAsync(data);
        if (!success)
            throw new Exception("Không thể cập nhật thông tin thể loại.");
    }

    /// <summary>
    /// Xóa một thể loại. Chỉ được xóa khi bản ghi tồn tại và chưa được dữ liệu khác sử dụng.
    /// </summary>
    /// <param name="id">Mã cần xóa.</param>
    /// <exception cref="Exception">Ném ra khi mã không hợp lệ, bản ghi không tồn tại, đang được sử dụng hoặc không xóa được.</exception>
    public async Task DeleteAsync(int id)
    {
        ValidationHelper.ValidateId(id, "thể loại");

        var existing = await _repository.GetAsync(id);
        if (existing == null)
            throw new Exception("Thể loại không tồn tại.");

        if (await _repository.InUseAsync(id))
            throw new Exception("Thể loại đang được sử dụng (đã gắn với sách hoặc là thể loại cha của thể loại khác) nên không thể xóa.");

        bool success = await _repository.DeleteAsync(id);
        if (!success)
            throw new Exception("Không thể xóa thể loại.");
    }

    /// <summary>
    /// Kiểm tra một thể loại có đang được dữ liệu khác sử dụng hay không.
    /// </summary>
    /// <param name="id">Mã cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được sử dụng; ngược lại <c>false</c>.</returns>
    /// <exception cref="Exception">Ném ra khi mã không hợp lệ.</exception>
    public async Task<bool> InUseAsync(int id)
    {
        ValidationHelper.ValidateId(id, "thể loại");
        return await _repository.InUseAsync(id);
    }

    /// <summary>
    /// Kiểm tra tính hợp lệ của dữ liệu thể loại: tên bắt buộc, thể loại cha phải tồn tại,
    /// không được là chính nó và không được là thể loại con/cháu của nó (tránh vòng lặp).
    /// </summary>
    /// <param name="data">Dữ liệu cần kiểm tra.</param>
    /// <exception cref="Exception">Ném ra khi dữ liệu không hợp lệ.</exception>
    private async Task ValidateAsync(Category data)
    {
        if (data == null)
            throw new Exception("Dữ liệu thể loại không được để trống.");

        data.CategoryName = ValidationHelper.Required(data.CategoryName, "Tên thể loại", 150);
        data.Description = ValidationHelper.Optional(data.Description, "Mô tả", 500);

        if (!data.ParentCategoryId.HasValue)
            return;

        int parentId = data.ParentCategoryId.Value;
        if (parentId <= 0)
            throw new Exception("Thể loại cha không hợp lệ.");

        // Khớp ràng buộc CK_Categories_NotOwnParent
        if (data.CategoryId > 0 && parentId == data.CategoryId)
            throw new Exception("Thể loại không thể là cha của chính nó.");

        var parent = await _repository.GetAsync(parentId);
        if (parent == null)
            throw new Exception("Thể loại cha không tồn tại.");

        // Khi sửa: đi ngược lên chuỗi cha của thể loại cha được chọn,
        // nếu gặp lại chính thể loại đang sửa thì sẽ tạo vòng lặp => không hợp lệ.
        if (data.CategoryId > 0)
        {
            var visited = new HashSet<int>();
            int? current = parent.ParentCategoryId;
            while (current.HasValue && visited.Add(current.Value))
            {
                if (current.Value == data.CategoryId)
                    throw new Exception("Không thể chọn thể loại con/cháu làm thể loại cha.");

                var next = await _repository.GetAsync(current.Value);
                current = next?.ParentCategoryId;
            }
        }
    }
}
