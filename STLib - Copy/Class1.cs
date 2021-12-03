using System;
using System.Threading.Tasks;

namespace STLib
{
    public class Class1
    {
        public static void Write(string text) => Console.WriteLine(text);

        public static event TestEvent testEvent;
        public delegate void TestEvent(string answer);

        public static void Init()
        {
            Task.Run(async () =>
            {
                int i = 0;
                while (true)
                {
                    testEvent?.Invoke($"i is {++i}");
                }
            });
        }
    }
}