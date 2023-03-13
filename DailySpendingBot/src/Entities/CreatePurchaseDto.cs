using System.ComponentModel.DataAnnotations;

namespace FinanceService.DTOs.Purchases
{
	public class CreatePurchaseDto
	{
		[Required] 
		public int Price { get; init; }
	}
}