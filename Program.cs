using static System.Runtime.InteropServices.JavaScript.JSType;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using ZstdSharp.Unsafe;
using TestMaryBot.Keyboards;
using TestMaryBot.UserBase.Regular;
using TestMaryBot.UserBase.Administrator;
using TestMaryBot.Forms;
namespace TestMaryBot
{
    internal class Program
    {
        private static readonly string BotApiKey = "TOKEN";
        private static DatabaseManager manager = new DatabaseManager();
        private static KeyboardsMarkup? _markups;
        private static UserBase.User? user;
        private static RegularUserRegForm regularUserRegForm = new RegularUserRegForm();
        private static AdminAddForm? adminAddForm;
        private static EditDataUserForm? editDataUserForm;
        private static ProductAddForm? productAddForm;
        private static long chatId;

        static async Task Main(string[] args)
        {
            var client = new TelegramBotClient(BotApiKey);

            client.StartReceiving(Update, Error);

            Console.ReadLine();

        }

        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;
            var username = message.Chat.Username;
            if (message.Chat != null)
            {
                chatId = message.Chat.Id;
                if (_markups == null)
                {
                    _markups = new KeyboardsMarkup();
                }
                if (manager.CheckRecordExists(chatId) || (manager.CheckRecordExists(username) && username != null))
                {
                    string? status = manager.GetStatusById(chatId);
                    if (status == null && username != null)
                    {
                        status = manager.GetStatusByUsername(username);
                        manager.UpdateData("id", chatId.ToString(), 0, username);
                        //отправить в бд айди
                    }
                    if (status == "админ")
                        user = new Admin();
                    else
                        user = new RegularUser();

                    user.ChatId = chatId;
                    user.Username = username;
                    manager.GetUserData(user); //получаем данные юзера

                    if (status == "админ")
                    {
                        if (message.Text == "/start")
                        {
                            await botClient.SendTextMessageAsync(chatId, $"{user.Greeting()}");
                        }
                        else if (message.Text == "/addadmin")
                        {
                            adminAddForm = new AdminAddForm(user);
                            var msg = adminAddForm.StageText();
                            await botClient.SendTextMessageAsync(chatId, msg);
                            if (message.Text == "/addadmin")
                                adminAddForm.stage = 2;
                            /*else if (message.Text == "Назад" || message.Text == "Прервать")
                                adminAddForm = null; 
                            else
                                adminAddForm.SetParam(message.Text);*/
                        }
                        else if (adminAddForm != null)
                        {
                            var msg = adminAddForm.StageText();
                            if (message.Text == "Назад")
                            {

                                if (adminAddForm.stage == 2)
                                {
                                    adminAddForm = null;
                                    await botClient.SendTextMessageAsync(chatId, "Добавление админа прекращено. Возвращаю тебя в начало");
                                }
                                else
                                {
                                    adminAddForm.stage -= 2;
                                    msg = adminAddForm.StageText();
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyBackKeyboard);
                                    adminAddForm.stage++;
                                    msg = adminAddForm.StageText();
                                }
                            }
                            else if (message.Text == "Прекратить")
                                adminAddForm = null;
                            else if (adminAddForm.stage == 3)
                            {
                                adminAddForm.SetParam(message.Text);
                                string s = manager.CheckPresenceAdmin(adminAddForm.admin.Name, adminAddForm.admin.Username);
                                if (s != " ")
                                {
                                    await botClient.SendTextMessageAsync(chatId, "Админ с такими именем и юзернеймом уже существуют. Ниже будут его данные, а ты направишься в начало. \nЧтобы заново попытаться добавить админа, пришли мне команду /addadmin");
                                    await botClient.SendTextMessageAsync(chatId, s);
                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyBackKeyboard);
                                }
                            }
                            else if (adminAddForm.stage == 4)
                            {
                                adminAddForm.SetParam(message.Text);
                                await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyCountriesKeyboard);
                            }
                            else if (adminAddForm.stage == 5)
                            {
                                if (message.Text == "🇷🇺Россия" || message.Text == "🌐Другое")
                                {
                                    adminAddForm.SetParam(message.Text);
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: new ReplyKeyboardRemove());
                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(chatId, "Пожалуйста, выбери один из варинатов на кнопках.", replyMarkup: _markups.ReplyCountriesKeyboard);
                                } //если не по кнопке
                            }
                            else if (adminAddForm.stage == 7)
                            {
                                await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyBackKeyboard);
                                adminAddForm.SetParam(message.Text);
                                adminAddForm = null;
                            }
                            else
                            {
                                await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyBackKeyboard);
                                /* if (message.Text == "/addadmin")
                                     adminAddForm.stage = 2;
                                 else*/
                                adminAddForm.SetParam(message.Text);
                            }
                        }
                        else if (message.Text == "/edit")
                        {
                            editDataUserForm = new EditDataUserForm(user);
                            var msg = editDataUserForm.StageText();
                            await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyAllParametersKeyboard);
                            if (message.Text == "/edit")
                                editDataUserForm.stage = 2;
                            else
                                editDataUserForm.SetParam(message.Text);

                        }
                        else if (editDataUserForm != null)
                        {
                            var msg = editDataUserForm.StageText();
                            if (message.Text == "Назад")
                            {

                                if (editDataUserForm.stage == 2)
                                {
                                    editDataUserForm = null;
                                    await botClient.SendTextMessageAsync(chatId, "Изменение данных прекращено. Возвращаю тебя в начало");
                                }
                                else
                                {
                                    editDataUserForm.stage -= 2;
                                    msg = editDataUserForm.StageText();
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyBackKeyboard);
                                    editDataUserForm.stage++;
                                    msg = editDataUserForm.StageText();
                                }
                            }
                            else if (message.Text == "Прекратить")
                                editDataUserForm = null;
                            else if (editDataUserForm.stage == 2)
                            {
                                if (message.Text == "Страна")
                                {
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyCountriesKeyboard);
                                    editDataUserForm.SetParam(message.Text);

                                }
                                else if (message.Text == "Пол")
                                {
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyGenderKeyboard);
                                    editDataUserForm.SetParam(message.Text);


                                }
                                else if (message.Text == "Номер телефона")
                                {
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplySendPhoneKeyboard);
                                    editDataUserForm.SetParam(message.Text);
                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyBackKeyboard);
                                    editDataUserForm.SetParam(message.Text);

                                }
                            }
                            else if (editDataUserForm.stage == 3)
                            {
                                if (editDataUserForm.field == "Страна")
                                {
                                    if (message.Text == "🇷🇺Россия" || message.Text == "🌐Другое")
                                    {
                                        editDataUserForm.SetParam(message.Text);
                                        await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: new ReplyKeyboardRemove());
                                        editDataUserForm = null;
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(chatId, "Пожалуйста, выберите один из варинатов на кнопках.", replyMarkup: _markups.ReplyCountriesKeyboard);
                                    } //если не по кнопке
                                }
                                else if (editDataUserForm.field == "Пол")
                                {
                                    if (message.Text == "🤵‍♀️Женщина" || message.Text == "🤵‍♂️Мужчина")
                                    {
                                        editDataUserForm.SetParam(message.Text);
                                        await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: new ReplyKeyboardRemove());
                                        editDataUserForm = null;
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(chatId, "Пожалуйста, выберите один из варинатов на кнопках.", replyMarkup: _markups.ReplyGenderKeyboard);
                                    } //если не по кнопке

                                }
                                else if (editDataUserForm.field == "Номер телефона")
                                {
                                    if (update.Message.Contact != null)
                                        editDataUserForm.SetParam(message.Contact.PhoneNumber.ToString());
                                    else
                                        editDataUserForm.SetParam(message.Text);

                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: new ReplyKeyboardRemove());
                                    editDataUserForm = null;
                                }
                                else
                                {
                                    editDataUserForm.SetParam(message.Text);
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: new ReplyKeyboardRemove());
                                    editDataUserForm = null;
                                }
                            }

                        }
                        else if (message.Text == "/addproduct")
                        {
                            productAddForm = new ProductAddForm();
                            var msg = productAddForm.StageText();
                            await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: productAddForm.keyboard);
                            if (message.Text == "/addproduct")
                                productAddForm.stage = 2;
                            else
                                productAddForm.SetParam(message.Text);
                        }
                        else if (productAddForm != null)
                        {
                            var msg = productAddForm.StageText();
                            if (message.Text == "Назад" || message.Text == "/back")
                            {

                                if (productAddForm.stage == 2)
                                {
                                    productAddForm = null;
                                    await botClient.SendTextMessageAsync(chatId, "Добавление товара прекращено. Возвращаю тебя в начало", replyMarkup: new ReplyKeyboardRemove());
                                }
                                else if(productAddForm.stage == 3)
                                {
                                    productAddForm.stage -= 2;
                                    msg = productAddForm.StageText();
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyBackKeyboard) ;
                                    productAddForm.stage++;
                                    msg = productAddForm.StageText();
                                }
                                else
                                {
                                    productAddForm.stage -= 2;
                                    msg = productAddForm.StageText();
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyBackKeyboard);
                                    productAddForm.stage++;
                                    msg = productAddForm.StageText();
                                }
                            }
                            else if (message.Text == "Прекратить")
                                productAddForm = null;
                            else if (productAddForm.stage == 8)
                            {
                                productAddForm.SetParam(message.Text);
                                await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: new ReplyKeyboardRemove());
                                productAddForm = null;
                            }
                            else
                            {
                                productAddForm.SetParam(message.Text);
                                await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyBackKeyboard);

                            }
                        }
                    }
                    else
                    {
                        if (message.Text == "/start")
                        {
                            await botClient.SendTextMessageAsync(chatId, $"{user.Greeting()}");
                        }
                        else if (message.Text == "/edit")
                        {
                            editDataUserForm = new EditDataUserForm(user);
                            var msg = editDataUserForm.StageText();
                            await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyAllParametersKeyboard);
                            if (message.Text == "/edit")
                                editDataUserForm.stage = 2;
                            else
                                editDataUserForm.SetParam(message.Text);

                        }
                        else if (editDataUserForm != null)
                        {
                            var msg = editDataUserForm.StageText();
                            if (message.Text == "Назад")
                            {

                                if (editDataUserForm.stage == 2)
                                {
                                    editDataUserForm = null;
                                    await botClient.SendTextMessageAsync(chatId, "Изменение данных прекращено. Возвращаю Вас в начало");
                                }
                                else
                                {
                                    editDataUserForm.stage -= 2;
                                    msg = editDataUserForm.StageText();
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyBackKeyboard);
                                    editDataUserForm.stage++;
                                    msg = editDataUserForm.StageText();
                                }
                            }
                            else if (message.Text == "Прекратить")
                                editDataUserForm = null;
                            else if (editDataUserForm.stage == 2)
                            {

                                if (message.Text == "Страна")
                                {
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyCountriesKeyboard);
                                    editDataUserForm.SetParam(message.Text);

                                }
                                else if (message.Text == "Пол")
                                {
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyGenderKeyboard);
                                    editDataUserForm.SetParam(message.Text);


                                }
                                else if (message.Text == "Номер телефона")
                                {
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplySendPhoneKeyboard);
                                    editDataUserForm.SetParam(message.Text);
                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyBackKeyboard);
                                    editDataUserForm.SetParam(message.Text);

                                }
                            }
                            else if (editDataUserForm.stage == 3)
                            {
                                if (editDataUserForm.field == "Страна")
                                {
                                    if (message.Text == "🇷🇺Россия" || message.Text == "🌐Другое")
                                    {
                                        editDataUserForm.SetParam(message.Text);
                                        await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: new ReplyKeyboardRemove());
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(chatId, "Пожалуйста, выберите один из варинатов на кнопках.", replyMarkup: _markups.ReplyCountriesKeyboard);
                                    } //если не по кнопке
                                }
                                else if (editDataUserForm.field == "Пол")
                                {
                                    if (message.Text == "🇷🇺Россия" || message.Text == "🌐Другое")
                                    {
                                        editDataUserForm.SetParam(message.Text);
                                        await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplySendPhoneKeyboard);
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(chatId, "Пожалуйста, выберите один из варинатов на кнопках.", replyMarkup: _markups.ReplyCountriesKeyboard);
                                    } //если не по кнопке

                                }
                                else if (editDataUserForm.field == "Номер телефона")
                                {
                                    if (update.Message.Contact != null)
                                        editDataUserForm.SetParam(message.Contact.PhoneNumber.ToString());
                                    else
                                        editDataUserForm.SetParam(message.Text);

                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyBackKeyboard);
                                }
                                else
                                {
                                    editDataUserForm.SetParam(message.Text);
                                    await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyBackKeyboard);
                                }
                            }

                        }


                    } //обычный юзер

                }
                else
                {

                    var msg = regularUserRegForm.StageText();
                    if (message.Text == "Назад")
                    {

                        if (regularUserRegForm.stage != 2)
                        {
                            regularUserRegForm.stage -= 2;
                            msg = regularUserRegForm.StageText();
                            await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyBackKeyboard);
                            regularUserRegForm.stage++;
                            msg = regularUserRegForm.StageText();
                        }
                    }
                    else if (regularUserRegForm.stage == 3)
                    {
                        regularUserRegForm.SetAccountData(chatId, username);
                        await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyGenderKeyboard);
                        regularUserRegForm.SetParam(message.Text);
                    }
                    else if (regularUserRegForm.stage == 4)
                    {
                        if (message.Text == "🤵‍♀️Женщина" || message.Text == "🤵‍♂️Мужчина")
                        {
                            regularUserRegForm.SetParam(message.Text);
                            await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplyCountriesKeyboard);
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(chatId, "Пожалуйста, выберите один из варинатов на кнопках.", replyMarkup: _markups.ReplyGenderKeyboard);
                        } //если не по кнопке
                    }
                    else if (regularUserRegForm.stage == 5)
                    {
                        if (message.Text == "🇷🇺Россия" || message.Text == "🌐Другое")
                        {
                            regularUserRegForm.SetParam(message.Text);
                            await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: _markups.ReplySendPhoneKeyboard);
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(chatId, "Пожалуйста, выберите один из варинатов на кнопках.", replyMarkup: _markups.ReplyCountriesKeyboard);
                        } //если не по кнопке
                    }
                    else if (regularUserRegForm.stage == 6)
                    {
                        regularUserRegForm.SetParam(message.Contact.PhoneNumber.ToString());
                        await botClient.SendTextMessageAsync(chatId, msg, replyMarkup: new ReplyKeyboardRemove());
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId, msg);
                        if (message.Text == "/start")
                            regularUserRegForm.stage = 2;
                        else
                            regularUserRegForm.SetParam(message.Text);
                    }
                } //регистрация юзера


            }
        }
        private static Task Error(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            botClient.SendTextMessageAsync(chatId, $"{exception.Message}");
            throw new NotImplementedException();
        }
    }
}
