using Dapper;
using SLib23T1020224.DataLayer.Interfaces;
using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.DataLayer.SQLServer;

/// <summary>
/// Cài đặt các phép xử lý dữ liệu trên bảng <c>Locations</c> (vị trí lưu trữ) bằng Dapper.
/// </summary>
public class LocationRepository : ILocationRepository
{
    private readonly DbHelper _db;

    /// <summary>
    /// Khởi tạo repository.
    /// </summary>
    /// <param name="db">Đối tượng cung cấp kết nối tới CSDL.</param>
    public LocationRepository(DbHelper db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Location>> ListAsync()
    {
        using var connection = _db.GetConnection();
        const string sql = "SELECT * FROM Locations ORDER BY LocationName";
        return await connection.QueryAsync<Location>(sql);
    }

    /// <inheritdoc />
    public async Task<Location?> GetAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = "SELECT * FROM Locations WHERE LocationId = @id";
        return await connection.QueryFirstOrDefaultAsync<Location>(sql, new { id });
    }

    /// <inheritdoc />
    public async Task<int> AddAsync(Location data)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                INSERT INTO Locations (LocationCode, LocationName, Floor, Room, Shelf, Description, IsActive)
                VALUES (@LocationCode, @LocationName, @Floor, @Room, @Shelf, @Description, @IsActive);
                SELECT CAST(SCOPE_IDENTITY() AS int);";
        return await connection.ExecuteScalarAsync<int>(sql, data);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(Location data)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                UPDATE Locations
                SET LocationCode = @LocationCode,
                    LocationName = @LocationName,
                    Floor = @Floor,
                    Room = @Room,
                    Shelf = @Shelf,
                    Description = @Description,
                    IsActive = @IsActive
                WHERE LocationId = @LocationId";
        return await connection.ExecuteAsync(sql, data) > 0;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = "DELETE FROM Locations WHERE LocationId = @id";
        return await connection.ExecuteAsync(sql, new { id }) > 0;
    }

    /// <summary>
    /// Kiểm tra vị trí lưu trữ có đang được tham chiếu tại bảng <c>BookCopies</c> (vị trí đang có bản sao sách) hay không.
    /// </summary>
    /// <param name="id">Mã vị trí lưu trữ cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được tham chiếu; ngược lại <c>false</c>.</returns>
    public async Task<bool> InUseAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                SELECT CAST(CASE WHEN
                    EXISTS (SELECT 1 FROM BookCopies WHERE LocationId = @id)
                THEN 1 ELSE 0 END AS bit)";
        return await connection.ExecuteScalarAsync<bool>(sql, new { id });
    }
}
