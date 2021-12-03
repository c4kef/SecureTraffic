using STLib.Utils;
using System;
using System.Threading.Tasks;
using SQLite;
namespace STLib
{
    public class Class1
    {
        public static void Write(string text) => Console.WriteLine(text);

        public static event TestEvent testEvent;
        public delegate void TestEvent(string answer);

        public static string[] GetQuestions() => new string[] { "Вопрос", "Вопрос 2", "Вопрос 3" };

        public static string GetAnswer(string question) => (question == "Вопрос") ? "Хорошая АЛЕК" : "Плохая работа АЛЕККК";

        public static async Task Init()
        {
            Globals.dataBase = new SQLiteAsyncConnection(@"C:\Users\artem\source\repos\SecureTraffic\identifier.sqlite");
        }

        public static async Task Test()
        {
            Console.WriteLine(await Globals.dataBase.InsertAsync(new Users() { Id = 1, Name = "Artemiy", Password = "Gast" }));
        }
    }
}