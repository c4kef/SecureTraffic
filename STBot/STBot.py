import clr

def printTick(answer):
  Class1.Write(answer)

clr.AddReference("D:\Build\Debug\STLib.dll")
from STLib import Class1
Class1.Write("Hello world!")
Class1.Init()
Class1.testEvent += printTick
