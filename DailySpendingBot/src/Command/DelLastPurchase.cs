using FinanceService.Controllers;

namespace DailySpendingBot.Command;

public class DelLastPurchase : ICommand
{
	private readonly PurchaseController _purchaseController;
	public string Cmd => "dellast";
	public bool IsAlwaysExecute => false;
	
	public DelLastPurchase(PurchaseController purchaseController)
		=> _purchaseController = purchaseController;
	
	public async Task<string?> Execute(string? message)
	{
		var lastPurchase = await _purchaseController.DeleteLastPurchase();
		if (lastPurchase.Id == Guid.Empty)
			return null;
		
		var dateTime = DateTime.Now;
		var summaryInMonth = await _purchaseController.GetPurchaseInMonth(dateTime.Month, dateTime.Year);
		var summaryInDay = await _purchaseController.GetPurchaseInDay(dateTime.Month, dateTime.Day, dateTime.Year);
		return $"Удалена последняя трата {lastPurchase.Price}RUB. GUID: {lastPurchase.Id} \n"
		       + $"Итого за день: {summaryInDay} \n"
		       + $"Итого за месяц: {summaryInMonth} \n";
	}
}