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
        public static bool CheckExists(long id)
        {
            if (Globals.dataBase is null)
                throw new Exception("Вы забыли инциализировать систему");

            var result = new List<Users>();

            Task.Run(async () =>
            {
                var query = Globals.dataBase.Table<Users>().Where(s => s.Id == (int)id);

                result = await query.ToListAsync();
            }).Wait();

            return result.Count > 0;
        }

        /// <summary>
        /// Добавление пользователя в систему
        /// </summary>
        /// <param name="id">id пользователя из Telegram</param>
        /// <param name="name">Имя пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>true если пользователь добавлен успешно и false если нет</returns>
        public static bool Register(long id, string name, string password, int lvl)
        {
            if (Globals.dataBase is null)
                throw new Exception("Вы забыли инциализировать систему");

            bool isSuc = false;

            Task.Run(async () => isSuc = await Globals.dataBase.InsertAsync(new Users()
            {
                Id = (int)id,
                Name = name,
                Password = password,
                low_level = lvl,
                passed_learning = ""
            }) == 1).Wait();

            return isSuc;
        }
    }
}
