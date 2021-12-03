using STLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace STLib.User
{
    public class Manage
    {
        /// <summary>
        /// Проверка пользователя на регистрацию
        /// </summary>
        /// <param name="id">id пользователя из Telegram</param>
        /// <returns>вернет true если есть в системе и false если нет</returns>
        public static async Task<bool> CheckExists(int id)
        {
            if (Globals.dataBase is null)
                throw new Exception("Вы забыли инциализировать систему");

            var query = Globals.dataBase.Table<Users>().Where(s => s.Id == id);

            var result = await query.ToListAsync();

            return result.Count > 0;
        }

        /// <summary>
        /// Добавление пользователя в систему
        /// </summary>
        /// <param name="id">id пользователя из Telegram</param>
        /// <param name="name">Имя пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>true если пользователь добавлен успешно и false если нет</returns>
        public static async Task<bool> Register(int id, string name, string password)
        {
            if (Globals.dataBase is null)
                throw new Exception("Вы забыли инциализировать систему");

            return await Globals.dataBase.InsertAsync(new Users()
            {
                Id = id,
                Name = name,
                Password = password
            }) == 1;
        }
    }
}
