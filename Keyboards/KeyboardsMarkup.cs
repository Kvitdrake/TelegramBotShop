using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TestMaryBot.Forms;

namespace TestMaryBot.Keyboards
{
    internal class KeyboardsMarkup
    {
        public ReplyKeyboardMarkup ReplyGenderKeyboard = new ReplyKeyboardMarkup(new[]
                            {
                                    new KeyboardButton[]
                                        {
                                            MessageResponses.Gender_G,
                                            MessageResponses.Gender_M
                                        },
                                    new KeyboardButton[]
                                        {
                                            MessageResponses.Back
                                        },
                                    new KeyboardButton[]
                                        {
                                            MessageResponses.Stop
                                        }
                                    });
        public ReplyKeyboardMarkup ReplyAllParametersKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            { MessageResponses.Name },
            [MessageResponses.SecondName],[MessageResponses.DateBirth],[MessageResponses.Country],[MessageResponses.Gender],[MessageResponses.PhoneNumber],[MessageResponses.Email],[MessageResponses.Username], [MessageResponses.Back], [MessageResponses.Stop]
        });
        public ReplyKeyboardMarkup ReplySendPhoneKeyboard = new ReplyKeyboardMarkup(new[]
                            {
                                    new KeyboardButton[]
                                        {
                                            MessageResponses.SendPhoneNumber
                                        },
                                        new KeyboardButton[]
                                        {
                                            MessageResponses.Back
                                        },
                                    new KeyboardButton[]
                                        {
                                            MessageResponses.Stop
                                        }
                                    });
        public ReplyKeyboardMarkup ReplyCountriesKeyboard = new ReplyKeyboardMarkup(new[]
                            {
                                    new KeyboardButton[]
                                        {
                                            MessageResponses.Ru
                                            },
                                    new KeyboardButton[]{
                                            MessageResponses.EngPay
                                        },
                                    new KeyboardButton[]
                                        {
                                            MessageResponses.Back
                                        },
                                    new KeyboardButton[]
                                        {
                                            MessageResponses.Stop
                                        }
                                    });
        public ReplyKeyboardMarkup ReplyAddingKeyboard = new ReplyKeyboardMarkup(new List<KeyboardButton[]>()
                                    {
                    {
                                    new KeyboardButton[]
                                        {
                                            MessageResponses.Add
                                        }
                                    } });
        public ReplyKeyboardMarkup ReplyBackKeyboard = new ReplyKeyboardMarkup(new[]
                            {
                                    new KeyboardButton[]
                                        {
                                            MessageResponses.Back
                                        },
                                    new KeyboardButton[]
                                        {
                                            MessageResponses.Stop
                                        }
                                    });

        public InlineKeyboardMarkup InlineAddingKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Посвящение", "god_dedication_button"),
                    InlineKeyboardButton.WithCallbackData("Курс")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Товар"),
                    InlineKeyboardButton.WithCallbackData("Назад")
                }
            });
        public KeyboardsMarkup()
        {
        }

        public static List<KeyboardButton> GetButtons()
        {
            DatabaseManager manager = new DatabaseManager();
            List<string> categories = manager.GetAllTypesProducts();
            List<KeyboardButton> buttons = new List<KeyboardButton>();
            foreach (string category in categories)
            {
                buttons.Add(new KeyboardButton(category));
            }
            return buttons;
        }
    }
}
