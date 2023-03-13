using FinanceService.Controllers;

namespace DailySpendingBot.Command;

public class ClearPurchasesCommand : ICommand
{
	private readonly PurchaseController _purchaseController;
	public string Cmd => "clear";
	public bool IsAlwaysExecute => false;

	public ClearPurchasesCommand(PurchaseController purchaseController) 
		=> _purchaseController = purchaseController;

	public async Task<string?> Execute(string? message)
	{
		await _purchaseController.Clear();
		return "CLEAR ALL DATA";
	}
}