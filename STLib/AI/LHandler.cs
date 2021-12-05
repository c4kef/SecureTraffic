using STLib.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace STLib.AI
{
    /// <summary>
    /// Получение материала на обучение
    /// </summary>
    public class LHandler
    {
        /// <summary>
        /// Уникальный идентификатор пользователя Telegram
        /// </summary>
        private long Id;
        /// <summary>
        /// Список материала
        /// </summary>
        private List<LMaterial> materials;

        private int step = 0;

        private int countCorrectAnswers = 0;

        private LMaterial currentTest;

        /// <summary>
        /// Объявление обработчика обучения
        /// </summary>
        /// <param name="id">Уникальный идентификатор пользователя Telegram</param>
        public LHandler(long id)
        {
            Id = id;
            materials = new List<LMaterial>();
            Init();
        }

        /// <summary>
        /// Инициализация (подгрузка материала)
        /// </summary>
        private void Init()
        {
            foreach (var dir in Directory.GetFiles(@"D:\Build\Debug\LMaterials"))
                materials.Add(File.ReadAllText(dir).FromJson<LMaterial>());
        }
        
        /// <summary>
        /// Получение списка рекомендаций
        /// </summary>
        /// <returns>вернет разбросанный вариант по строкам (требуется спарсить)</returns>
        public string GetRecomendation()
        {
            List<LMaterial> result = new List<LMaterial>();

            var user = Globals.dataBase.QueryAsync<Users>($"SELECT * FROM Users WHERE Id = {Id}").Result[0];

            foreach (LMaterial material in materials)
                if (material.level <= user.low_level)
                    result.Add(material);

            return result.ToJson();
        }

        /// <summary>
        /// Получение списка остальных занятий
        /// </summary>
        /// <returns>вернет разбросанный вариант по строкам (требуется спарсить)</returns>
        public string GetAnother()
        {
            List<LMaterial> result = new List<LMaterial>();

            var user = Globals.dataBase.QueryAsync<Users>($"SELECT * FROM Users WHERE Id = {Id}").Result[0];

            foreach (LMaterial material in materials)
                if (material.level > user.low_level)
                    result.Add(material);

            return result.ToJson();
        }

        /// <summary>
        /// Выбор теста
        /// </summary>
        /// <param name="title">Название теста</param>
        /// <returns>0 если тест уже загружен, -1 если не смогли, 1 если успешно загружен</returns>
        public int SelectTest(string title)
        {
            if (currentTest != null)
                return 0;

            var material = materials.Find(x => x.title == title);

            if (material == null)
                return -1;

            currentTest = material;
            return 1;
        }

        /// <summary>
        /// Проверка на доступность этапов
        /// </summary>
        /// <returns>true если есть следующий этап и false наоборот</returns>
        public bool CheckSteps()
        {
            bool retVal = step <= currentTest.content.Count - 1;

            if (!retVal)
            {
                step = countCorrectAnswers = 0;
                currentTest = null;
            }

            return retVal;
        }

        /// <summary>
        /// Получение вопроса этапа
        /// </summary>
        /// <returns>возвращает вопросы в формате JSON текущего этапа</returns>
        public string GetQuestion() => currentTest.content[step].ToJson();

        /// <summary>
        /// Проверка ответа на правильность
        /// </summary>
        /// <param name="answer">сам ответ</param>
        /// <returns>true в случае если ответ верен и false если нет</returns>
        public bool CheckAnswer(string answer)
        {
            bool isCorrectAnswer = false;

            if (answer == currentTest.content[step].correctAnswer)
            {
                isCorrectAnswer = true;
                countCorrectAnswers++;
            }

            ++step;

            return isCorrectAnswer;
        }
    }
}
