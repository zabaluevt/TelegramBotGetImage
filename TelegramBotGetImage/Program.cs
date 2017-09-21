using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBotGetImage
{
    class Program
    {
        static void Main(string[] args)
        {
            TelegramMetod();
            Console.WriteLine();
            Console.ReadKey();
        }

        async static void TelegramMetod()
        {
            string key = "417836710:AAHaiu8stm_QKQBiZX11MJqIcDCrOrTLyMk";
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
                        if (message.Text == "/saysomething")
                        {
                            // в ответ на команду /saysomething выводим сообщение
                            await bot.SendTextMessageAsync(message.Chat.Id, "Вы сказали",
                                replyToMessageId: message.MessageId);
                        }

                        if (message.Text == "/getimage")
                        {
                            // в ответ на команду /getimage выводим картинку
                            string[] dirs = Directory.GetFiles(@"C:\Users\workstation1\Documents\Visual Studio 2015\Projects\TelegramBot_WPF\TelegramBot_WPF\Images");
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
                        //https://tlgrm.ru/docs/bots/api читать тут
                        if (message.Text == "/getmusic")
                        {
                            //string[] dirs = Directory.GetFiles(@"C:\Users\workstation1\Documents\Visual Studio 2015\Projects\TelegramBot_WPF\TelegramBot_WPF\Music");
                            //Random rnd = new Random();
                            //int i = rnd.Next(0, dirs.Length);
                            //string FileUrl = dirs[i];

                            //using (var stream = System.IO.File.Open(FileUrl, FileMode.Open))
                            //{
                            //    FileToSend fts = new FileToSend();
                            //    fts.Content = stream;
                            //    await bot.SendAudioAsync(message.Chat.Id, fts, "New song", 1000, "qwe", "Song is");
                            //}
                            await bot.SendTextMessageAsync(message.Chat.Id, "К сожалению пока не работает");
                        }
                    }
                    offset = update.Id + 1;
                }
            }
        }
    }
}
