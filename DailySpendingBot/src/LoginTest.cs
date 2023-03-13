using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DailySpendingBot;
using Telegram.Bot;

public class LoginTest
{
	private const string BOT_TOKEN = "6005535998:AAHGfAWi3rQGRuKhlGhlezozO5Ay5St17jQ";
	public async Task Login()
	{
		var botClient = new TelegramBotClient(BOT_TOKEN);
		using CancellationTokenSource cts = new ();

		// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
		ReceiverOptions receiverOptions = new ()
		{
			AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
		};

		botClient.StartReceiving(
			updateHandler: HandleUpdateAsync,
			pollingErrorHandler: HandlePollingErrorAsync,
			receiverOptions: receiverOptions,
			cancellationToken: cts.Token
		);

		Console.ReadLine();

		cts.Cancel();

		async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			var message = update.ChannelPost;
			var messageText = message.Text;
			var chatId = message.Chat.Id;
			
			Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
			var asd = ParseMessage(messageText);

			if (asd != 0)
			{
				Message sentMessage = await botClient.SendTextMessageAsync(
					chatId: chatId,
					text: "Вы потратили:\n" + $"{asd}RUB. \n" + $"Итого за день: {dayli}RUB.",
					cancellationToken: cancellationToken);
			}
		}
		

		Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
		{
			var ErrorMessage = exception switch
			{
				ApiRequestException apiRequestException
					=> $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
				_ => exception.ToString()
			};

			Console.WriteLine(ErrorMessage);
			return Task.CompletedTask;
		}
	}

	private int dayli;

	private int ParseMessage(string message)
	{
		string b = string.Empty;
		int val;

		if (message == null)
		{
			return 0;
		}
		for (int i=0; i< message.Length; i++)
		{
			if (Char.IsDigit(message[i]))
				b += message[i];
		}

		if (b.Length > 0)
		{
			if (int.TryParse(b, out var value))
			{
				dayli += value;
				return value;
			}
		}

		return 0;
	}
}