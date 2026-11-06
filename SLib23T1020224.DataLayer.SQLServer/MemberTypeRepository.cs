using Dapper;
using SLib23T1020224.DataLayer.Interfaces;
using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.DataLayer.SQLServer;

/// <summary>
/// Cài đặt các phép xử lý dữ liệu trên bảng <c>MemberTypes</c> (loại độc giả) bằng Dapper.
/// </summary>
public class MemberTypeRepository : IMemberTypeRepository
{
    private readonly DbHelper _db;

    /// <summary>
    /// Khởi tạo repository.
    /// </summary>
    /// <param name="db">Đối tượng cung cấp kết nối tới CSDL.</param>
    public MemberTypeRepository(DbHelper db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MemberType>> ListAsync()
    {
        using var connection = _db.GetConnection();
        const string sql = "SELECT * FROM MemberTypes ORDER BY TypeName";
        return await connection.QueryAsync<MemberType>(sql);
    }

    /// <inheritdoc />
    public async Task<MemberType?> GetAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = "SELECT * FROM MemberTypes WHERE MemberTypeId = @id";
        return await connection.QueryFirstOrDefaultAsync<MemberType>(sql, new { id });
    }

    /// <inheritdoc />
    public async Task<int> AddAsync(MemberType data)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                INSERT INTO MemberTypes (TypeName, MaxActiveLoans, LoanPeriodDays, MaxRenewals, IsActive)
                VALUES (@TypeName, @MaxActiveLoans, @LoanPeriodDays, @MaxRenewals, @IsActive);
                SELECT CAST(SCOPE_IDENTITY() AS int);";
        return await connection.ExecuteScalarAsync<int>(sql, data);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(MemberType data)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                UPDATE MemberTypes
                SET TypeName = @TypeName,
                    MaxActiveLoans = @MaxActiveLoans,
                    LoanPeriodDays = @LoanPeriodDays,
                    MaxRenewals = @MaxRenewals,
                    IsActive = @IsActive
                WHERE MemberTypeId = @MemberTypeId";
        return await connection.ExecuteAsync(sql, data) > 0;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = "DELETE FROM MemberTypes WHERE MemberTypeId = @id";
        return await connection.ExecuteAsync(sql, new { id }) > 0;
    }

    /// <summary>
    /// Kiểm tra loại độc giả có đang được tham chiếu tại bảng <c>Members</c> (loại độc giả đã có độc giả) hay không.
    /// </summary>
    /// <param name="id">Mã loại độc giả cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được tham chiếu; ngược lại <c>false</c>.</returns>
    public async Task<bool> InUseAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                SELECT CAST(CASE WHEN
                    EXISTS (SELECT 1 FROM Members WHERE MemberTypeId = @id)
                THEN 1 ELSE 0 END AS bit)";
        return await connection.ExecuteScalarAsync<bool>(sql, new { id });
    }
}
