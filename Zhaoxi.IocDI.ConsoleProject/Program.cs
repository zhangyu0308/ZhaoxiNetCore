using System;

namespace Zhaoxi.IocDI.ConsoleProject
{
    class Program
    {
        static void Main(string[] args)
        {
			try
			{
				Console.WriteLine("Welcome");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.ReadLine();
        }
    }
}
