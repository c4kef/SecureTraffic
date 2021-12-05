using System.Text;
using STLib.Utils;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
var enc1251 = Encoding.GetEncoding(1251);
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = enc1251;

STLib.Main.Init(@"C:\Users\artem\source\repos\SecureTraffic\identifier.sqlite", "D:\\Build\\Debug\\basestart.json");
var q = new STLib.AI.LHandler(0);
q.SelectTest("Задание 1");
while(q.CheckSteps())
{
    Console.WriteLine(q.GetQuestion().FromJson<LContentMaterial>().question);
    Console.WriteLine(q.CheckAnswer(Console.ReadLine()));
}
Console.WriteLine("конец!");