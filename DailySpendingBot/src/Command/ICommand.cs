namespace DailySpendingBot.Command;

public interface ICommand
{
	string Cmd { get; }
	bool IsAlwaysExecute { get; }
	Task<string?> Execute(string? message);
}