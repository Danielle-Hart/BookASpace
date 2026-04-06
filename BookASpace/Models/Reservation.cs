namespace BookASpace.Models;

public class Reservation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int RoomId { get; set; }
    public string RoomDisplayName { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string? Notes { get; set; }

    public bool HasNotes => !string.IsNullOrWhiteSpace(Notes);
}
