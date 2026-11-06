namespace SLib23T1020224.Models.Constants;

/// <summary>
/// Tập hợp các hằng giá trị được sử dụng trong các cột của cơ sở dữ liệu
/// (theo các ràng buộc CHECK và giá trị mặc định). Tên hằng có dạng <c>TENBANG_TENCOT_GIATRI</c>.
/// </summary>
public static class DatabaseConstants
{
    /// <summary>
    /// Sách mới. Giá trị của tình trạng vật lý của bản sao sách (bảng <c>BookCopies</c>, cột <c>CopyCondition</c>).
    /// </summary>
    public const string BOOKCOPIES_COPYCONDITION_NEW = "New";

    /// <summary>
    /// Sách còn tốt (giá trị mặc định). Giá trị của tình trạng vật lý của bản sao sách (bảng <c>BookCopies</c>, cột <c>CopyCondition</c>).
    /// </summary>
    public const string BOOKCOPIES_COPYCONDITION_GOOD = "Good";

    /// <summary>
    /// Sách ở mức trung bình, có dấu hiệu sử dụng. Giá trị của tình trạng vật lý của bản sao sách (bảng <c>BookCopies</c>, cột <c>CopyCondition</c>).
    /// </summary>
    public const string BOOKCOPIES_COPYCONDITION_FAIR = "Fair";

    /// <summary>
    /// Sách bị hư hỏng. Giá trị của tình trạng vật lý của bản sao sách (bảng <c>BookCopies</c>, cột <c>CopyCondition</c>).
    /// </summary>
    public const string BOOKCOPIES_COPYCONDITION_DAMAGED = "Damaged";

    /// <summary>
    /// Bản sao sẵn sàng cho mượn (giá trị mặc định). Giá trị của trạng thái của bản sao sách (bảng <c>BookCopies</c>, cột <c>CopyStatus</c>).
    /// </summary>
    public const string BOOKCOPIES_COPYSTATUS_AVAILABLE = "Available";

    /// <summary>
    /// Bản sao đang được mượn. Giá trị của trạng thái của bản sao sách (bảng <c>BookCopies</c>, cột <c>CopyStatus</c>).
    /// </summary>
    public const string BOOKCOPIES_COPYSTATUS_ONLOAN = "OnLoan";

    /// <summary>
    /// Bản sao đã được đặt trước. Giá trị của trạng thái của bản sao sách (bảng <c>BookCopies</c>, cột <c>CopyStatus</c>).
    /// </summary>
    public const string BOOKCOPIES_COPYSTATUS_RESERVED = "Reserved";

    /// <summary>
    /// Bản sao bị mất. Giá trị của trạng thái của bản sao sách (bảng <c>BookCopies</c>, cột <c>CopyStatus</c>).
    /// </summary>
    public const string BOOKCOPIES_COPYSTATUS_LOST = "Lost";

    /// <summary>
    /// Bản sao đã bị rút khỏi kho (thanh lý, loại bỏ). Giá trị của trạng thái của bản sao sách (bảng <c>BookCopies</c>, cột <c>CopyStatus</c>).
    /// </summary>
    public const string BOOKCOPIES_COPYSTATUS_WITHDRAWN = "Withdrawn";

    /// <summary>
    /// Đang mượn, chưa trả (giá trị mặc định). Giá trị của trạng thái của dòng chi tiết phiếu mượn (bảng <c>LoanItems</c>, cột <c>ItemStatus</c>).
    /// </summary>
    public const string LOANITEMS_ITEMSTATUS_BORROWED = "Borrowed";

    /// <summary>
    /// Đã trả sách. Giá trị của trạng thái của dòng chi tiết phiếu mượn (bảng <c>LoanItems</c>, cột <c>ItemStatus</c>).
    /// </summary>
    public const string LOANITEMS_ITEMSTATUS_RETURNED = "Returned";

    /// <summary>
    /// Sách bị mất. Giá trị của trạng thái của dòng chi tiết phiếu mượn (bảng <c>LoanItems</c>, cột <c>ItemStatus</c>).
    /// </summary>
    public const string LOANITEMS_ITEMSTATUS_LOST = "Lost";

