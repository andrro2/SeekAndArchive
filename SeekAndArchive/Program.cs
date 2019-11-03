using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
        private static string archiveDirectory = AppDomain.CurrentDomain.BaseDirectory + "Archive";
        static void Main(string[] args)
        {
            searchedFile = args[0];
            directory = args[1];
            if (Directory.Exists(archiveDirectory))
            {
                Console.WriteLine("Directory exists");
            }
            else
            {
                Directory.CreateDirectory(archiveDirectory);
                Console.WriteLine("Directory created");
            }
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
                        FileSystemWatcher fileWatcher = new FileSystemWatcher(directory, fileName);
                        fileWatcher.Changed += new FileSystemEventHandler(FileChanged);
                        fileWatcher.EnableRaisingEvents = true;
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

        private static void FileChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(e.FullPath + " file changed");
            FileStream sourceFile = File.OpenRead(e.FullPath);
            DateTime dt = DateTime.Now;
            string formattedDateTime = dt.Year + "_" + dt.Month + "_" + dt.Day + "_" + dt.Hour + "_" + dt.Minute + "_" + dt.Second;
            string dest = archiveDirectory + @"\" + e.Name + formattedDateTime + ".gz";
            FileStream destFile = File.Create(dest);
            byte[] buffer = new byte[sourceFile.Length];
            sourceFile.Read(buffer, 0, buffer.Length);
            GZipStream output = new GZipStream(destFile, CompressionMode.Compress);
            output.Write(buffer, 0, buffer.Length);
            sourceFile.Close();
            destFile.Close();
            
            
        }
    }
}
