using SQLite;

namespace BookASpace.Data;

[Table("reservations")]
public class ReservationRecord
{
    [PrimaryKey]
    public string Id { get; set; } = string.Empty;

    public int RoomId { get; set; }
    public string RoomDisplayName { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string? Notes { get; set; }
}
