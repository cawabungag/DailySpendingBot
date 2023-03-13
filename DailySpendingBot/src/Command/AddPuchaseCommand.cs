using FinanceService.Controllers;
using FinanceService.DTOs.Purchases;

namespace DailySpendingBot.Command;

public class AddPurchaseCommand : ICommand
{
	private readonly PurchaseController _purchaseController;

	public AddPurchaseCommand(PurchaseController purchaseController)
		=> _purchaseController = purchaseController;

	public string Cmd => "AddPurchaseCommand";
	public bool IsAlwaysExecute => true;

	public async Task<string?> Execute(string? message)
	{
		var digit = string.Empty;

		if (message == null)
			return null;

		for (int i = 0; i < message.Length; i++)
		{
			if (char.IsDigit(message[i]))
				digit += message[i];
		}

		if (digit.Length <= 0)
			return null;

		if (!int.TryParse(digit, out var value))
			return null;

		await _purchaseController.CreatePurchaseAsync(new CreatePurchaseDto(value));
		var dateTime = DateTime.Now;
		var summaryInMonth = await _purchaseController.GetPurchaseInMonth(dateTime.Month, dateTime.Year);
		var summaryInDay = await _purchaseController.GetPurchaseInDay(dateTime.Month, dateTime.Day, dateTime.Year);
		return $"ПОТРАЧЕНО {value}RUB. \n"
		       + $"Итого за день: {summaryInDay} \n"
		       + $"Итого за месяц: {summaryInMonth} \n";
	}
}