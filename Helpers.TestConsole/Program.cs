using System;

namespace Helpers.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var misc = new Misc();
            var test = misc.ElapsedWorkingHours();
            var time = TimeSpan.FromMinutes(test);
            Console.WriteLine("{0:00}:{1:00}", (int)time.TotalHours, time.Minutes);
            Console.ReadLine();
        }
    }
}
