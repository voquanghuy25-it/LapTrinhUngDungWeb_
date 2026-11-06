using Dapper;
using SLib23T1020224.DataLayer.Interfaces;
using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.DataLayer.SQLServer;

/// <summary>
/// Cài đặt các phép xử lý dữ liệu trên bảng <c>Languages</c> (ngôn ngữ) bằng Dapper.
/// </summary>
public class LanguageRepository : ILanguageRepository
{
    private readonly DbHelper _db;

    /// <summary>
    /// Khởi tạo repository.
    /// </summary>
    /// <param name="db">Đối tượng cung cấp kết nối tới CSDL.</param>
    public LanguageRepository(DbHelper db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Language>> ListAsync()
    {
        using var connection = _db.GetConnection();
        const string sql = "SELECT * FROM Languages ORDER BY LanguageName";
        return await connection.QueryAsync<Language>(sql);
    }

    /// <inheritdoc />
    public async Task<Language?> GetAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = "SELECT * FROM Languages WHERE LanguageId = @id";
        return await connection.QueryFirstOrDefaultAsync<Language>(sql, new { id });
    }

    /// <inheritdoc />
    public async Task<int> AddAsync(Language data)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                INSERT INTO Languages (LanguageCode, LanguageName, IsActive)
                VALUES (@LanguageCode, @LanguageName, @IsActive);
                SELECT CAST(SCOPE_IDENTITY() AS int);";
        return await connection.ExecuteScalarAsync<int>(sql, data);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(Language data)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                UPDATE Languages
                SET LanguageCode = @LanguageCode,
                    LanguageName = @LanguageName,
                    IsActive = @IsActive
                WHERE LanguageId = @LanguageId";
        return await connection.ExecuteAsync(sql, data) > 0;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = "DELETE FROM Languages WHERE LanguageId = @id";
        return await connection.ExecuteAsync(sql, new { id }) > 0;
    }

    /// <summary>
    /// Kiểm tra ngôn ngữ có đang được tham chiếu tại bảng <c>Books</c> (ngôn ngữ đã có sách) hay không.
    /// </summary>
    /// <param name="id">Mã ngôn ngữ cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được tham chiếu; ngược lại <c>false</c>.</returns>
    public async Task<bool> InUseAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                SELECT CAST(CASE WHEN
                    EXISTS (SELECT 1 FROM Books WHERE LanguageId = @id)
                THEN 1 ELSE 0 END AS bit)";
        return await connection.ExecuteScalarAsync<bool>(sql, new { id });
    }
}
