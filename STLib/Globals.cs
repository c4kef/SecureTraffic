using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using STLib.Utils;

namespace STLib
{
    public static class Globals
    {
        /// <summary>
        /// Ссылка на инициализированный компонент для обращения к базе данных
        /// </summary>
        public static SQLiteAsyncConnection dataBase;

        /// <summary>
        /// Ссылка на инициализированный компонент для обращения к аналитической базе
        /// </summary>
        public static BaseStart baseStart;
    }
}
