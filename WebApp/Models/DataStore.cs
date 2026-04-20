namespace WebApp.Models;

//Fake Data

public static class DataStore
{
    public static List<Room> Rooms = new List<Room>
    {
        new Room { Id = 1, Name = "Room A", BuildingCode = "A", Floor = 1, Capacity = 20, HasProjector = true, IsActive = true },
        new Room { Id = 2, Name = "Room B", BuildingCode = "A", Floor = 2, Capacity = 30, HasProjector = false, IsActive = true },
        new Room { Id = 3, Name = "Room C", BuildingCode = "B", Floor = 1, Capacity = 15, HasProjector = true, IsActive = false },
        new Room { Id = 4, Name = "Room D", BuildingCode = "B", Floor = 3, Capacity = 40, HasProjector = true, IsActive = true }
    };

    public static List<Reservation> Reservations = new List<Reservation>
    {
        new Reservation
        {
            Id = 1,
            RoomId = 1,
            OrganizerName = "John Doe",
            Topic = "C# Basics",
            Date = new DateTime(2026, 5, 10),
            StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(12, 0, 0),
            Status = "confirmed"
        }
    };
}