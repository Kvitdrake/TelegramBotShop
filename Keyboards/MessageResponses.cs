using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace TestMaryBot.Keyboards
{
    internal class MessageResponses
    {
        public static KeyboardButton Edit => new KeyboardButton("Изменить");
        public static KeyboardButton Stop => new KeyboardButton("Прекратить");
        public static KeyboardButton Back => new KeyboardButton("Назад");
        public static KeyboardButton Add => new KeyboardButton("Добавить");
        public static KeyboardButton Gender_G => new KeyboardButton("🤵‍♀️Женщина");
        public static KeyboardButton Gender_M => new KeyboardButton("🤵‍♂️Мужчина");
        public static KeyboardButton Ru => new KeyboardButton("🇷🇺Россия");
        public static KeyboardButton KZ => new KeyboardButton("🇰🇿Казахстан");
        public static KeyboardButton SNG => new KeyboardButton("🌏СНГ");
        public static KeyboardButton EngPay => new KeyboardButton("🌐Другое");
        public static KeyboardButton SendPhoneNumber => new KeyboardButton("Отправить номер") { RequestContact = true };
        public static KeyboardButton Name => new KeyboardButton("Имя");
        public static KeyboardButton SecondName => new KeyboardButton("Фамилия");
        public static KeyboardButton DateBirth => new KeyboardButton("Дата рождения");
        public static KeyboardButton Country => new KeyboardButton("Страна");
        public static KeyboardButton Gender => new KeyboardButton("Пол");
        public static KeyboardButton Email => new KeyboardButton("Электропочта");
        public static KeyboardButton PhoneNumber => new KeyboardButton("Номер телефона");
        public static KeyboardButton Username => new KeyboardButton("Юзернейм");

    }
}
