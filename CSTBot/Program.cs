using CSTBot;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

TelegramBotClient? Bot;

STLib.Main.Init(@"C:\Users\artem\source\repos\SecureTraffic\identifier.sqlite");

Bot = new TelegramBotClient(Configuration.BotToken);

User me = await Bot.GetMeAsync();
Console.Title = me.Username ?? "My awesome Bot";

using var cts = new CancellationTokenSource();

    // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };
Bot.StartReceiving(Handlers.HandleUpdateAsync,
                       Handlers.HandleErrorAsync,
                       receiverOptions,
                       cts.Token);

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();