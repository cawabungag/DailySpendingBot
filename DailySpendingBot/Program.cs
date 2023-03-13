using DailySpendingBot;
using DailySpendingBot.Command;
using DailySpendingBot.Repositories;
using FinanceService.Controllers;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var contentRootPath = builder.Environment.ContentRootPath;
services.AddSingleton<PurchaseController>();
services.AddSingleton(new DatabaseHandler(contentRootPath));
services.AddSingleton<IPurchaseRepository, PurchaseRepository>();
services.AddSingleton<ICommand, AddPurchaseCommand>();
services.AddSingleton<ICommand, DelLastPurchase>();
services.AddSingleton<ICommand, ClearPurchasesCommand>();
var app = builder.Build();
var serviceProvider = services.BuildServiceProvider();
var telegramBotExecutor = new TelegramBotExecutor(serviceProvider);
app.Run();


