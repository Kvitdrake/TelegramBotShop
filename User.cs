using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMaryBot.UserBase
{
    public abstract class User
    {
        public long ChatId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public DateTime DateRegistration { get; set; }


        public abstract string Greeting();
    }
}
