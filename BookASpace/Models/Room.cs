namespace BookASpace.Models;

public class Room
{
    public int Id { get; set; }
    public string Building { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public bool HasWhiteboard { get; set; }
    public bool HasOutlets { get; set; }

    public string DisplayName =>
        string.IsNullOrWhiteSpace(Building)
            ? RoomNumber
            : $"{Building} {RoomNumber}";

    public string WhiteboardSummary => HasWhiteboard ? "Whiteboard: Yes" : "Whiteboard: No";

    public string OutletsSummary => HasOutlets ? "Outlets: Yes" : "Outlets: No";
}