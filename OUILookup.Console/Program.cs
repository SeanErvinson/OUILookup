using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OUILookup.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine(Regex.IsMatch("00FF", @"^[\dA-F]+$"));

            System.Console.Read();
        }




    }
}
