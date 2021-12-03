using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace STLib
{
    public static class Globals
    {
        /// <summary>
        /// Ссылка на инициализированный компонент для обращения к базе данных
        /// </summary>
        public static SQLiteAsyncConnection dataBase;
    }
}
