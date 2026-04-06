using BookASpace.Data;
using SQLite;

namespace BookASpace.Services;

public sealed class DatabaseService
{
    private const string DatabaseFileName = "bookaspace.db3";
    private SQLiteAsyncConnection? _database;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    public async Task<SQLiteAsyncConnection> GetDatabaseAsync()
    {
        await _initLock.WaitAsync();
        try
        {
            if (_database != null)
                return _database;

            SQLitePCL.Batteries_V2.Init();
            var path = Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName);
            _database = new SQLiteAsyncConnection(path);
            await _database.CreateTableAsync<ReservationRecord>();
            await _database.CreateTableAsync<UserProfileRecord>();
            return _database;
        }
        finally
        {
            _initLock.Release();
        }
    }
}
