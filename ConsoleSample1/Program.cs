using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ConsoleSample1
{
    class Program
    {
        const string ResourcePath = @"C:\Users\Public\Pictures\Sample Pictures";

        static void Main(string[] args)
        {
            DirectoryTraversal.DirectoryTraversal1(@ResourcePath);
            Console.WriteLine();
            DirectoryTraversal.DirectoryTraversal2(@ResourcePath);

            Console.ReadKey(true);

        }

    }
}
