using BookASpace.Data;
using BookASpace.Models;

namespace BookASpace.Services;

public class BookingService
{
    private readonly DatabaseService _database;

    public BookingService(DatabaseService database)
    {
        _database = database;
    }

    public async Task<IReadOnlyList<Reservation>> GetReservationsAsync()
    {
        var db = await _database.GetDatabaseAsync();
        var rows = await db.Table<ReservationRecord>().ToListAsync();
        return rows
            .Select(ToModel)
            .OrderByDescending(r => r.Start)
            .ToList();
    }

    public async Task<(bool Success, string? Error)> TryAddReservationAsync(Reservation reservation)
    {
        if (reservation.End <= reservation.Start)
            return (false, "End time must be after start time.");

        var db = await _database.GetDatabaseAsync();
        var existing = await db.Table<ReservationRecord>().ToListAsync();
        if (HasOverlap(existing, reservation.RoomId, reservation.Start, reservation.End, excludeId: null))
            return (false, "This room is already booked for part of that time.");

        await db.InsertAsync(ToRecord(reservation));
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> TryUpdateReservationAsync(Reservation reservation)
    {
        if (reservation.End <= reservation.Start)
            return (false, "End time must be after start time.");

        var db = await _database.GetDatabaseAsync();
        var existing = await db.Table<ReservationRecord>().ToListAsync();
        var id = reservation.Id.ToString();
        if (!existing.Any(r => r.Id == id))
            return (false, "Reservation was not found.");

        if (HasOverlap(existing, reservation.RoomId, reservation.Start, reservation.End, excludeId: id))
            return (false, "This room is already booked for part of that time.");

        await db.UpdateAsync(ToRecord(reservation));
        return (true, null);
    }

    public async Task<bool> CancelReservationAsync(Guid id)
    {
        var db = await _database.GetDatabaseAsync();
        var n = await db.ExecuteAsync("DELETE FROM reservations WHERE Id = ?", id.ToString());
        return n > 0;
    }

    public async Task<bool> IsRoomFreeAsync(int roomId, DateTime start, DateTime end, Guid? excludeReservationId = null)
    {
        var db = await _database.GetDatabaseAsync();
        var existing = await db.Table<ReservationRecord>().ToListAsync();
        return !HasOverlap(
            existing,
            roomId,
            start,
            end,
            excludeReservationId?.ToString());
    }

    public bool IsRoomFreeAtNow(IReadOnlyList<Reservation> reservations, int roomId, DateTime now)
    {
        return !reservations.Any(r =>
            r.RoomId == roomId &&
            r.Start <= now &&
            r.End >= now);
    }

    private static bool HasOverlap(
        IEnumerable<ReservationRecord> rows,
        int roomId,
        DateTime start,
        DateTime end,
        string? excludeId)
    {
        return rows.Any(r =>
            r.RoomId == roomId &&
            r.Id != excludeId &&
            r.Start < end &&
            r.End > start);
    }

    private static ReservationRecord ToRecord(Reservation r) => new()
    {
        Id = r.Id.ToString(),
        RoomId = r.RoomId,
        RoomDisplayName = r.RoomDisplayName,
        Start = r.Start,
        End = r.End,
        Notes = r.Notes
    };

    private static Reservation ToModel(ReservationRecord r) => new()
    {
        Id = Guid.Parse(r.Id),
        RoomId = r.RoomId,
        RoomDisplayName = r.RoomDisplayName,
        Start = r.Start,
        End = r.End,
        Notes = r.Notes
    };
}
