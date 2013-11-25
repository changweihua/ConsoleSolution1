using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PLinqSample
{
    class Program
    {
        const string PATH = @"E:\FTP\笔记本备份";

        static void Main(string[] args)
        {
            DirectoryTraversal.DirectoryTraversal1(PATH);
            DirectoryTraversal.DirectoryTraversal2(PATH);

            Console.ReadKey(true);
        }
    }
}
