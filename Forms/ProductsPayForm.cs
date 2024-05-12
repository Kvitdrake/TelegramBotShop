using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TestMaryBot.Keyboards;
using TestMaryBot.Models;

namespace TestMaryBot.Forms
{
    public class ProductsPayForm : Form
    {
        public Product product;
        private UserBase.User user;
        public string Adress { get; set; }
        public int Count { get; set; }
        public ReplyKeyboardMarkup keyboardCategoryes { get; set; }
        public ReplyKeyboardMarkup keyboardProducts { get; set; }
        public ProductsPayForm()
        {
            product = new Product();
            stage = 1;
            keyboardCategoryes = new ReplyKeyboardMarkup(KeyboardsMarkup.GetButtons()); //чтобы появлялось актуальное меню
        }
        public override string StageText()
        {
            if (stage == 1)
                return "Выберите категорию товара";
            if (stage == 2)
                return "вот все товары на данный момент";
            if (stage == 3)
                return $"**{product.Title}** \n\n\n{product.Description} \nВ наличии - {product.Amount} шт. \n\nЦена {product.Price_r}₽ или {product.Price_e}$ \n\nХэштеги: \n{product.Hashtags}";
            if (stage == 4)
                return "Пожалуйста, напишите, какое количество Вы хотите купить:";
            if (stage == 5)
            {
                if (user.Country == "Россия")
                    return "Мы отправляем товары Почтой России, бла-бла. \n\nНапишите, пожалуйста, аде=рес доставки, откуда Вам будет удобно его забрать:";
                else
                    return "Мы как-нить отправим Вам товар. \n\nНапишите, пожалуйста, аде=рес доставки, откуда Вам будет удобно его забрать:";
            }
            if (stage == 6)
                return "Хорошо, на данный момент";
            if (stage == 7)
                return $"Напиши пару хэштегов \nЖелательно, напиши, с каким Богом связан этот товар или на что он. \n\nПример: Фрейя удача любовь";
            if (stage == 8)
                return $"Хорошо, {product.Title} был успешно добавлен";
            else
                return " ";
        }
        public override void SetParam(string param)
        {
            if (stage == 2)
                product.Type = databaseManager.GetNumberCategoryProduct(param);
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
                databaseManager.AddNewProduct(product.Type, product.Title, product.Price_r, product.Price_e, product.Description, product.Amount, product.Hashtags);
            }

            stage++;
        }
    }
}
