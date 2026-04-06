using SQLite;

namespace BookASpace.Data;

[Table("user_profile")]
public class UserProfileRecord
{
    [PrimaryKey]
    public int RowId { get; set; } = 1;

    public string MNumber { get; set; } = string.Empty;
    public string UcEmail { get; set; } = string.Empty;
}
