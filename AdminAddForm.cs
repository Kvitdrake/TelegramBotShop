using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Telegram.Bot.Types;

namespace TestMaryBot.UserBase.Administrator
{
    internal class AdminAddForm 
    {
        public Admin admin = new Admin();
        private static User user;
        public int stage;
        public AdminAddForm(User admin)
        {
            user = admin;
            stage = 1;
        }
        DatabaseManager databaseManager = new DatabaseManager();

        public string StageText()
        {
            if (stage == 1)
                return $"{user.Name}, введи имя нового админа";
            if (stage == 2)
                return "Введи его/её юзернейм:";
            if (stage == 3)
                return "Введи его/её дату рождения:";
            if (stage == 4)
                return "Введи его/её страну:";
            if (stage == 5)
                return "Введи его/её номер:";
            if (stage == 6)
                return "Отправь его/её электропочту:";
            if (stage == 7)
                return $"Новый админ успешно зарегистрирован";
            else
                return " ";
        }

        public void SetParam(string param)
        {
            if (stage == 2)
            {
                admin.Name = param;
            }
            if (stage == 3)
            {
                admin.Username = param;

            }
            if (stage == 4)
            {
                string[] dates = param.Split(".");
                short day = Convert.ToInt16(dates[0]);
                short month = Convert.ToInt16(dates[1]);
                int year = Convert.ToInt16(dates[2]);
                admin.DateOfBirth = new DateTime(year, month, day);
            }
            if (stage == 5)
            {
                if (param == "🇷🇺Россия")
                    param = "Россия";
                if (param == "🌐Другое")
                    param = "Другое";
                admin.Country = param;
            }
            if (stage == 6)
                admin.Number = param;
            if (stage == 7)
            {
                admin.Email = param;
                admin.DateRegistration = DateTime.Now;
                databaseManager.AddNewAdmin(admin.Name, $"{admin.DateOfBirth.Year}-{admin.DateOfBirth.Month}-{admin.DateOfBirth.Day}", admin.Username, admin.Country, admin.Email, admin.Number, $"{admin.DateRegistration.Year}-{admin.DateRegistration.Month}-{admin.DateRegistration.Day}");
            }
            stage++;
        }

    }
}
