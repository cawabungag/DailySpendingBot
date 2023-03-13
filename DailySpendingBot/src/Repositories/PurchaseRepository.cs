using FinanceService.Entities;
using SQLite;

namespace DailySpendingBot.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
	private const string TABLE_NAME = "purchase";
	private readonly SQLiteConnection _sqLite;

	public PurchaseRepository(DatabaseHandler databaseHandler)
	{
		_sqLite = databaseHandler.Db;
	}
    
	public async Task<IEnumerable<Purchase>> GetPurchasesAsync()
	{
		var result = _sqLite.Query<Purchase>($"SELECT * FROM {TABLE_NAME}");
		return await Task.FromResult(result);
	}

	public async Task<Purchase?> GetPurchaseAsync(Guid id)
	{
		var result = GetPurchasesAsync().Result.FirstOrDefault(x => x.Id == id);
		return await Task.FromResult(result);
	}

	public async Task CreatePurchaseAsync(Purchase item)
	{
		_sqLite.Insert(item);
		await Task.CompletedTask;
	}

	public async Task UpdatePurchaseAsync(Purchase item)
	{
		_sqLite.Update(item);
		await Task.CompletedTask;
	}

	public async Task DeletePurchaseAsync(Guid id)
	{
		_sqLite.Delete<Purchase>(id);
		await Task.CompletedTask;
	}
}