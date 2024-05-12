using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMaryBot.UserBase.Administrator
{
    internal class Admin : User
    {
        public Admin() { }
        public override string Greeting()
        {
            return $"Приветствую, {Name}";
        }
        public static void NewAdmin()
        {

        }
    }
}
