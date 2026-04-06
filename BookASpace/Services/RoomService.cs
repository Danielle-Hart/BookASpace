using BookASpace.Models;

namespace BookASpace.Services;

public class RoomService
{
    public List<Room> GetRooms()
    {
        return new List<Room>
        {
            new Room{
                Id = 1,
                Building = "Langsam",
                RoomNumber = "204",
                Description = "Quiet Study Room",
                Capacity = 4,
                HasWhiteboard = true,
                HasOutlets = true
            },
            new Room{
                Id = 2,
                Building = "Langsam",
                RoomNumber = "205",
                Description = "Group Study Room",
                Capacity = 6,
                HasWhiteboard = true,
                HasOutlets = true
            },
            new Room{
                Id = 3,
                Building = "CECH",
                RoomNumber = "116",
                Description = "Small Meeting Room",
                Capacity = 8,
                HasWhiteboard = false,
                HasOutlets = true
            },
            new Room{
                Id = 4,
                Building = "Langsam",
                RoomNumber = "Computer Lab 101",
                Description = "Computer Lab with Desktop Stations",
                Capacity = 20,
                HasWhiteboard = true,
                HasOutlets = true,
            },
            new Room{
                Id = 5,
                Building = "Calhoun Hall",
                RoomNumber = "210",
                Description = "Collaborative Study Room with Large Table",
                Capacity = 10,
                HasWhiteboard = true,
                HasOutlets = true,
            },
            new Room{
                Id = 6,
                Building = "Langsam",
                RoomNumber = "365",
                Description = "Silent Individual Study Room",
                Capacity = 2,
                HasWhiteboard = false,
                HasOutlets = true,
             }
        };
    }

    public Room? GetRoomById(int id) =>
        GetRooms().FirstOrDefault(r => r.Id == id);
}