using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMaryBot.Models
{
    public class Servise
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public int Prise_r {  get; set; }
        public int Prise_e { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public short Steps { get; set; }

        public Servise() { }
    }
}
