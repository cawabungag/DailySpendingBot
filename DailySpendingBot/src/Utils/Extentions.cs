using FinanceService.Entities;

namespace FinanceService;

public static class Extentions
{
	public static PurchaseDto AsDto(this Purchase purchase) =>
		new()
		{
			Id = purchase.Id,
			Price = purchase.Price,
			CreatedDate = purchase.CreatedDate
		};
}