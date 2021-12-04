using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STLib.Utils;

namespace STLib.AI
{
    public class QHandler
    {
        private int Id;
        private Random random;
        private int step = 0;
        private int level = 0;
        private Dictionary<BaseStartStruct, bool> keys;
        private BaseStartStruct currentStep;

        public QHandler(int id)
        {
            Id = id;
            random = new Random();
            keys = new Dictionary<BaseStartStruct, bool>();
        }

        public async Task<bool> GetStep()
        {
            await Task.Run(() =>
            {
            again://FIX THAT!!!
                List<BaseStartStruct> steps = new List<BaseStartStruct>();
                int maxStep = 0;

                List<BaseStartStruct> datas = Globals.baseStart.instance.FindAll(bss => bss.level == level);
                foreach (BaseStartStruct data in datas)
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
            });

            return currentStep.answerNeeded;
        }

        public string GetMessageStep() => currentStep.results[currentStep.randomResult ? random.Next(0, currentStep.results.Length - 1) : 0];

        public async Task<string> GetIncorrectMessage()
        {
            BaseStartStruct incorrectAnswer = default;

            await Task.Run(() =>
            {
                List<BaseStartStruct> datas = Globals.baseStart.instance.FindAll(bss => bss.level == level);
                foreach (BaseStartStruct data in datas)
                    if (data.level == level && data.type == BaseStartTypes.incorrect_answer_gen_mask)
                        incorrectAnswer = data;
            });

            return incorrectAnswer.results[incorrectAnswer.randomResult ? random.Next(0, incorrectAnswer.results.Length - 1) : 0];
        }

        public async Task<string> GetCounterquestionMessage(string message)
        {
            BaseStartStruct counterquestion = default;

            await Task.Run(() =>
            {
                List<BaseStartStruct> datas = Globals.baseStart.instance.FindAll(bss => bss.level == level);
                foreach (BaseStartStruct data in datas)
                    if (data.level == level && data.type == BaseStartTypes.counterquestion)
                        foreach(string keyword in data.keywords)
                            if (message.Contains(keyword))
                                counterquestion = data;
            });

            return (counterquestion.Equals(default(BaseStartStruct))) ? string.Empty : counterquestion.results[counterquestion.randomResult ? random.Next(0, counterquestion.results.Length - 1) : 0];
        }

        public async Task<string> AddAnswerStep(string message)
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

            string counterquestion = await GetCounterquestionMessage(message);

            if (!isFoundContains && string.IsNullOrEmpty(counterquestion))
                answer = await GetIncorrectMessage();

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
