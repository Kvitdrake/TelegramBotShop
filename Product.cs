using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMaryBot.Models
{
    public class Product
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price_r  { get; set; }
        public int Price_e { get; set; }
        public int Amount { get; set; }
        public string Hashtags { get; set; }
        public int Type { get; set; }
        public Product() { }
    }
}
