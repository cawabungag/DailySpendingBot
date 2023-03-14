using DailySpendingBot.Command;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;

namespace DailySpendingBot;

public class TelegramBotExecutor
{
	private readonly IEnumerable<ICommand> _commands;
	private const string BOT_TOKEN = "6005535998:AAHGfAWi3rQGRuKhlGhlezozO5Ay5St17jQ";

	public TelegramBotExecutor(IServiceProvider serviceProvider)
	{
		_commands = serviceProvider.GetServices<ICommand>();
		var botClient = new TelegramBotClient(BOT_TOKEN);
		using CancellationTokenSource cts = new();
		ReceiverOptions receiverOptions = new() {AllowedUpdates = Array.Empty<UpdateType>()};

		botClient.StartReceiving(
			HandleUpdateAsync,
			HandlePollingErrorAsync,
			receiverOptions,
			cts.Token
		);

		Thread.Sleep(Timeout.Infinite);

		cts.Cancel();

		async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			var message = update.ChannelPost;
			if (message == null)
				return;
			
			var messageText = message.Text;
			var chatId = message.Chat.Id;
			// Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
			await Execute(messageText, botClient, cancellationToken, chatId);
		}

		Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
		{
			var ErrorMessage = exception switch
			{
				ApiRequestException apiRequestException
					=> $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
				_ => exception.ToString()
			};

			// Console.WriteLine(ErrorMessage);
			return Task.CompletedTask;
		}
	}

	private async Task Execute(string? message, ITelegramBotClient botClient, CancellationToken cancellationToken,
		long chatId)
	{
		foreach (var command in _commands)
		{
			if (command.Cmd != message && !command.IsAlwaysExecute)
				continue;

			var response = await command.Execute(message);
			if (string.IsNullOrEmpty(response))
				continue;

			await botClient.SendTextMessageAsync(chatId, response, cancellationToken: cancellationToken);
		}
	}
}