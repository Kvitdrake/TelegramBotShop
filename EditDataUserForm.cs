using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TestMaryBot.Forms
{
    public class EditDataUserForm : Form
    {
        public string? field { get; set; }
        private UserBase.User user { get; set; }
        public EditDataUserForm(UserBase.User user) { this.user = user; stage = 1; }

        public override string StageText()
        {
            if (stage == 1)
                return "Выберите параметр, который хотели бы изменить:";
            if (stage == 2)
                return "Отлично, введите новые данные";
            if (stage == 3)
                return "Ваши даннные обновлены. Направляю Вас в начало.";
            if (stage == 4)
                return "Введите Вашу страну:";
            else
                return " ";
        }
        public override void SetParam(string param)
        {
            if (stage == 2)
            {
                field = param;
            }
            if (stage == 3)
            {
                if (field == "Дата рождения")
                {
                    string[] dates = param.Split(".");
                    short day = Convert.ToInt16(dates[0]);
                    short month = Convert.ToInt16(dates[1]);
                    int year = Convert.ToInt16(dates[2]);
                    param = $"{year}-{month}-{day}";
                }
                else if (field == "Пол")
                {
                    if (param == "🤵‍♀️Женщина")
                        param = "G";
                    if (param == "🤵‍♂️Мужчина")
                        param = "M";
                }
                else if (field == "Страна")
                {
                    if (param == "🇷🇺Россия")
                        param = "Россия";
                    if (param == "🌐Другое")
                        param = "Другое";
                }   
                databaseManager.UpdateData( field, param, user.ChatId, user.Username);
            }
            stage++;
        }
    }
}
