using Dapper;
using SLib23T1020224.DataLayer.Interfaces;
using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.DataLayer.SQLServer;

/// <summary>
/// Cài đặt các phép xử lý dữ liệu trên bảng <c>Categories</c> (thể loại) bằng Dapper.
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    private readonly DbHelper _db;

    /// <summary>
    /// Khởi tạo repository.
    /// </summary>
    /// <param name="db">Đối tượng cung cấp kết nối tới CSDL.</param>
    public CategoryRepository(DbHelper db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Category>> ListAsync()
    {
        using var connection = _db.GetConnection();
        const string sql = "SELECT * FROM Categories ORDER BY CategoryName";
        return await connection.QueryAsync<Category>(sql);
    }

    /// <inheritdoc />
    public async Task<Category?> GetAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = "SELECT * FROM Categories WHERE CategoryId = @id";
        return await connection.QueryFirstOrDefaultAsync<Category>(sql, new { id });
    }

    /// <inheritdoc />
    public async Task<int> AddAsync(Category data)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                INSERT INTO Categories (CategoryName, ParentCategoryId, Description, IsActive)
                VALUES (@CategoryName, @ParentCategoryId, @Description, @IsActive);
                SELECT CAST(SCOPE_IDENTITY() AS int);";
        return await connection.ExecuteScalarAsync<int>(sql, data);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(Category data)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                UPDATE Categories
                SET CategoryName = @CategoryName,
                    ParentCategoryId = @ParentCategoryId,
                    Description = @Description,
                    IsActive = @IsActive
                WHERE CategoryId = @CategoryId";
        return await connection.ExecuteAsync(sql, data) > 0;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = "DELETE FROM Categories WHERE CategoryId = @id";
        return await connection.ExecuteAsync(sql, new { id }) > 0;
    }

    /// <summary>
    /// Kiểm tra thể loại có đang được tham chiếu tại bảng <c>BookCategories</c> (thể loại đã gắn với sách) và chính bảng <c>Categories</c> (thể loại đang là cha của thể loại khác) hay không.
    /// </summary>
    /// <param name="id">Mã thể loại cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được tham chiếu; ngược lại <c>false</c>.</returns>
    public async Task<bool> InUseAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                SELECT CAST(CASE WHEN
                    EXISTS (SELECT 1 FROM BookCategories WHERE CategoryId = @id)
                    OR EXISTS (SELECT 1 FROM Categories WHERE ParentCategoryId = @id)
                THEN 1 ELSE 0 END AS bit)";
        return await connection.ExecuteScalarAsync<bool>(sql, new { id });
    }
}
