using System.ComponentModel.DataAnnotations;

namespace FinanceService.DTOs.Purchases
{
	public class CreatePurchaseDto
	{
		public CreatePurchaseDto(int price) => Price = price;

		[Required] 
		public int Price { get; init; }
	}
}