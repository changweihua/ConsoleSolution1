using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace ConsoleSample1
{
    public class DirectoryTraversal
    {
        #region 公共

        struct FileResult
        {
            public string Text;
            public string FileName;
        }

        #endregion

        /// <summary>
        /// 您有权访问树中的所有目录，文件大小不太大，并且访问时间不太多。 此方法在最开始构造文件名数组时有一段时间的延迟
        /// </summary>
        public static void DirectoryTraversal1(string path)
        {
            Stopwatch watch = Stopwatch.StartNew();
            int count = 0;
            string[] files = null;

            try
            {
                files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("没有权限访问该文件夹");
                return;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("未找到文件夹 {0}", path);
                return;
            }

            var fileContents = from file in files.AsParallel()
                               let extension = Path.GetExtension(file)
                               where extension == ".txt" || extension == "*.html"
                               let text = File.ReadAllText(file)
                               select new FileResult { Text = text, FileName = file };
            try
            {
                foreach (var item in fileContents)
                {
                    Console.WriteLine(Path.GetFileName(item.FileName) + ":" + item.Text.Length);
                    count++;
                }
            }
            catch (AggregateException ae)
            {
                ae.Handle((ex) =>
                    {
                        if (ex is UnauthorizedAccessException)
                        {
                            Console.WriteLine(ex.Message);
                            return true;
                        }
                        return false;
                    });
            }

            Console.WriteLine("文件数为 {0}, 耗时 {1} 毫秒", count, watch.ElapsedMilliseconds);

        }

        /// <summary>
        /// 您有权访问树中的所有目录，文件大小不太大，并且访问时间不太多。 此方法比前面的示例更快地开始生成结果
        /// </summary>
        public static void DirectoryTraversal2(string path)
        {
            var count = 0;
            var watch = Stopwatch.StartNew();
            var fileNames = from dir in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                            select dir;


            var fileContents = from file in fileNames.AsParallel() // Use AsOrdered to preserve source ordering
                               let extension = Path.GetExtension(file)
                               where extension == ".txt" || extension == ".html"
                               let Text = File.ReadAllText(file)
                               select new { Text, FileName = file }; //Or ReadAllBytes, ReadAllLines, etc.
            try
            {
                foreach (var item in fileContents)
                {
                    Console.WriteLine(Path.GetFileName(item.FileName) + ":" + item.Text.Length);
                    count++;
                }
            }
            catch (AggregateException ae)
            {
                ae.Handle((ex) =>
                {
                    if (ex is UnauthorizedAccessException)
                    {
                        Console.WriteLine(ex.Message);
                        return true;
                    }
                    return false;
                });
            }

            Console.WriteLine("文件数为 {0}, 耗时 {1} 毫秒", count, watch.ElapsedMilliseconds);
        }
    }
}
