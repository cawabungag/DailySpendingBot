using System.ComponentModel.DataAnnotations;

namespace DailySpendingBot.Entities;

public class UpdatePurchaseDto
{
	[Required] 
	public int Price { get; init; }
}