using Dapper;
using SLib23T1020224.DataLayer.Interfaces;
using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.DataLayer.SQLServer;

/// <summary>
/// Cài đặt các phép xử lý dữ liệu trên bảng <c>Authors</c> (tác giả) bằng Dapper.
/// </summary>
public class AuthorRepository : IAuthorRepository
{
    private readonly DbHelper _db;

    /// <summary>
    /// Khởi tạo repository.
    /// </summary>
    /// <param name="db">Đối tượng cung cấp kết nối tới CSDL.</param>
    public AuthorRepository(DbHelper db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Author>> ListAsync()
    {
        using var connection = _db.GetConnection();
        const string sql = "SELECT * FROM Authors ORDER BY AuthorName";
        return await connection.QueryAsync<Author>(sql);
    }

    /// <inheritdoc />
    public async Task<Author?> GetAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = "SELECT * FROM Authors WHERE AuthorId = @id";
        return await connection.QueryFirstOrDefaultAsync<Author>(sql, new { id });
    }

    /// <inheritdoc />
    public async Task<int> AddAsync(Author data)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                INSERT INTO Authors (AuthorName, Biography, BirthDate, DeathDate, IsActive)
                VALUES (@AuthorName, @Biography, @BirthDate, @DeathDate, @IsActive);
                SELECT CAST(SCOPE_IDENTITY() AS int);";
        return await connection.ExecuteScalarAsync<int>(sql, data);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(Author data)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                UPDATE Authors
                SET AuthorName = @AuthorName,
                    Biography = @Biography,
                    BirthDate = @BirthDate,
                    DeathDate = @DeathDate,
                    IsActive = @IsActive
                WHERE AuthorId = @AuthorId";
        return await connection.ExecuteAsync(sql, data) > 0;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = "DELETE FROM Authors WHERE AuthorId = @id";
        return await connection.ExecuteAsync(sql, new { id }) > 0;
    }

    /// <summary>
    /// Kiểm tra tác giả có đang được tham chiếu tại bảng <c>BookAuthors</c> (tác giả đã gắn với sách) hay không.
    /// </summary>
    /// <param name="id">Mã tác giả cần kiểm tra.</param>
    /// <returns><c>true</c> nếu đang được tham chiếu; ngược lại <c>false</c>.</returns>
    public async Task<bool> InUseAsync(int id)
    {
        using var connection = _db.GetConnection();
        const string sql = @"
                SELECT CAST(CASE WHEN
                    EXISTS (SELECT 1 FROM BookAuthors WHERE AuthorId = @id)
                THEN 1 ELSE 0 END AS bit)";
        return await connection.ExecuteScalarAsync<bool>(sql, new { id });
    }

}
