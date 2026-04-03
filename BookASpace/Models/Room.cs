namespace BookASpace.Models;

public class Room
{
    public int Id { get; set; }
    public string Building { get; set; }
    public string RoomNumber { get; set; }
    public string Description { get; set; }
    public int Capacity { get; set; }
    public bool HasWhiteboard { get; set; }
    public bool HasOutlets { get; set; }

    public string DisplayName => $"{Building} {RoomNumber}";
}