using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task2;

namespace Output
{
    class Program
    {
        static void Main(string[] args)
        {
            Finder finder = new Finder();
            var list = finder.FindRecordWithExtension("2000");
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("DONE");
            Console.ReadLine();
        }
    }
}
