using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ClassLibraryHelper.C_;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //string name = "Hello"; // TODO: Initialize to an appropriate value
            //int iteration = 1; // TODO: Initialize to an appropriate value
            //Action action = DoSomeThing; // TODO: Initialize to an appropriate value
            //CodeTimer.Time(name, iteration, action);
            int iteration = 100 * 1000;

            string s = "";
            CodeTimer.Time("String Concat", iteration, () => { s += "a"; });

            StringBuilder sb = new StringBuilder();
            CodeTimer.Time("StringBuilder", iteration, () => { sb.Append("a"); });
            Console.Read();
        }

        public static void DoSomeThing()
        {
            Console.WriteLine("Sleep 3 second");
            Thread.Sleep(3000);
            Console.WriteLine("T Do For");
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(i);
            }
        }
    }
}
