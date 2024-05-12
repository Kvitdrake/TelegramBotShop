using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TestMaryBot.Keyboards;
using TestMaryBot.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestMaryBot.Forms
{
    public class ProductAddForm : Form
    {
        public Product product { get; set; }
        public ReplyKeyboardMarkup keyboard {  get; set; }
        public ProductAddForm()
        {
            product = new Product();
            stage = 1;
            keyboard = new ReplyKeyboardMarkup(KeyboardsMarkup.GetButtons()); //чтобы появлялось актуальное меню
        }
        public override string StageText()
        {
            if (stage == 1)
                return "Выбери категорию нового товара";
            if (stage == 2)
                return "Введи название товара";
            if (stage == 3)
                return "Введи его цену в рублях";
            if (stage == 4)
                return "Введи его цену в доллорах:";
            if (stage == 5)
                return "Напиши его описание";
            if (stage == 6)
                return "Сколько в наличии?";
            if (stage == 7)
                return $"Напиши пару хэштегов \nЖелательно, напиши, с каким Богом связан этот товар или на что он. \n\nПример: Фрейя удача любовь";
            if (stage == 8)
                return $"Хорошо, {product.Title} был успешно добавлен";
            else
                return " ";
        }
        public override void SetParam(string param)
        {
            if(stage == 2) 
                product.Type = databaseManager.GetNumberCategoryProduct( param);
            if (stage == 3)
                product.Title = param;
            if (stage == 4)
                product.Price_r = Convert.ToInt16(param);
            if (stage == 5)
                product.Price_e = Convert.ToInt16(param);
            if (stage == 6)
                product.Description = param;
            if (stage == 7)
                product.Amount = Convert.ToInt16(param);
            if (stage == 8)
            {
                product.Hashtags = param;
                databaseManager.AddNewProduct(product.Type, product.Title, product.Price_r, product.Price_e, product.Description, product.Amount, product.Hashtags );
            }

            stage++;
        }
    }
}
