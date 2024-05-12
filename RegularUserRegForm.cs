using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bots.Types;

namespace TestMaryBot.UserBase.Regular
{
    internal class RegularUserRegForm : RegularUser
    {
        public int stage;

        public RegularUserRegForm() => stage = 1;
        DatabaseManager databaseManager = new DatabaseManager();


        public string StageText()
        {
            if (stage == 1)
                return "Введите Ваши имя и фамилию, через пробел, пожалуйста:";
            if (stage == 2)
                return "Введите дату рождения:";
            if (stage == 3)
                return "Введите Ваш пол:";
            if (stage == 4)
                return "Введите Вашу страну:";
            if (stage == 5)
                return "Введите Ваш номер:";
            if (stage == 6)
                return "Отправьте боту Вашу электропочту:";
            if (stage == 7)
                return $"{Name}, Вы успешно прошли регистрацию! Добро пожаловать";
            else
                return " ";
        }
        public void SetAccountData(long chatId, string username)
        {
            ChatId = chatId;
            Username = username;
        }

        public void SetParam(string param)
        {
            if (stage == 2)
            {
                string[] s = param.Split();
                Name = s[0];
                SecondName = s[1];
            }
            if (stage == 3)
            {
                string[] dates = param.Split(".");
                short day = Convert.ToInt16(dates[0]);
                short month = Convert.ToInt16(dates[1]);
                int year = Convert.ToInt16(dates[2]);
                DateOfBirth = new DateTime(year, month, day);
            }
            if (stage == 4)
            {
                if (param == "🤵‍♀️Женщина")
                    param = "G";
                if (param == "🤵‍♂️Мужчина")
                    param = "M";
                Gender = param;
            }
            if (stage == 5)
            {
                if (param == "🇷🇺Россия")
                    param = "Россия";
                if (param == "🌐Другое")
                    param = "Другое";
                Country = param;
            }
            if (stage == 6)
                Number = param;
            if (stage == 7)
            {
                Email = param;
                DateRegistration = DateTime.Now;
                databaseManager.AddNewRegularUser(ChatId, Name, SecondName, Username, Email, Number, $"{DateOfBirth.Year}-{DateOfBirth.Month}-{DateOfBirth.Day}", Gender, $"{DateRegistration.Year}-{DateRegistration.Month}-{DateRegistration.Day}", Country);
            }
            stage++;
        }
    }
}
