using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using STLib.User;

namespace CSTBot
{
    public class Handlers
    {
        private static int step = -1;
        private static bool isAuth = false;
        private static List<Message> messages = new List<Message>();

        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                UpdateType.Message => BotOnMessageReceived(botClient, update.Message!),
                //UpdateType.EditedMessage => BotOnMessageReceived(botClient, update.EditedMessage!),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(botClient, update.CallbackQuery!),
                UpdateType.InlineQuery => BotOnInlineQueryReceived(botClient, update.InlineQuery!),
                UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult!),
                _ => UnknownUpdateHandlerAsync(botClient, update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }

        private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            Console.WriteLine($"Receive message type: {message.Type}");
            if (message.Type != MessageType.Text)
                return;

            var action = message.Text!.Split(' ')[0] switch
            {
                /*"/inline" => SendInlineKeyboard(botClient, message),
                "/keyboard" => SendReplyKeyboard(botClient, message),
                "/remove" => RemoveKeyboard(botClient, message),
                "/photo" => SendFile(botClient, message),
                "/request" => RequestContactAndLocation(botClient, message),*/
                _ => HandlerMessages(botClient, message)
            };
            Message sentMessage = await action;
            Console.WriteLine($"The message was sent with id: {sentMessage.MessageId}");
            

            static async Task<Message> StartBot(ITelegramBotClient botClient, Message message)
            {
                if (!await Manage.CheckExists(message.From.Id))
                {
                    step = 0;
                    isAuth = true;
                    var ret = await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Давайте пройдем регистрацию!", replyMarkup: new ReplyKeyboardRemove());
                    return await StepAuth(botClient, message);
                }
                else
                    return await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты уже есть в системе", replyMarkup: new ReplyKeyboardRemove());
            }

            static async Task<Message> HandlerMessages(ITelegramBotClient botClient, Message message)
            {
                if (!await Manage.CheckExists(message.From.Id) && !isAuth)
                {
                    step = 0;
                    isAuth = true;
                    var ret = await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Давайте пройдем регистрацию!", replyMarkup: new ReplyKeyboardRemove());
                    return await StepAuth(botClient, message);
                }
                else if (isAuth)
                    return await StepAuth(botClient, message);
                else
                    return await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: step++.ToString(), replyMarkup: new ReplyKeyboardRemove());
            }

            static async Task<Message> StepAuth(ITelegramBotClient botClient, Message message)
            {
                Message retVal;
                switch (step)
                {
                    case 0:
                        messages.Add(retVal = await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Введите логин:", replyMarkup: new ReplyKeyboardRemove()));
                        step++;
                        return retVal;
                    case 1:
                        messages.Add(retVal = message);
                        step++;
                        messages.Add(await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Введите пароль:", replyMarkup: new ReplyKeyboardRemove()));
                        return retVal;
                    case 2:
                        messages.Add(retVal = message);
                        retVal = await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: (await Manage.Register(messages[1].Chat.Id, messages[1].Text, messages[3].Text)) ? "Успех!" : "Ошибка регистрации!", replyMarkup: new ReplyKeyboardRemove());
                        for (int i = 0; i < messages.Count; i++)
                            await botClient.DeleteMessageAsync(chatId: messages[i].Chat.Id, messageId: messages[i].MessageId);

                        messages.Clear();
                        isAuth = false;
                        return retVal;
                    default:
                        return null;
                }
            }

        }

        // Process Inline Keyboard callback data
        private static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}");

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Received {callbackQuery.Data}");
        }

        private static async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            Console.WriteLine($"Received inline query from: {inlineQuery.From.Id}");

            InlineQueryResult[] results = {
            // displayed result
            new InlineQueryResultArticle(
                id: "3",
                title: "TgBots",
                inputMessageContent: new InputTextMessageContent(
                    "hello"
                )
            )
        };

            await botClient.AnswerInlineQueryAsync(inlineQueryId: inlineQuery.Id,
                                                   results: results,
                                                   isPersonal: true,
                                                   cacheTime: 0);
        }

        private static Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResult.ResultId}");
            return Task.CompletedTask;
        }

        private static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            Console.WriteLine($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }
    }
}
