using SQLite;

namespace FinanceService.Entities;

public record Purchase
{
	[PrimaryKey, AutoIncrement]
	[Column("id")]	
	public Guid Id { get; init; }
	
	[Column("created_date")]	
	public DateTimeOffset CreatedDate { get; init; }
	
	[Column("price")]	
	public int Price { get; init; }
}