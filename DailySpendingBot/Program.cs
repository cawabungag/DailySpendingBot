using DailySpendingBot;
using DailySpendingBot.Repositories;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var contentRootPath = builder.Environment.ContentRootPath;
var database = new DatabaseHandler(contentRootPath);
services.AddSingleton(database);
services.AddSingleton<IPurchaseRepository, PurchaseRepository>();
var app = builder.Build();
var login = new LoginTest();
await login.Login();
app.Run();


