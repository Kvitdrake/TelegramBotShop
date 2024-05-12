using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMaryBot.Forms
{
    abstract public class Form
    {
        public int stage;
        public static DatabaseManager databaseManager = new DatabaseManager();
        public abstract string StageText();
        public abstract void SetParam(string param);
    }
}
