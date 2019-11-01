using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeekAndArchive
{
    class Program
    {
        public static string searchedFile;
        private static List<String> filePaths = new List<string>();
        public static string directory;

        static void Main(string[] args)
        {
            searchedFile = args[0];
            directory = args[1];
            Console.WriteLine(searchedFile);
            Console.WriteLine(directory);
            SearchForFile(searchedFile, directory);
            Console.WriteLine("Found Files:");
            foreach (string filePath in filePaths)
            {
                Console.WriteLine(filePath);
            }
            Console.ReadLine();
        }

        private static void SearchForFile(string fileName, string directory)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            FileInfo[] directoryFiles = directoryInfo.GetFiles();
            DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();
            if (directoryFiles.Length > 0)
            {
                foreach (FileInfo file in directoryFiles)
                {
                    if (file.Name == fileName)
                    {
                        filePaths.Add(file.FullName);
                    }
                }
            }
            if (subDirectories.Length > 0)
            {
                foreach (DirectoryInfo subdirectory in subDirectories)
                {
                    SearchForFile(fileName, subdirectory.FullName);
                }
            }
        }
    }
}
