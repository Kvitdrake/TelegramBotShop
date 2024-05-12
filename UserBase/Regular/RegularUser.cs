using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMaryBot.UserBase.Regular
{
    public class RegularUser : User
    {
        public string SecondName { get; set; }
        public RegularUser() { }

        public override string Greeting()
        {
            return "Привет, давай знакомиться";
        }
    }
}
