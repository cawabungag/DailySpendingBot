using FinanceService.Entities;
using SQLite;

public class DatabaseHandler
{
    public SQLiteConnection Db { get; }

    public DatabaseHandler(string environmentPath)
    {
        var dbPath = Path.Combine(environmentPath, "spending.db");
        Db = new SQLiteConnection(dbPath);
        Db.CreateTable<Purchase>();
    }
}