    /// <summary>
    /// Sách còn mới khi trả. Giá trị của tình trạng sách khi trả (bảng <c>LoanItems</c>, cột <c>ReturnCondition</c>).
    /// </summary>
    public const string LOANITEMS_RETURNCONDITION_NEW = "New";

    /// <summary>
    /// Sách còn tốt khi trả. Giá trị của tình trạng sách khi trả (bảng <c>LoanItems</c>, cột <c>ReturnCondition</c>).
    /// </summary>
    public const string LOANITEMS_RETURNCONDITION_GOOD = "Good";

    /// <summary>
    /// Sách ở mức trung bình khi trả. Giá trị của tình trạng sách khi trả (bảng <c>LoanItems</c>, cột <c>ReturnCondition</c>).
    /// </summary>
    public const string LOANITEMS_RETURNCONDITION_FAIR = "Fair";

    /// <summary>
    /// Sách bị hư hỏng khi trả. Giá trị của tình trạng sách khi trả (bảng <c>LoanItems</c>, cột <c>ReturnCondition</c>).
    /// </summary>
    public const string LOANITEMS_RETURNCONDITION_DAMAGED = "Damaged";

    /// <summary>
    /// Phiếu mượn đang hoạt động (giá trị mặc định). Giá trị của trạng thái của phiếu mượn (bảng <c>Loans</c>, cột <c>LoanStatus</c>).
    /// </summary>
    public const string LOANS_LOANSTATUS_ACTIVE = "Active";

    /// <summary>
    /// Phiếu mượn đã hoàn tất. Giá trị của trạng thái của phiếu mượn (bảng <c>Loans</c>, cột <c>LoanStatus</c>).
    /// </summary>
    public const string LOANS_LOANSTATUS_COMPLETED = "Completed";

    /// <summary>
    /// Phiếu mượn đã bị hủy. Giá trị của trạng thái của phiếu mượn (bảng <c>Loans</c>, cột <c>LoanStatus</c>).
    /// </summary>
    public const string LOANS_LOANSTATUS_CANCELLED = "Cancelled";

    /// <summary>
    /// Nam. Giá trị của giới tính của độc giả (bảng <c>Members</c>, cột <c>Gender</c>).
    /// </summary>
    public const string MEMBERS_GENDER_MALE = "Male";

    /// <summary>
    /// Nữ. Giá trị của giới tính của độc giả (bảng <c>Members</c>, cột <c>Gender</c>).
    /// </summary>
    public const string MEMBERS_GENDER_FEMALE = "Female";

    /// <summary>
    /// Khác. Giá trị của giới tính của độc giả (bảng <c>Members</c>, cột <c>Gender</c>).
    /// </summary>
    public const string MEMBERS_GENDER_OTHER = "Other";

    /// <summary>
    /// Thẻ đang hoạt động (giá trị mặc định). Giá trị của trạng thái thẻ của độc giả (bảng <c>Members</c>, cột <c>MemberStatus</c>).
    /// </summary>
    public const string MEMBERS_MEMBERSTATUS_ACTIVE = "Active";

    /// <summary>
    /// Thẻ bị tạm khóa. Giá trị của trạng thái thẻ của độc giả (bảng <c>Members</c>, cột <c>MemberStatus</c>).
    /// </summary>
    public const string MEMBERS_MEMBERSTATUS_SUSPENDED = "Suspended";

    /// <summary>
    /// Thẻ đã hết hạn. Giá trị của trạng thái thẻ của độc giả (bảng <c>Members</c>, cột <c>MemberStatus</c>).
    /// </summary>
    public const string MEMBERS_MEMBERSTATUS_EXPIRED = "Expired";

    /// <summary>
    /// Thẻ đã đóng. Giá trị của trạng thái thẻ của độc giả (bảng <c>Members</c>, cột <c>MemberStatus</c>).
    /// </summary>
    public const string MEMBERS_MEMBERSTATUS_CLOSED = "Closed";
}
