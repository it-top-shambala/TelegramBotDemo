using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotDemo.App
{
    internal static class Program
    {
        private static readonly ITelegramBotClient Bot = new TelegramBotClient("5297338841:AAHtUVOBQ6RF9m9raeQjrRt-7HB7neiE5rE");

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            
            if(update.Type == UpdateType.Message)
            {
                var message = update.Message;
                
                var command = message?.Text?.ToLower();
                var chatId = message?.Chat;
                
                switch (command)
                {
                    case "/start":
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: message.From.Id.ToString(),
                            parseMode: ParseMode.MarkdownV2,
                            disableNotification: true,
                            replyToMessageId: update.Message.MessageId,
                            replyMarkup: new InlineKeyboardMarkup(
                                InlineKeyboardButton.WithUrl(
                                    "Check sendMessage method",
                                    "https://core.telegram.org/bots/api#sendmessage")),
                            cancellationToken: cancellationToken);
                        break;
                    case "/photo":
                        await botClient.SendPhotoAsync(
                            chatId: chatId,
                            photo: "https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg",
                            caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
                            parseMode: ParseMode.Html,
                            cancellationToken: cancellationToken);
                        break;
                    case "/file":
                        await botClient.SendPhotoAsync(
                            chatId: chatId,
                            photo: new InputOnlineFile("https://cdn.pixabay.com/photo/2017/04/11/21/34/giraffe-2222908_640.jpg"),
                            cancellationToken: cancellationToken
                            );

                        break;
                    default:
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Привет",
                            parseMode: ParseMode.MarkdownV2,
                            cancellationToken: cancellationToken);
                        break;
                }
            }
        }

        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
            return Task.CompletedTask;
        }


        private static void Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + Bot.GetMeAsync().Result.FirstName);
            
            var cancellationToken = new CancellationTokenSource().Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            Bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}