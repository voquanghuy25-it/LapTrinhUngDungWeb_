using Dapper;
using SLib23T1020224.DataLayer.Interfaces;
using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.DataLayer.SQLServer;

/// <summary>
/// Cài đặt các phép xử lý dữ liệu trên bảng <c>Publishers</c> (nhà xuất bản) bằng Dapper.
/// </summary>
public class PublisherRepository : IPublisherRepository
{
    private readonly DbHelper _db;

    /// <summary>
    /// Khởi tạo repository.
    /// </summary>
    /// <param name="db">Đối tượng cung cấp kết nối tới CSDL.</param>
    public PublisherRepository(DbHelper db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Publisher>> ListAsync()
    {
        using var connection = _db.GetConnection();
        const string sql = "SELECT * FROM Publishers ORDER BY PublisherName";
        return await connection.QueryAsync<Publisher>(sql);
    }

    /// <inheritdoc />
    public async Task<Publisher?> GetAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = "SELECT * FROM Publishers WHERE PublisherId = @id";
        return await connection.QueryFirstOrDefaultAsync<Publisher>(sql, new { id });
    }

    /// <inheritdoc />
    public async Task<int> AddAsync(Publisher data)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                INSERT INTO Publishers (PublisherName, Address, Phone, Email, Website, IsActive)
                VALUES (@PublisherName, @Address, @Phone, @Email, @Website, @IsActive);
                SELECT CAST(SCOPE_IDENTITY() AS int);";
        return await connection.ExecuteScalarAsync<int>(sql, data);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(Publisher data)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                UPDATE Publishers
                SET PublisherName = @PublisherName,
                    Address = @Address,
                    Phone = @Phone,
                    Email = @Email,
                    Website = @Website,
                    IsActive = @IsActive
                WHERE PublisherId = @PublisherId";
        return await connection.ExecuteAsync(sql, data) > 0;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = "DELETE FROM Publishers WHERE PublisherId = @id";
        return await connection.ExecuteAsync(sql, new { id }) > 0;
    }

    /// <summary>
    /// Kiểm tra nhà xuất bản có đang được tham chiếu tại bảng <c>Books</c> (nhà xuất bản đã có sách) hay không.
    /// </summary>
    /// <param name="id">Mã nhà xuất bản cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được tham chiếu; ngược lại <c>false</c>.</returns>
    public async Task<bool> InUseAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                SELECT CAST(CASE WHEN
                    EXISTS (SELECT 1 FROM Books WHERE PublisherId = @id)
                THEN 1 ELSE 0 END AS bit)";
        return await connection.ExecuteScalarAsync<bool>(sql, new { id });
    }
}
