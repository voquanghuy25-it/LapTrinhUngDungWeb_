using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SLib23T1020224.DataLayer.SQLServer;

/// <summary>
/// Cung cấp kết nối tới CSDL SQL Server cho các lớp cài đặt repository.
/// </summary>
public class DbHelper
{
    private readonly string _connectionString;

    /// <summary>
    /// Khởi tạo, đọc chuỗi kết nối "DefaultConnection" từ cấu hình.
    /// </summary>
    /// <param name="configuration">Cấu hình của ứng dụng.</param>
    public DbHelper(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    /// <summary>
    /// Tạo một kết nối mới (chưa mở) tới CSDL.
    /// </summary>
    /// <returns>Đối tượng <see cref="SqlConnection"/>.</returns>
    public SqlConnection GetConnection() => new SqlConnection(_connectionString);
}
