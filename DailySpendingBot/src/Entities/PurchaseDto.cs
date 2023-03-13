namespace FinanceService.Entities;

public class PurchaseDto
{
	public Guid Id { get; init; }
	public DateTimeOffset CreatedDate { get; init; }
	public int Price { get; init; }
}