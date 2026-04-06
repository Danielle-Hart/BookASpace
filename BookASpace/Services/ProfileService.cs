using BookASpace.Data;
using BookASpace.Models;

namespace BookASpace.Services;

public class ProfileService
{
    private const int ProfileRowId = 1;
    private readonly DatabaseService _database;

    public ProfileService(DatabaseService database)
    {
        _database = database;
    }

    public async Task<UserProfile> GetProfileAsync()
    {
        var db = await _database.GetDatabaseAsync();
        var row = await db.Table<UserProfileRecord>()
            .Where(u => u.RowId == ProfileRowId)
            .FirstOrDefaultAsync();

        if (row == null)
            return new UserProfile();

        return new UserProfile
        {
            MNumber = row.MNumber ?? string.Empty,
            UcEmail = row.UcEmail ?? string.Empty
        };
    }

    public async Task SaveProfileAsync(UserProfile profile)
    {
        var db = await _database.GetDatabaseAsync();
        var row = new UserProfileRecord
        {
            RowId = ProfileRowId,
            MNumber = profile.MNumber.Trim(),
            UcEmail = profile.UcEmail.Trim()
        };
        await db.InsertOrReplaceAsync(row);
    }
}
