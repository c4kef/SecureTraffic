using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STLib.Utils;

namespace STLib.AI
{
    /// <summary>
    /// Класс для регисрации маски пользователя
    /// </summary>
    public class QHandler
    {
        /// <summary>
        /// уникальный идентификатор пользователя в Telegram
        /// </summary>
        private long Id;
        /// <summary>
        /// текущий шаг
        /// </summary>
        private int step = 0;
        /// <summary>
        /// текущий уровень
        /// </summary>
        private int level = 0;
        /// <summary>
        /// сохраненная структура для дальнейшей обработки на слабости
        /// </summary>
        private Dictionary<BaseStartStr, bool> keys;
        /// <summary>
        /// Просто временная переменная
        /// </summary>
        private BaseStartStr currentStep;

        private Random random;

        /// <summary>
        /// Инициализация регистратора
        /// </summary>
        /// <param name="id">уникальный идентификатора пользователя в Telegram</param>
        public QHandler(long id)
        {
            Id = id;
            random = new Random();
            keys = new Dictionary<BaseStartStr, bool>();
        }

        /// <summary>
        /// Получение следующего этапа вопроса
        /// </summary>
        /// <returns>true если еще есть вопросы, false если их не осталось</returns>
        public bool GetStep()
        {
            again://FIX THAT!!!
                if (level > Globals.baseStart.maxLevel)
                {
                    currentStep = null;
                    goto end;
                }

                List<BaseStartStr> steps = new List<BaseStartStr>();
                int maxStep = 0;

                List<BaseStartStr> datas = Globals.baseStart.instance.FindAll(bss => bss.level == level);
                foreach (BaseStartStr data in datas)
                    if (data.level == level && data.type != BaseStartTypes.incorrect_answer_gen_mask && data.type != BaseStartTypes.counterquestion)
                        steps.Add(data);

                maxStep = steps.Count;

                if (step >= maxStep)
                {
                    step = 0;
                    ++level;
                    goto again;
                }

                currentStep = steps[step];

                if (!currentStep.answerNeeded)
                    ++step;
            end:
            return currentStep is null ? false : currentStep.answerNeeded;
        }

        /// <summary>
        /// Получение текущего сообщения этапа
        /// </summary>
        /// <returns>возвращает сообщение текущего этапа</returns>
        public string GetMessageStep() => currentStep.results[currentStep.randomResult ? random.Next(0, currentStep.results.Length - 1) : 0];

        /// <summary>
        /// Получаем сообщение когда пользователь догадался не отвечать
        /// </summary>
        /// <returns>ответ для вывода</returns>
        public string GetIncorrectMessage()
        {
            BaseStartStr incorrectAnswer = null;

                List<BaseStartStr> datas = Globals.baseStart.instance.FindAll(bss => bss.level == level);
                foreach (BaseStartStr data in datas)
                    if (data.level == level && data.type == BaseStartTypes.incorrect_answer_gen_mask)
                        incorrectAnswer = data;

            return incorrectAnswer.results[incorrectAnswer.randomResult ? random.Next(0, incorrectAnswer.results.Length - 1) : 0];
        }

        /// <summary>
        /// Получаем ответ на сообщение в котором содержится вопрос
        /// </summary>
        /// <param name="message">текст полного сообщения</param>
        /// <returns>пустая строка если ничего не найдено, ну а полная если найден ответ на контрвопрос</returns>
        public string GetCounterquestionMessage(string message)
        {
            BaseStartStr counterquestion = null;

                List<BaseStartStr> datas = Globals.baseStart.instance.FindAll(bss => bss.level == level);
                foreach (BaseStartStr data in datas)
                    if (data.level == level && data.type == BaseStartTypes.counterquestion)
                        foreach(string keyword in data.keywords)
                            if (message.Contains(keyword))
                                counterquestion = data;

            return counterquestion is null ? string.Empty : counterquestion.results[counterquestion.randomResult ? random.Next(0, counterquestion.results.Length - 1) : 0];
        }

        /// <summary>
        /// Установка ответа на поставленный вопрос ботом
        /// </summary>
        /// <param name="message">тест полного сообщения</param>
        /// <returns>ответ на поставленный ответ (гениально звучит). Пустая строка значит что ничего нет, ну а строка со значением наоборот</returns>
        public string AddAnswerStep(string message)
        {
            bool isFoundContains = false;
            string answer = string.Empty;

            foreach (string data in currentStep.keywords)
            {
                if (data == "%num%")
                    if (message.Any(char.IsDigit))
                        isFoundContains = true;

                if (message.ToLower().Contains(data) && !isFoundContains)
                    isFoundContains = true;
            }

            string counterquestion = GetCounterquestionMessage(message);

            if (!isFoundContains && string.IsNullOrEmpty(counterquestion))
                answer = GetIncorrectMessage();

            if (answer == string.Empty && !isFoundContains)
                answer = counterquestion + GetMessageStep();
            else
            {
                keys.Add(currentStep, isFoundContains);
                ++step;
            }

            return answer;
        }
    }
}
