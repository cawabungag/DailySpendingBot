using FinanceService.Entities;

namespace DailySpendingBot.Repositories;

public interface IPurchaseRepository
{
	Task<IEnumerable<Purchase>> GetPurchasesAsync();
	Task<Purchase?> GetPurchaseAsync(Guid id);
	Task CreatePurchaseAsync(Purchase item);
	Task UpdatePurchaseAsync(Purchase item);
	Task DeletePurchaseAsync(Guid id);
}