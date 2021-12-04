using STLib.Utils;
using System;
using System.Threading.Tasks;
using SQLite;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace STLib
{
    public static class Main
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
        /// <param name="AIBasePath">Полный путь до базы бота</param>
        public static void Init(string DBPath, string AIBasePath)
        {
            InstallResolveHandler();
            Globals.dataBase = new SQLiteAsyncConnection(DBPath);
            Globals.baseStart = BaseStart.Load(AIBasePath);
        }

        public static void InstallResolveHandler()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveHandler;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve +=
                ResolveHandler;
        }

        public static void RemoveResolveHandler()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= ResolveHandler;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -=
                ResolveHandler;
        }

        static IDictionary<string, Assembly> _cache =
            new Dictionary<string, Assembly>();

        private static Assembly ResolveHandler(
            object sender, ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name).Name;
            Assembly result = null;

            if (!_cache.TryGetValue(name, out result))
            {
                var current = Assembly.GetAssembly(typeof(Main));
                var path = Path.GetDirectoryName(current.Location);
                var dllPath = Path.Combine(path, name + ".dll");

                if (File.Exists(dllPath))
                {
                    result = Assembly.LoadFrom(dllPath);
                }

                _cache[name] = result;
            }

            return result;
        }
    }
}