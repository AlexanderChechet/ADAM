using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task2;
using Task3;

namespace Output
{
    class Program
    {
        static void Main(string[] args)
        {
            var task3 = new Task3Library();
            task3.CreateTextSettingDefinition();
            Console.WriteLine("DONE");
            Console.ReadLine();
        }
    }
}
