using STLib.Utils;
using System;
using System.Threading.Tasks;
using SQLite;
using System.Reflection;
using System.IO;

namespace STLib
{
    public class Main
    {
        public static void Write(string text) => Console.WriteLine(text);

        public static event TestEvent testEvent;
        public delegate void TestEvent(string answer);

        public static string[] GetQuestions() => new string[] { "Вопрос", "Вопрос 2", "Вопрос 3" };

        public static string GetAnswer(string question) => (question == "Вопрос") ? "Хорошая АЛЕК" : "Плохая работа АЛЕККК";

        /// <summary>
        /// Инициализация системы
        /// </summary>
        /// <param name="DBPath">Полный путь до базы sqlite</param>
        public static void Init(string DBPath)
        {
            Globals.dataBase = new SQLiteAsyncConnection(DBPath);
            Globals.baseStart = new BaseStart();
            Globals.baseStart.Load();
        }
    }
}