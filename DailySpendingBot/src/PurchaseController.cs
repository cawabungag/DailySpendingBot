using DailySpendingBot.Entities;
using DailySpendingBot.Repositories;
using FinanceService.DTOs.Purchases;
using FinanceService.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinanceService.Controllers;

[ApiController]
[Route("purchases")]
public class PurchaseController : ControllerBase
{
	private readonly IPurchaseRepository _purchasesRepository;

	public PurchaseController(IPurchaseRepository purchasesRepository)
	{
		_purchasesRepository = purchasesRepository;
	}

	//GET /Purchases
	[HttpGet]
	public async Task<IEnumerable<PurchaseDto>> GetPurchasesAsync()
	{
		var items = (await _purchasesRepository.GetPurchasesAsync())
			.Select(item => item.AsDto());
		return items;
	}

	//GET /Purchases/id
	[HttpGet("{id}")]
	public async Task<ActionResult<PurchaseDto>> GetPurchaseAsync(Guid id)
	{
		var purchase = await _purchasesRepository.GetPurchaseAsync(id);
		if (purchase is null)
		{
			return NotFound();
		}

		return purchase.AsDto();
	}

	//POST /Purchases
	[HttpPost]
	public async Task<ActionResult<PurchaseDto>> CreatePurchaseAsync(CreatePurchaseDto purchaseDto)
	{
		Purchase newPurchase = new()
		{
			Id = Guid.NewGuid(),
			Price = purchaseDto.Price,
			CreatedDate = DateTimeOffset.Now
		};
		await _purchasesRepository.CreatePurchaseAsync(newPurchase);

		return CreatedAtAction("GetPurchase", new {id = newPurchase.Id}, newPurchase.AsDto());
	}

	//PUT /Purchases/{id}
	[HttpPut("{id}")]
	public async Task<ActionResult> UpdatePurchase(Guid id, UpdatePurchaseDto purchaseDto)
	{
		var existingPurchase = await _purchasesRepository.GetPurchaseAsync(id);
		if (existingPurchase is null)
			return NotFound();

		var updatedPurchase = existingPurchase with
		{
			Price = purchaseDto.Price
		};
		await _purchasesRepository.UpdatePurchaseAsync(updatedPurchase);
		return NoContent();
	}

	//DELETE /Purchases/{id}
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeletePurchase(Guid id)
	{
		var existingPurchase = await _purchasesRepository.GetPurchaseAsync(id);
		if (existingPurchase is null)
			return NotFound();

		await _purchasesRepository.DeletePurchaseAsync(id);
		return NoContent();
	}

	public async Task<int> GetPurchaseInMonth(int month, int year)
	{
		var purchasesPrice = 0;
		var existingPurchase = await _purchasesRepository.GetPurchasesAsync();
		foreach (var purchase in existingPurchase)
		{
			if (purchase.CreatedDate.Month != month
			    || purchase.CreatedDate.Year != year)
				continue;

			purchasesPrice += purchase.Price;
		}

		return purchasesPrice;
	}

	public async Task<int> GetPurchaseInDay(int month, int day, int year)
	{
		var purchasesPrice = 0;
		var existingPurchase = await _purchasesRepository.GetPurchasesAsync();
		foreach (var purchase in existingPurchase)
		{
			if (purchase.CreatedDate.Month != month
			    || purchase.CreatedDate.Day != day
			    || purchase.CreatedDate.Year != year)
				continue;

			purchasesPrice += purchase.Price;
		}

		return purchasesPrice;
	}

	public async Task<(int Price, Guid Id)> DeleteLastPurchase()
	{
		var existingPurchase = await _purchasesRepository.GetPurchasesAsync();
		var enumerable = existingPurchase as Purchase[] ?? existingPurchase.ToArray();
		if (!enumerable.Any())
			return (-1, Guid.Empty);
		
		var purchase = enumerable.Last();
		await _purchasesRepository.DeletePurchaseAsync(purchase.Id);
		return (purchase.Price, purchase.Id);
	}

	public async Task Clear()
	{
		var existingPurchase = await _purchasesRepository.GetPurchasesAsync();
		foreach (var purchase in existingPurchase) 
			await DeletePurchase(purchase.Id);
	}
}