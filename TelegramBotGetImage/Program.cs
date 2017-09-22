using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;

namespace TelegramBotGetImage
{
    class Program
    {
        static void Main(string[] args)
        {
            TelegramMetod();
            Console.WriteLine("Бот @RandomPhoto_bot работает");
            Console.WriteLine("Eсли вы хотите выключить его нажмите любую клавишу ...");
            Console.ReadKey();
        }

        async static void TelegramMetod()
        {
            string key = "417836710:AAHaiu8stm_QKQBiZX11MJqIcDCrOrTLyMk"; //telegram: @RandomPhoto_bot
            var bot = new Telegram.Bot.TelegramBotClient(key);
            await bot.SetWebhookAsync("");
            int offset = 0;
            while (true)
            {
                var updates = await bot.GetUpdatesAsync(offset); // получаем массив обновлений

                foreach (var update in updates) // Перебираем все обновления
                {
                    var message = update.Message;
                    if (message?.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
                    {
                        string rain = "";
                        if (message.Text == "/start")
                        {
                            await bot.SendTextMessageAsync(message.Chat.Id, "Привет " + message.Chat.FirstName);
                            var keyboard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                            {
                                Keyboard = new[] {
                                    new[] // row 1
                                    {
                                        new Telegram.Bot.Types.KeyboardButton("Пришли картинку"),
                                        new Telegram.Bot.Types.KeyboardButton("Пришли музыку")
                                    },
                                    new[] // row 1
                                    {
                                        new Telegram.Bot.Types.KeyboardButton("Какая погода?"),
                                        new Telegram.Bot.Types.KeyboardButton("Какая погода завтра?")
                                    },
                                },
                                ResizeKeyboard = true
                            };

                            await bot.SendTextMessageAsync(message.Chat.Id, "Выбери что хочешь получить",
                                    ParseMode.Default, false, false, 0, keyboard, CancellationToken.None);
                        }

                        if (message.Text == "/getimage" || message.Text == "Пришли картинку")
                        {
                            string[] dirs = Directory.GetFiles(
                                @"C:\Users\workstation1\Documents\Visual Studio 2015\Projects\TelegramBotGetImage\TelegramBotGetImage\Images");
                            Random rnd = new Random();
                            int i = rnd.Next(0, dirs.Length);
                            string FileUrl = dirs[i];

                            using (var stream = System.IO.File.Open(FileUrl, FileMode.Open))
                            {
                                FileToSend fts = new FileToSend();
                                fts.Content = stream;
                                fts.Filename = FileUrl.Split('\\').Last();
                                await bot.SendPhotoAsync(message.Chat.Id, fts, "New IMG");
                            }
                        }

                        if (message.Text == "/getmusic" || message.Text == "Пришли музыку")
                        {
                            string[] dirs = Directory.GetFiles(
                                @"C:\Users\workstation1\Documents\Visual Studio 2015\Projects\TelegramBotGetImage\TelegramBotGetImage\Music");
                            Random rnd = new Random();
                            int w = rnd.Next(0, dirs.Length);
                            string FileUrl = dirs[w];

                            using (var stream = System.IO.File.Open(FileUrl, FileMode.Open))
                            {
                                FileToSend fts = new FileToSend();
                                fts.Content = stream;
                                fts.Filename = FileUrl.Split('\\').Last();
                                string[] eq = fts.Filename.Split('-');
                                string songName = eq.Last();
                                string athor = "";
                                for (int i = 0; i < eq.Length-1; i++)
                                {
                                    athor += eq[i];
                                }
                                await bot.SendTextMessageAsync(message.Chat.Id, "Отправляю песню: " + athor + " - " + songName + " подождите");
                                await bot.SendAudioAsync(message.Chat.Id, fts, "Файл получен", 0,
                                    athor, songName);
                            }
                        }

                        if (message.Text == "/about")
                        {
                            await bot.SendTextMessageAsync(message.Chat.Id, "Бот написан 22.09.2017\nАвтор: Тимофей Забалуев\nКонтактные данные: zabti@yandex.ru");
                        }

                        if (message.Text == "/weathernow" || message.Text == "Какая погода?")
                        {
                            var q = GetSimpleWeather.WeatherGetNow();
                            //if (q.rain != null) rain = " дождь";
                            await bot.SendTextMessageAsync(message.Chat.Id, "В городе: " + q.Name + " сейчас " + q.Main.temp + " градусов");
                        }

                        if (message.Text == "/weathertomorrow" || message.Text == "Какая погода завтра?")
                        {
                            var w = GetSimpleWeather.WeatherGetFor5Days();
                            await bot.SendTextMessageAsync(message.Chat.Id, "В городе: " + w.city.name);
                            string str = " ";
                            for (int i = 1; i < 9; i++)
                            {
                                if (w.list[i].rain!= null) rain = " дождь";
                                
                                string[] split = w.list[i].dt_txt.Split(' ', ':', '-');
                                if (split[3] == "00")
                                    str += split[2] + "/" + split[1] + " "; //Выводим дополнительно число и месяц если следующий день
                                else str += "---------";
                                str += split[3] + ":00 = " + w.list[i].main.temp + "° " + rain + "\n";
                            }
                            await bot.SendTextMessageAsync(message.Chat.Id, str);
                        }
                    }
                    offset = update.Id + 1;
                }
            }
        }
    }
}
