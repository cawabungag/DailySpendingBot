using DailySpendingBot;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var login = new LoginTest();
await login.Login();
app.Run();


