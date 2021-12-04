using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
var enc1251 = Encoding.GetEncoding(1251);
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = enc1251;

STLib.Main.Init(@"C:\Users\artem\source\repos\SecureTraffic\identifier.sqlite", "");
var q = new STLib.AI.QHandler(0);
for (int i = 0; i < 3; i++)
{
    q.GetStep();
    Console.WriteLine(q.GetMessageStep());
}

while(q.GetStep())
{
    Console.WriteLine(q.GetMessageStep());
    string a = q.AddAnswerStep(Console.ReadLine());
    Console.WriteLine(a);
}
Console.WriteLine("конец!");