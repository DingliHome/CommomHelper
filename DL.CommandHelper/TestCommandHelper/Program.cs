using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibraryHelper.C_.Extensions;

namespace TestCommonHelper
{
    public class Program
    {
        private static void Main(string[] args)
        {
            string typeName = string.Empty;
            int input = 3;
            input.Switch((string i) => typeName = i).Case(0, "aaaa").Case(2, "ccc").Default("www");

            //var enumerable = Enumerable.Range(1, 10);
            //var first = enumerable.ForEach(i => i=i* 10).First();
        }

    }

}
